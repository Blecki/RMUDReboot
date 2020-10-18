using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    /// <summary>
    /// A basic door object. Doors are openable. When used as a portal, a door will automatically sync it's open state
    /// with the opposite side of the portal.
    /// </summary>
    public partial class ObjectDecorator
    {
        public static void BasicDoor(MudObject This)
        {
            This.AddNoun("DOOR");

            // Doors can be referred to as 'the open door' or 'the closed door' as appropriate.
            This.AddNoun("CLOSED").When(actor => !This.GetProperty<bool>("open?"));
            This.AddNoun("OPEN").When(actor => This.GetProperty<bool>("open?"));

            This.SetProperty("open?", false);
            This.SetProperty("openable?", true);

            // Maybe some global rules so they aren't duped onto every door?
            This.Check<MudObject, MudObject>("can open?")
                .Last
                .Do((a, b) =>
                {
                    if (This.GetProperty<bool>("open?"))
                    {
                        Core.SendMessage(a, "@already open");
                        return CheckResult.Disallow;
                    }
                    return CheckResult.Allow;
                })
                .Name("Can open doors rule.");

            This.Check<MudObject, MudObject>("can close?")
                .Last
                .Do((a, b) =>
                {
                    if (!This.GetProperty<bool>("open?"))
                    {
                        Core.SendMessage(a, "@already closed");
                        return CheckResult.Disallow;
                    }
                    return CheckResult.Allow;
                })
                .Name("Can close doors rule.");

            This.Perform<MudObject, MudObject>("opened").Do((a, b) =>
            {
                This.SetProperty("open?", true);

                // Doors are usually two-sided. If there is an opposite side, we need to open it and emit appropriate
                // messages.
                var otherSide = Core.FindOppositeSide(This);
                if (otherSide != null)
                {
                    otherSide.SetProperty("open?", true);
                    
                    // This message is defined in the standard actions module. It is perhaps a bit coupled?
                    Core.SendLocaleMessage(otherSide, "@they open", a, This);
                    Core.MarkLocaleForUpdate(otherSide);
                }

                return PerformResult.Continue;
            })
            .Name("Open a door rule");

            This.Perform<MudObject, MudObject>("close").Do((a, b) =>
            {
                This.SetProperty("open?", false);

                // Doors are usually two-sided. If there is an opposite side, we need to close it and emit
                // appropriate messages.
                var otherSide = Core.FindOppositeSide(This);
                if (otherSide != null)
                {
                    otherSide.SetProperty("open?", false);
                    Core.SendLocaleMessage(otherSide, "@they close", a, This);
                    Core.MarkLocaleForUpdate(otherSide);
                }

                return PerformResult.Continue;
            })
            .Name("Close a door rule");
        }
    }
}
