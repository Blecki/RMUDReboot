using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RMUD;

class Program
{
    static void Main(string[] args)
    {
        var driver = new RMUD.SinglePlayer.Driver();
        driver.Start(typeof(Skrogard.Game).Assembly, Write);
        while (driver.IsRunning)
            driver.Input(Console.ReadLine());
        Console.WriteLine("[Press any key to exit..]");
        Console.ReadKey();
    }

    private enum OutputStates
    {
        Normal,
        ReadingColor
    }

    private static OutputStates OutputState = OutputStates.Normal;
    private static String ColorName = "";

    private static void WriteCharacter(char C)
    {
        if (OutputState == OutputStates.ReadingColor)
        {
            if (C == ';')
            {
                var color = ConsoleColor.White;
                if (Enum.TryParse(ColorName, out color))
                    Console.ForegroundColor = color;
                OutputState = OutputStates.Normal;
            }
            else
                ColorName += C;
        }
        else if (C == '$')
        {
            OutputState = OutputStates.ReadingColor;
            ColorName = "";
        }
        else
            Console.Write(C);
    }

    static void Write(String Text)
    {
        foreach (var c in Text)
            WriteCharacter(c);
    }
}