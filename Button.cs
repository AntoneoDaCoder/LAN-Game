using SFML.Graphics;
namespace To_The_Stars_hashtag_version
{
    internal class Button
    {
        public Text ButtonCap { get; set; }
        public string MenuBind { get; set; }
        public string ParameterBind { get; set; }
        public bool StartsGame { get; set; }
        public bool IsExit { get; set; }
        public IntRect ButtonRect { get; set; }
        public Button(Font uiFont, string caption, uint charSize, bool startsGame = false, bool isExit = false, string menuBind = "", string parameterBind = "")
        {
            ButtonCap = new Text(caption, uiFont, charSize);
            MenuBind = menuBind;
            StartsGame = startsGame;
            IsExit = isExit;
            ParameterBind = parameterBind;
            FloatRect temp = ButtonCap.GetGlobalBounds();
            ButtonRect = new((int)temp.Left, (int)temp.Top, (int)temp.Width, (int)temp.Height);
        }
        public void SetPos(float x, float y)
        {
            ButtonCap.Position = new(x, y);
            FloatRect temp = ButtonCap.GetGlobalBounds();
            ButtonRect = new((int)temp.Left, (int)temp.Top, (int)temp.Width, (int)temp.Height);
        }
    };
}
