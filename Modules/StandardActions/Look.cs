using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace StandardActionsModule
{
    internal class Look : CommandFactory
    {
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    Or(
                        KeyWord("LOOK"),
                        KeyWord("EXAMINE"),
                        KeyWord("L"),
                        KeyWord("X"),
                        KeyWord("EX")).Stage("I got as far as knowing you wanted to look."),
                    Optional(
                        Or(
                            KeyWord("AT").Stage("I got as far as knowing you wanted to look at something."),
                            RelativeLocation("RELLOC").Stage("I got as far as knowing you wanted to look in, on, under, or behind something."))),
                    Optional(Object("OBJECT", InScope).Stage("I got as far as figuring out the thing involved."))))
                .Name("LOOK")
                .ID("StandardActions:Look")
                .Manual("Take a look around, or at an object; or in, on, under, or behind something.")
                .ProceduralRule((match, actor) =>
                {
                    if (match.ContainsKey("OBJECT"))
                    {
                        if (match.ContainsKey("RELLOC"))
                        {
                            if (Core.GlobalRules.ConsiderCheckRule("can look relloc?", match["ACTOR"], match["OBJECT"], match["RELLOC"]) == SharpRuleEngine.CheckResult.Disallow)
                                return SharpRuleEngine.PerformResult.Stop;
                            Core.GlobalRules.ConsiderPerformRule("look relloc", match["ACTOR"], match["OBJECT"], match["RELLOC"]);
                        }
                        else
                        {
                            if (Core.GlobalRules.ConsiderCheckRule("can examine?", match["ACTOR"], match["OBJECT"]) == SharpRuleEngine.CheckResult.Disallow)
                                return SharpRuleEngine.PerformResult.Stop;
                            Core.GlobalRules.ConsiderPerformRule("describe", match["ACTOR"], match["OBJECT"]);
                        }
                        return SharpRuleEngine.PerformResult.Continue;
                    }
                    else
                    {
                        Core.GlobalRules.ConsiderPerformRule("describe locale", match["ACTOR"], (match["ACTOR"] as MudObject).Location);
                        return SharpRuleEngine.PerformResult.Continue;
                    }
                });
        }

        public static void AtStartup(RuleEngine GlobalRules)
        {
            Core.StandardMessage("nowhere", "You aren't anywhere.");
            Core.StandardMessage("dark", "It's too dark to see.");
            Core.StandardMessage("also here", "Also here: <l0>.");
            Core.StandardMessage("on which", "(on which is <l0>)");
            Core.StandardMessage("obvious exits", "Obvious exits:");
            Core.StandardMessage("through", "through <the0>");
            Core.StandardMessage("to", "to <the0>");
            Core.StandardMessage("dont see that", "I don't see that here.");
            Core.StandardMessage("cant look relloc", "You can't look <s0> that.");
            Core.StandardMessage("is closed error", "^<the0> is closed.");
            Core.StandardMessage("relloc it is", "^<s0> <the1> is..");
            Core.StandardMessage("nothing relloc it", "There is nothing <s0> <the1>.");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("describe in locale", "[Actor, Item] : Generate a locale description for the item.", "actor", "item");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("describe locale", "[Actor, Room] : Generates a description of the locale.", "actor", "room");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .When((viewer, room) => room == null)
                .Do((viewer, room) =>
                {
                    MudObject.SendMessage(viewer, "@nowhere");
                    return SharpRuleEngine.PerformResult.Stop;
                })
                .Name("Can't describe the locale if there isn't one rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .Do((viewer, room) =>
                {
                    GlobalRules.ConsiderPerformRule("update", room);
                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("Update room lighting before generating description rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .Do((viewer, room) =>
                {
                    if (!String.IsNullOrEmpty(room.GetProperty<String>("short"))) MudObject.SendMessage(viewer, room.GetProperty<String>("short"));
                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("Display room name rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .When((viewer, room) => room.GetProperty<LightingLevel>("light") == LightingLevel.Dark)
                .Do((viewer, room) =>
                {
                    MudObject.SendMessage(viewer, "@dark");
                    return SharpRuleEngine.PerformResult.Stop;
                })
                .Name("Can't see in darkness rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .Do((viewer, room) =>
                {
                    GlobalRules.ConsiderPerformRule("describe", viewer, room);
                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("Include describe rules in locale description rule.");

            var describingLocale = false;

            GlobalRules.Value<MudObject, MudObject, String, String>("printed name")
                .When((viewer, container, article) => describingLocale && (container.LocationsSupported & RelativeLocations.On) == RelativeLocations.On)                    
                .Do((viewer, container, article) =>
                    {
                        var subObjects = new List<MudObject>(container.EnumerateObjects(RelativeLocations.On)
                        .Where(t => GlobalRules.ConsiderCheckRule("should be listed?", viewer, t) == SharpRuleEngine.CheckResult.Allow));

                        if (subObjects.Count > 0)
                            return container.GetProperty<String>("short") + " " + Core.FormatMessage(viewer, Core.GetMessage("on which"), subObjects);
                        else
                            return container.GetProperty<String>("short");
                    })
                    .Name("List contents of container after name when describing locale rule");

            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("should be listed in locale?", "[Viewer, Item] : When describing a room, or the contents of a container, should this item be listed?");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
                .When((viewer, item) => System.Object.ReferenceEquals(viewer, item))
                .Do((viewer, item) => SharpRuleEngine.CheckResult.Disallow)
                .Name("Don't list yourself rule.");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
               .When((viewer, item) => item.GetProperty<bool>("scenery?"))
               .Do((viewer, item) => SharpRuleEngine.CheckResult.Disallow)
               .Name("Don't list scenery objects rule.");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
                .When((viewer, item) => item.GetProperty<bool>("portal?"))
                .Do((viewer, item) => SharpRuleEngine.CheckResult.Disallow)
                .Name("Don't list portals rule.");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
               .Do((viewer, item) => SharpRuleEngine.CheckResult.Allow)
               .Name("List objects by default rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .Do((viewer, room) =>
                {
                    var visibleThings = room.EnumerateObjects(RelativeLocations.Contents)
                        .Where(t => GlobalRules.ConsiderCheckRule("should be listed in locale?", viewer, t) == SharpRuleEngine.CheckResult.Allow);

                    var normalContents = new List<MudObject>();

                    foreach (var thing in visibleThings)
                    {
                        Core.BeginOutputQuery();
                        GlobalRules.ConsiderPerformRule("describe in locale", viewer, thing);
                        if (!Core.CheckOutputQuery()) normalContents.Add(thing);
                    }

                    if (normalContents.Count > 0)
                    {
                        describingLocale = true;
                        MudObject.SendMessage(viewer, "@also here", normalContents);
                        describingLocale = false;
                    }

                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("List contents of room rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .Last
                .Do((viewer, room) =>
                {
                    if (room.EnumerateObjects().Where(l => l.GetProperty<bool>("portal?")).Count() > 0)
                    {
                        MudObject.SendMessage(viewer, "@obvious exits");

                        foreach (var link in room.EnumerateObjects<MudObject>().Where(l => l.GetProperty<bool>("portal?")))
                        {
                            var builder = new StringBuilder();
                            builder.Append("  ^");
                            builder.Append(link.GetProperty<Direction>("link direction").ToString());

                            if (!link.GetProperty<bool>("link anonymous?"))
                                builder.Append(" " + Core.FormatMessage(viewer, Core.GetMessage("through"), link));

                            var destinationRoom = MudObject.GetObject(link.GetProperty<String>("link destination"));
                            if (destinationRoom != null)
                                builder.Append(" " + Core.FormatMessage(viewer, Core.GetMessage("to"), destinationRoom));

                            MudObject.SendMessage(viewer, builder.ToString());
                        }
                    }

                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("List exits in locale description rule.");

            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("can examine?", "[Actor, Item] : Can the viewer examine the item?", "actor", "item");

            GlobalRules.Check<MudObject, MudObject>("can examine?")
                .First
                .Do((viewer, item) => MudObject.CheckIsVisibleTo(viewer, item))
                .Name("Can't examine what isn't here rule.");

            GlobalRules.Check<MudObject, MudObject>("can examine?")
                .Last
                .Do((viewer, item) => SharpRuleEngine.CheckResult.Allow)
                .Name("Default can examine everything rule.");

            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject, RelativeLocations>("can look relloc?", "[Actor, Item, Relative Location] : Can the actor look in/on/under/behind the item?", "actor", "item", "relloc");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .Do((actor, item, relloc) => MudObject.CheckIsVisibleTo(actor, item))
                .Name("Container must be visible rule.");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .When((actor, item, relloc) => (item.LocationsSupported & relloc) != relloc)
                .Do((actor, item, relloc) =>
                {
                    MudObject.SendMessage(actor, "@cant look relloc", Relloc.GetRelativeLocationName(relloc));
                    return SharpRuleEngine.CheckResult.Disallow;
                })
                .Name("Container must support relloc rule.");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .When((actor, item, relloc) => (relloc == RelativeLocations.In) && !item.GetProperty<bool>("open?"))
                .Do((actor, item, relloc) =>
                {
                    MudObject.SendMessage(actor, "@is closed error", item);
                    return SharpRuleEngine.CheckResult.Disallow;
                })
                .Name("Container must be open to look in rule.");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .Do((actor, item, relloc) => SharpRuleEngine.CheckResult.Allow)
                .Name("Default allow looking relloc rule.");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject, RelativeLocations>("look relloc", "[Actor, Item, Relative Location] : Handle the actor looking on/under/in/behind the item.", "actor", "item", "relloc");

            GlobalRules.Perform<MudObject, MudObject, RelativeLocations>("look relloc")
                .Do((actor, item, relloc) =>
                {
                    var contents = new List<MudObject>(item.EnumerateObjects(relloc));

                    if (contents.Count > 0)
                    {
                        MudObject.SendMessage(actor, "@relloc it is", Relloc.GetRelativeLocationName(relloc), item);
                        foreach (var thing in contents)
                            MudObject.SendMessage(actor, "  <a0>", thing);
                    }
                    else
                        MudObject.SendMessage(actor, "@nothing relloc it", Relloc.GetRelativeLocationName(relloc), item);

                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("List contents in relative location rule.");

        }
    }
}
