using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace StandardActionsModule.Look
{
    internal class LocaleRules
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("describe in locale", "[Actor, Item] : Generate a locale description for the item.", "actor", "item");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("describe locale", "[Actor, Room] : Generates a description of the locale.", "actor", "room");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .When((viewer, room) => room == null)
                .Do((viewer, room) =>
                {
                    MudObject.SendMessage(viewer, "@nowhere");
                    return PerformResult.Stop;
                })
                .Name("Can't describe the locale if there isn't one rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .Do((viewer, room) =>
                {
                    GlobalRules.ConsiderPerformRule("update", room);
                    return PerformResult.Continue;
                })
                .Name("Update room lighting before generating description rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .Do((viewer, room) =>
                {
                    if (!String.IsNullOrEmpty(room.GetProperty<String>("short"))) MudObject.SendMessage(viewer, room.GetProperty<String>("short"));
                    return PerformResult.Continue;
                })
                .Name("Display room name rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .First
                .When((viewer, room) => room.GetProperty<LightingLevel>("light") == LightingLevel.Dark)
                .Do((viewer, room) =>
                {
                    MudObject.SendMessage(viewer, "@dark");
                    return PerformResult.Stop;
                })
                .Name("Can't see in darkness rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .Do((viewer, room) =>
                {
                    GlobalRules.ConsiderPerformRule("describe", viewer, room);
                    return PerformResult.Continue;
                })
                .Name("Include describe rules in locale description rule.");

            var describingLocale = false;

            GlobalRules.Value<MudObject, MudObject, String, String>("printed name")
                .When((viewer, container, article) => describingLocale && (container.LocationsSupported & RelativeLocations.ON) == RelativeLocations.ON)                    
                .Do((viewer, container, article) =>
                    {
                        var subObjects = new List<MudObject>(container.EnumerateObjects(RelativeLocations.ON)
                        .Where(t => GlobalRules.ConsiderCheckRule("should be listed?", viewer, t) == CheckResult.Allow));

                        if (subObjects.Count > 0)
                            return container.GetProperty<String>("short") + " " + Core.FormatMessage(viewer, Core.GetMessage("on which"), subObjects);
                        else
                            return container.GetProperty<String>("short");
                    })
                    .Name("List contents of container after name when describing locale rule");

            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("should be listed in locale?", "[Viewer, Item] : When describing a room, or the contents of a container, should this item be listed?");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
                .When((viewer, item) => System.Object.ReferenceEquals(viewer, item))
                .Do((viewer, item) => CheckResult.Disallow)
                .Name("Don't list yourself rule.");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
               .When((viewer, item) => item.GetProperty<bool>("scenery?"))
               .Do((viewer, item) => CheckResult.Disallow)
               .Name("Don't list scenery objects rule.");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
                .When((viewer, item) => item.GetProperty<bool>("portal?"))
                .Do((viewer, item) => CheckResult.Disallow)
                .Name("Don't list portals rule.");

            GlobalRules.Check<MudObject, MudObject>("should be listed in locale?")
               .Do((viewer, item) => CheckResult.Allow)
               .Name("List objects by default rule.");

            GlobalRules.Perform<MudObject, MudObject>("describe locale")
                .Do((viewer, room) =>
                {
                    var visibleThings = room.EnumerateObjects(RelativeLocations.CONTENTS)
                        .Where(t => GlobalRules.ConsiderCheckRule("should be listed in locale?", viewer, t) == CheckResult.Allow);

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

                    return PerformResult.Continue;
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

                    return PerformResult.Continue;
                })
                .Name("List exits in locale description rule.");
        }
    }
}
