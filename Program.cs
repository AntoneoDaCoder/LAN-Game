using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Net.NetworkInformation;
using System.Net;
namespace To_The_Stars_hashtag_version;
internal class Program
{
    static RenderWindow Window = new(new VideoMode(600, 1200), "To The Stars");//create window 600*1200, on which you can draw 2d objects
    static public Font uiFont = new Font("RSS/Fonts/Pixelopolis9000-PzZZ.ttf");
    static Mutex Mutex = new Mutex();
    static Controller Controller = new();
    static View View = new();
    static volatile Model Model;
    static public bool IsClient = false, IsServer = false;
    static public string MachineIp = GetMachineIP();
    static public void OnClose(object? sender, EventArgs e)
    {
        RenderWindow window = (RenderWindow)sender;
        window.Close();
        Controller.EndTransmission();
    }
    static public void OnKeyPress(object? sender, KeyEventArgs e)
    {
        if (!Model.Handler.IsActive)
        {
            HandleKeyPress(e);
        }
    }
    static public bool CheckInternetConnection()
    {
        try
        {
            Ping myPing = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            return (reply.Status == IPStatus.Success);
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static string GetMachineIP()
    {
        if (Program.CheckInternetConnection())
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.Last().ToString();
        }
        return "";
    }
    static public void OnKeyRelease(object? sender, KeyEventArgs e)
    {
        if (!Model.Handler.IsActive)
        {
            HandleKeyRelease(e);
        }
    }
    static public void OnMouseKeyRelease(object? sender, MouseButtonEventArgs e)
    {
        if (Model.Handler.IsActive)
        {
            HandleMenuInput(e);
        }
    }
    static void HandleMenuInput(MouseButtonEventArgs e)
    {
        Storage.ResetSong();//reset current song
        if (e.Button == Mouse.Button.Left)//if user has released left mouse button
            Model.Handler.SelectAction(Window, ref IsServer, ref IsClient);//handle user action
        if (!Model.Handler.IsActive && Window.IsOpen)//if user has either returned to menu or pressed the start button
        {
            Storage.SetSFXStatus(Model.Handler.GetParamValue("sound"), Model.Handler.GetParamValue("music"));//update sound and music settings
        }
    }
    static void HandleKeyPress(KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Left:
                Model.Player.SetEntityState("left");//now player is moving left
                Client.CurrPlayerState = "left";
                break;
            case Keyboard.Key.Right:
                Model.Player.SetEntityState("right");//now player is moving right
                Client.CurrPlayerState = "right";
                break;
            case Keyboard.Key.Up:
                Model.Player.SetEntityState("up");//now player is moving up
                Client.CurrPlayerState = "up";
                break;
            case Keyboard.Key.Down:
                Model.Player.SetEntityState("down");//now player is moving down
                Client.CurrPlayerState = "down";
                break;
            case Keyboard.Key.Space:
                Model.Player.IsShooting = true;//shoot a bullet
                Client.IsShooting = true;
                break;
            case Keyboard.Key.Escape:
                Model.Handler.SetMenuActive("main");//return to the main menu
                break;

        }
    }
    static void HandleKeyRelease(KeyEventArgs e)
    {
        if (e.Code != Keyboard.Key.Space)//if released key was arrow
        {
            Animator.ResetCurrentAnimation(Model.Player);//end current animation
            Client.ResetAnim = true;
        }
        else
        if (e.Code == Keyboard.Key.Space)
        {
            Model.Player.IsShooting = false;
            Client.IsShooting = false;
        }
    }
    static void CreateMainMenu()
    {
        var mainMenu = Model.Handler.CreateMenu(uiFont, "main", "To The Stars", 100);//create new object in menus list
        float offset = mainMenu.GetTitleRect().Width / 2;//set title offset
        mainMenu.SetTitlePos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 4);//place the title on the screen

        var but1 = mainMenu.AddButton(uiFont, "Start", 80, false, false, "gamemode");//add new button to the created menu
        offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = mainMenu.AddButton(uiFont, "Settings", 80, false, false, "settings");//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = mainMenu.AddButton(uiFont, "Exit", 80, false, true);//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//place the created button on the screen
    }
    static void CreateSettingsMenu()
    {
        var settingsMenu = Model.Handler.CreateMenu(uiFont, "settings", "", 80);//create new object in menus list

        var but1 = settingsMenu.AddButton(uiFont, "Sound", 80, false, false, "", "sound");//add new button to the created menu
        float offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = settingsMenu.AddButton(uiFont, "Music", 80, false, false, "", "music");//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = settingsMenu.AddButton(uiFont, "Back", 80, false, false, "main");//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//place the created button on the screen
    }
    static void CreateGameOverMenu()
    {
        var gameOverMenu = Model.Handler.CreateMenu(uiFont, "gameover", "", 100);//create new object in menus list

        var but1 = gameOverMenu.AddButton(uiFont, "Restart", 80, true);//add new button to the created menu
        float offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = gameOverMenu.AddButton(uiFont, "Main Menu", 80, false, false, "main");//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = gameOverMenu.AddButton(uiFont, "Exit", 80, false, true);//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//add new button to the created menu
    }
    static void CreateGameModeMenu()
    {
        var gameOverMenu = Model.Handler.CreateMenu(uiFont, "gamemode", "", 100);//create new object in menus list
        var but1 = gameOverMenu.AddButton(uiFont, "Solo", 80, true);//add new button to the created menu
        float offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = gameOverMenu.AddButton(uiFont, "Multiplayer", 80, false, false, "multiplayeroptions");//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = gameOverMenu.AddButton(uiFont, "Back", 80, false, false, "main");//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//add new button to the created menu
    }
    static void CreateMultiplayerOptionsMenu()
    {
        var gameOverMenu = Model.Handler.CreateMenu(uiFont, "multiplayeroptions", "", 100);//create new object in menus list
        var but1 = gameOverMenu.AddButton(uiFont, "Become host", 80, false, false, "sinfo");//add new button to the created menu
        float offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = gameOverMenu.AddButton(uiFont, "Connect to host", 80, true);//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = gameOverMenu.AddButton(uiFont, "Back", 80, false, false, "gamemode");//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//add new button to the created menu
    }
    static void CreateServerInfoMenu()
    {
        var serverInfoMenu = Model.Handler.CreateMenu(uiFont, "sinfo", MachineIp, 100);//create new object in menus list
        float offset = serverInfoMenu.GetTitleRect().Width / 2;//set title offset
        serverInfoMenu.SetTitlePos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 4);//place the title on the screen

        var but1 = serverInfoMenu.AddButton(uiFont, "Start as server", 80, true);//add new button to the created menu
        offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = serverInfoMenu.AddButton(uiFont, "Back", 80, false, false, "multiplayeroptions");//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = serverInfoMenu.AddButton(uiFont, "Exit", 80, false, true);//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//add new button to the created menu
    }
    static void CreateConnectionErrorMenu()
    {
        var connectionErrorMenu = Model.Handler.CreateMenu(uiFont, "cerror", "Connection Error", 100);//create new object in menus list
        float offset = connectionErrorMenu.GetTitleRect().Width / 2;//set title offset
        connectionErrorMenu.SetTitlePos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 4);//place the title on the screen

        var but1 = connectionErrorMenu.AddButton(uiFont, "Solo", 80, true);//add new button to the created menu
        offset = but1.ButtonRect.Width / 2;//set button offset
        but1.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 90);//place the created button on the screen

        var but2 = connectionErrorMenu.AddButton(uiFont, "Back to main", 80, false, false, "main");//add new button to the created menu
        offset = but2.ButtonRect.Width / 2;//set button offset
        but2.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 180);//place the created button on the screen

        var but3 = connectionErrorMenu.AddButton(uiFont, "Exit", 80, false, true);//add new button to the created menu
        offset = but3.ButtonRect.Width / 2;//set button offset
        but3.SetPos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 3 + 270);//add new button to the created menu
    }
    static void AddMenus()
    {
        CreateMainMenu();//create main menu in menu handler
        CreateSettingsMenu();//create settings menu in menu handler
        CreateGameOverMenu();//create gameover menu in manu handler
        CreateGameModeMenu();
        CreateMultiplayerOptionsMenu();
        CreateConnectionErrorMenu();
        CreateServerInfoMenu();
    }
    static void LaunchGame()
    {
        Model = new();
        Controller.RestartGame(Model);//restart the game
        Controller.ChooseGameMode(Model, Mutex, Window.Size.X, Window.Size.Y);
        Clock gameClock = new();//create the game clock
        while (Window.IsOpen)//execute while the window is open
        {

            Window.DispatchEvents();
            double elapsedTime = gameClock.Restart().AsMilliseconds();//get elapsed time
            if (Model.Handler.IsActive)
            {
                Storage.ResetSong();
                Controller.EndTransmission();
                return;
            }
            else if (Window.IsOpen)
            {
                bool gameOver = (Model.Player.Hp == 0) && Model.Player.AnimHasEnded; //gameover condition
                if (!gameOver)//if player is still alive
                {
                   Mutex.WaitOne();
                    Model.EnemyUpgradeLvl = Model.Player.Score > 10000 ? 1 : 0;
                    Controller.RestoreEnemies(Model, Window.Size.X);//this method refills enemy list, if there are no enemies left
                    Storage.UpdateSong(Entity.GetRandNum(0, 11));//this methods chooses the next song if the current one has ended
                    View.UpdateText(Model);
                    Controller.Update(Model, elapsedTime);
                    View.UpdateGameView(Model, Window);
                    Mutex.ReleaseMutex();
                }
                else
                {
                    Storage.ResetSong();
                    Model.Handler.SetMenuActive("gameover");//select gameover menu
                    Model.Handler.CurrMenu.Title.DisplayedString = ("Score: " + Model.Player.Score.ToString());//set string (total score)
                    float offset = Model.Handler.CurrMenu.GetTitleRect().Width / 2;//centering the string
                    Model.Handler.CurrMenu.SetTitlePos(Window.Size.X / 2 - offset, 100 + Window.Size.Y / 4);//set string position on the screen
                    Controller.EndTransmission();
                    return;
                }
            }

        }
    }

    static void OpenMenu()
    {
        while (Window.IsOpen)
        {
            IsClient = false;
            IsServer = false;
            Window.DispatchEvents();
            View.UpdateMenuView(Window);
            if (!Model.Handler.IsActive)
                LaunchGame();
        }
    }
    static void Main()
    {
        Window.SetFramerateLimit(90);//restrict maximum fps, otherwise program will consume all free resources
        Model.Background = new(0.2, Window.Size.Y);//create an instance of background handler class
        Model.Handler.AddParameter("sound", true);//add variable parameter to menu handler instance
        Model.Handler.AddParameter("music", true);
        AddMenus();
        Window.Closed += OnClose;
        Window.KeyPressed += OnKeyPress;
        Window.KeyReleased += OnKeyRelease;
        Window.MouseButtonReleased += OnMouseKeyRelease;
        OpenMenu();
        Window.Dispose();
    }
}


