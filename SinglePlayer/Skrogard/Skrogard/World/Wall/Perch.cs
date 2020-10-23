using RMUD;

namespace World.Wall
{

    public class Perch : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Perch");
            SetProperty("long", "To the east: The land of Skogard. Gray, dismal, heaped with the discarded metal of Valhalla. To the west: Hel. The blasted wasteland of the Jotnar. Somehow, from way up here, it actually looks green. Distant snow capped peaks stab at a clear sky, and the land beyond the wall is dotted with glistening lakes. There's no railing here, and you feel a sudden urge to jump. You could look down.");

            AddScenery("What are those shapes at the bottom of the wall? Are those bones?", "down");

            OpenLink(Direction.EAST, "Wall.Overhang");

            Perform<MudObject>("jump").Do((actor) =>
            {
                Core.SendMessage(actor, "And why shouldn't you jump? Oh, right. Because of the sudden stop.");
                return PerformResult.Stop;
            });
        }
    }
}