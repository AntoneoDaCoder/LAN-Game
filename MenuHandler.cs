using SFML.Graphics;
using SFML.Window;

namespace To_The_Stars_hashtag_version
{
    internal class MenuHandler
    {
        public bool IsActive { get; set; }
        public Menu CurrMenu { get; set; }
        public Button CurrButton { get; set; }
        public Dictionary<string, bool> Parameters { get; set; }
        public Dictionary<string, Menu> Menus { get; set; }
        public MenuHandler()
        {
            Parameters = new();
            Menus = new Dictionary<string, Menu>();
            CurrMenu = null;
            CurrButton = null;
            IsActive = true;
        }
        public bool GetParamValue(string name)
        {
            return Parameters[name];
        }
        public void ChangeParamValue(string name)
        {
            Parameters[name] = !Parameters[name];
        }
        public void Draw(RenderWindow window)
        {
            window.Draw(CurrMenu.Title);
            foreach (var b in CurrMenu.Ui)
                window.Draw(b.ButtonCap);
        }
        public Menu GetMenu(string name)
        {
            return Menus[name];
        }
        public void SelectAction(RenderWindow window, ref bool IsServer, ref bool IsClient)
        {
            if (CurrButton != null)//if any of the buttons has been selected
            {
                if (CurrButton.MenuBind != "")//if button has menu bind
                {
                    CurrMenu = GetMenu(CurrButton.MenuBind);//get menu based on menuBind button parameter
                }
                else if (CurrButton.ParameterBind != "")//else if button has parameter bind
                {
                    ChangeParamValue(CurrButton.ParameterBind);
                }
                else if (CurrButton.StartsGame)
                {
                    IsActive = false;//if the button starts the game, then the menu should be closed
                    if (CurrButton.ButtonCap.DisplayedString == "Start as server")
                        IsServer = true;
                    else
                        if (CurrButton.ButtonCap.DisplayedString == "Connect to host")
                        IsClient = true;

                }
                else if (CurrButton.IsExit)
                {
                    IsActive = false;//if the button closes the game, then the menu should be closed
                    window.Close();//close the game window
                }
            }
        }
        public void AddParameter(string key, bool value)
        {
            Parameters.Add(key, value);
        }
        public void Update(RenderWindow window)
        {
            foreach (var buttons in CurrMenu.Ui)//reset every button in the menu button list
            {
                buttons.ButtonCap.Style = Text.Styles.Regular;
                if (buttons.ParameterBind != "")//reset parameter button color
                    buttons.ButtonCap.FillColor = (GetParamValue(buttons.ParameterBind) ? Color.Green : Color.Red);
            }
            CurrButton = null;//clear pointer to the current button
            foreach (var buttons in CurrMenu.Ui)
            {
                var mousePos = Mouse.GetPosition(window);
                if (buttons.ButtonRect.Contains(mousePos.X, mousePos.Y))
                {
                    buttons.ButtonCap.Style = Text.Styles.Underlined;
                    CurrButton = buttons;
                    break;
                }

            }
            Draw(window);//draw the current menu
        }
        public void SetMenuActive(string name)
        {
            IsActive = true;
            CurrMenu = GetMenu(name);
        }
        public Menu CreateMenu(Font uiFont, string name, string title, uint charSize)
        {
            Menus.Add(name, new(uiFont, title, charSize));
            if (CurrMenu == null) CurrMenu = GetMenu(name);
            return GetMenu(name);
        }
    }
}

