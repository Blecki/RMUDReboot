using RMUD;
using static RMUD.Core;

public class settings : RMUD.Settings
{
    public override void Initialize()
    {
        Banner = "~~== DELMUD PROTOTYPE ==~~";
        MessageOfTheDay = "register username - Create a new account.\r\nlogin username - Log into an existing account.";
        TelnetPort = 8669;
        NewPlayerStartRoom = "palantine/antechamber";
        PlayerBaseObject = "player_base";
    }
}