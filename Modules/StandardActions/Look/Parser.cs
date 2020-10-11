using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace StandardActionsModule.Look
{
    internal class Parser : CommandFactory
    {
        public enum LookBranch
        {
            LOCALE,
            OBJECT,
            RELLOC,
            LINK
        };

        public override void Create(CommandParser Parser)
        {

            Parser.AddCommand(
                Sequence(
                    Or(
                        KeyWord("LOOK"),
                        KeyWord("EXAMINE"),
                        KeyWord("L"),
                        KeyWord("X"),
                        KeyWord("EX")
                    ).Stage("I got as far as knowing you wanted to look."),
                    Or(
                        SetValue("LOOKBRANCH", LookBranch.LOCALE),
                        Sequence(
                            SetValue("LOOKBRANCH", LookBranch.OBJECT),
                            KeyWord("AT"),
                            Object("OBJECT", InScope).Stage("I think you were trying to look at something, but I couldn't tell what.")),
                        Sequence(
                            SetValue("LOOKBRANCH", LookBranch.RELLOC),
                            RelativeLocation("RELLOC"),
                            Object("OBJECT", InScope).Stage("I think you were trying to look [RELLOC] something, but I couldn't tell what.")),
                        Sequence(
                            SetValue("LOOKBRANCH", LookBranch.LINK),
                            Cardinal("DIRECTION"))
                        )
                    )
                )
                .Name("LOOK")
                .ID("StandardActions:Look")
                .Manual("Take a look around, or at an object; or in, on, under, or behind something.")
                .ProceduralRule((match, actor) =>
                {
                    switch ((match["LOOKBRANCH"] as LookBranch?).Value)
                    {
                        case LookBranch.LOCALE:
                            Core.GlobalRules.ConsiderPerformRule("describe locale", match["ACTOR"], (match["ACTOR"] as MudObject).Location);
                            return SharpRuleEngine.PerformResult.Continue;
                        case LookBranch.OBJECT:
                            if (Core.GlobalRules.ConsiderCheckRule("can examine?", match["ACTOR"], match["OBJECT"]) == SharpRuleEngine.CheckResult.Disallow)
                                return SharpRuleEngine.PerformResult.Stop;
                            Core.GlobalRules.ConsiderPerformRule("describe", match["ACTOR"], match["OBJECT"]);
                            return SharpRuleEngine.PerformResult.Continue;
                        case LookBranch.RELLOC:
                            if (Core.GlobalRules.ConsiderCheckRule("can look relloc?", match["ACTOR"], match["OBJECT"], match["RELLOC"]) == SharpRuleEngine.CheckResult.Disallow)
                                return SharpRuleEngine.PerformResult.Stop;
                            Core.GlobalRules.ConsiderPerformRule("look relloc", match["ACTOR"], match["OBJECT"], match["RELLOC"]);
                            return SharpRuleEngine.PerformResult.Continue;
                        case LookBranch.LINK:
                            {
                                var direction = match["DIRECTION"] as Direction?;
                                var link = actor.Location.EnumerateObjects().FirstOrDefault(thing => thing.GetProperty<bool>("portal?") && thing.GetProperty<Direction>("link direction") == direction.Value);
                                match.Upsert("LINK", link);
                                var destination = MudObject.GetObject(link.GetProperty<String>("link destination"));
                                if (destination == null)
                                {
                                    MudObject.SendMessage(actor, "@bad link");
                                    return SharpRuleEngine.PerformResult.Stop;
                                }
                                if (Core.GlobalRules.ConsiderCheckRule("can examine?", match["ACTOR"], match["LINK"]) == SharpRuleEngine.CheckResult.Disallow)
                                    return SharpRuleEngine.PerformResult.Stop;
                                Core.GlobalRules.ConsiderPerformRule("describe", match["ACTOR"], match["LINK"]);
                            }
                            return SharpRuleEngine.PerformResult.Continue;
                        default:
                            return SharpRuleEngine.PerformResult.Continue;
                    }
                });
        }
    }
}
