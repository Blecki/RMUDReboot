using System.Linq;
using System.Collections.Generic;
using System;
using RMUD;

namespace AdminModule
{
    internal class Sonar : CommandFactory
    {
        private const int MapWidth = 11;
        private const int MapHeight = 5;
        private const int RoomWidth = 9;
        private const int RoomHeight = 7;
        private const int RoomMidWidth = 4;
        private const int RoomMidHeight = 3;
        private const int RenderWidth = MapWidth * RoomWidth + 2;
        private const int RenderHeight = MapHeight * RoomHeight + 2;

        private static MudObject[,] GriddedMap = new MudObject[MapWidth, MapHeight];
        private static List<MudObject> VisitedRooms = new List<MudObject>();

        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(KeyWord("SONAR"))
                .ProceduralRule((match, actor) =>
                {
                    ClearGrid();
                    if (actor.Location.HasValue(out var loc))
                        PopulateGrid(loc, MapWidth / 2, MapHeight / 2);

                    // Prep output grid
                    var mapGrid = new int[RenderWidth, RenderHeight];
                    for (int y = 0; y < RenderHeight; ++y)
                        for (int x = 0; x < RenderWidth; ++x)
                            mapGrid[x, y] = ' ';

                    // Draw frame
                    for (int y = 1; y < RenderHeight - 1; ++y)
                    {
                        mapGrid[0, y] = '|';
                        mapGrid[RenderWidth - 1, y] = '|';
                    }

                    for (int x = 1; x < RenderWidth - 1; ++x)
                    {
                        mapGrid[x, 0] = '-';
                        mapGrid[x, RenderHeight - 1] = '-';
                    }

                    mapGrid[0, 0] = '+';
                    mapGrid[0, RenderHeight - 1] = '+';
                    mapGrid[RenderWidth - 1, 0] = '+';
                    mapGrid[RenderWidth - 1, RenderHeight - 1] = '+';

                    // Render Map
                    var roomLegend = new List<String>();
                    for (var x = 0; x < MapWidth; ++x)
                        for (var y = 0; y < MapHeight; ++y)
                            if (GriddedMap[x, y] != null)
                            {
                                roomLegend.Add(Pad(FindSymbol(GriddedMap[x, y]), RoomWidth - 4) + " - " + GriddedMap[x, y].Short);
                                RenderRoom(GriddedMap[x, y], mapGrid, x * RoomWidth + 1, y * RoomHeight + 1);
                            }

                    PlaceSymbol(mapGrid, 1 + (MapWidth / 2) * RoomWidth + RoomMidWidth, 1 + (MapHeight / 2) * RoomHeight + RoomMidHeight, '@');

                    // Dump map to screen.
                    var builder = new System.Text.StringBuilder();

                    for (int y = 0; y < RenderHeight; ++y)
                    {
                        for (int x = 0; x < RenderWidth; ++x)
                            builder.Append((char)mapGrid[x, y]);
                        builder.Append("\r\n");
                    }

                    foreach (var entry in roomLegend)
                        builder.Append(entry + "\r\n");

                    Core.SendMessage(actor, builder.ToString());

                    ClearGrid();

                    return PerformResult.Continue;
                }, "Implement sonar device rule.");
        }

        private static void ClearGrid()
        {
            for (var x = 0; x < MapWidth; ++x)
                for (var y = 0; y < MapHeight; ++y)
                    GriddedMap[x, y] = null;
            VisitedRooms.Clear();
        }

        private static void PopulateGrid(MudObject Location, int X, int Y)
        {
            if (X < 0 || X >= MapWidth || Y < 0 || Y >= MapHeight) return;
            if (GriddedMap[X, Y] != null) return;
            if (Location == null) return;
            if (VisitedRooms.Contains(Location)) return;
            VisitedRooms.Add(Location);
            GriddedMap[X, Y] = Location;

            foreach (var link in Location.EnumerateObjects().Where(t => t.HasProperty("link direction")))
            {
                var destinationName = link.GetProperty<string>("link destination");
                var destination = MudObject.GetObject(destinationName);
                var direction = link.GetProperty<RMUD.Direction>("link direction");
                var vec = Link.GetAsVector(direction);
                PopulateGrid(destination, X + vec.X, Y + vec.Y);
            }
        }

        private static String Pad(String Symbol, int Width)
        {
            while (Symbol.Length < Width)
                Symbol = Symbol + " ";
            return Symbol;
        }

        private static void PlaceSymbol(int[,] MapGrid, int X, int Y, int Symbol)
        {
            MapGrid[X, Y] = Symbol;
        }

        private static void PlaceString(int[,] MapGrid, int X, int Y, String Str)
        {
            for (var i = 0; i < Str.Length && X + i < RenderWidth - 1; ++i)
                PlaceSymbol(MapGrid, X + i, Y, Str[i]);
        }

        private static String ComposeSymbol(String[] Pieces, int GoalLength)
        {
            if (Pieces.Length == 0) return new string('?', GoalLength);
            var r = "";
            var piecesLeft = Pieces.Length;
            foreach (var piece in Pieces)
            {
                var targetPieceLength = (int)(Math.Ceiling((float)(GoalLength - r.Length) / (float)piecesLeft));
                r += piece.Substring(0, Math.Min(targetPieceLength, piece.Length));
                piecesLeft -= 1;
            }
            return r.ToUpper();
        }

        private static String FindSymbol(RMUD.MudObject Location)
        {
            if (Location == null) return "???";
            var namePart = Core.StripColorTags(Location.Short).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return ComposeSymbol(namePart, RoomWidth - 4);
        }

        private static void RenderRoom(MudObject Location, int[,] MapGrid, int X, int Y)
        { 
            var symbol = FindSymbol(Location);

            
            PlaceString(MapGrid, X + 2, Y + 2, symbol);
            PlaceSymbol(MapGrid, X + 1, Y + 1, '+');
            PlaceSymbol(MapGrid, X + RoomWidth - 2, Y + 1, '+');
            PlaceSymbol(MapGrid, X + RoomWidth - 2, Y + RoomHeight - 2, '+');
            PlaceSymbol(MapGrid, X + 1, Y + RoomHeight - 2, '+');
            for (var i = 0; i < RoomWidth - 4; ++i)
            {
                PlaceSymbol(MapGrid, X + 2 + i, Y + 1, '-');
                PlaceSymbol(MapGrid, X + 2 + i, Y + RoomHeight - 2, '-');
            }
            for (var i = 0; i < RoomHeight - 4; ++i)
            {
                PlaceSymbol(MapGrid, X + 1, Y + 2 + i, '|');
                PlaceSymbol(MapGrid, X + RoomWidth - 2, Y + 2 + i, '|');
            }

            if (Location != null)
            {
                var bonusLinks = "";

                foreach (var link in Location.EnumerateObjects().Where(t => t.HasProperty("link direction")))
                {
                    var destinationName = link.GetProperty<string>("link destination");
                    var destination = MudObject.GetObject(destinationName);
                    var direction = link.GetProperty<RMUD.Direction>("link direction");

                    if (Link.IsBonusDirection(direction))
                    {
                        bonusLinks += direction.ToString()[0];
                    }
                    else
                    {
                        var directionVector = RMUD.Link.GetAsVector(direction);
                        PlaceEdge(MapGrid, X, Y, direction);
                    }

                    var bonusSpace = RoomWidth - 4;
                    var bonusRows = (int)Math.Ceiling((float)bonusLinks.Length / (float)bonusSpace);
                    var bonusY = Y + RoomHeight - 2 - bonusRows;
                    var bonusX = 0;
                    for (var i = 0; i < bonusLinks.Length; ++i)
                    {
                        PlaceSymbol(MapGrid, X + 2 + bonusX, bonusY, bonusLinks[i]);
                        bonusX += 1;
                        if (bonusX >= bonusSpace)
                        {
                            bonusX = 0;
                            bonusY += 1;
                        }
                    }
                }
            }
        }

        private static void PlaceEdge(int[,] MapGrid, int X, int Y, RMUD.Direction Direction)
        {
            switch (Direction)
            {
                case RMUD.Direction.NORTH:
                    PlaceSymbol(MapGrid, X + RoomMidWidth, Y, '|');
                    break;
                case RMUD.Direction.SOUTH:
                    PlaceSymbol(MapGrid, X + RoomMidWidth, Y + RoomHeight - 1, '|');
                    break;
                case RMUD.Direction.EAST:
                    PlaceSymbol(MapGrid, X + RoomWidth - 1, Y + RoomMidHeight, '-');
                    break;
                case RMUD.Direction.WEST:
                    PlaceSymbol(MapGrid, X, Y + RoomMidHeight, '-');
                    break;
                case RMUD.Direction.NORTHEAST:
                    PlaceSymbol(MapGrid, X + RoomWidth - 1, Y, '/');
                    break;
                case RMUD.Direction.SOUTHWEST:
                    PlaceSymbol(MapGrid, X, Y + RoomHeight - 1, '/');
                    break;
                case RMUD.Direction.NORTHWEST:
                    PlaceSymbol(MapGrid, X, Y, '\\');
                    break;
                case RMUD.Direction.SOUTHEAST:
                    PlaceSymbol(MapGrid, X + RoomWidth - 1, Y + RoomHeight - 1, '\\');
                    break;
                default:
                    //PlaceSymbol(MapGrid, X + 2, Y, '|');
                    break;
            }
        }
    }
}