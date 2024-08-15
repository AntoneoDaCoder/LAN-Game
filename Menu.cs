using SFML.Graphics;


namespace To_The_Stars_hashtag_version
{
    internal class Menu
    {
        public Text Title { get; set; }
        public List<Button> Ui { get; set; }
        public Menu(Font uiFont, string title, uint charSize)
        {
            Title = new Text(title, uiFont, charSize);
            Title.Style = Text.Styles.Bold;
            Ui = new List<Button>();
        }
        public void SetTitlePos(float x, float y)
        {
            Title.Position = new(x, y);
        }
        public FloatRect GetTitleRect()
        {
            return Title.GetGlobalBounds();
        }
        public Button AddButton(Font uiFont, string caption, uint charSize, bool startsGame = false, bool isExit = false, string menuBind = "", string parameterBind = "")
        {
            Ui.Add(new(uiFont, caption, charSize, startsGame, isExit, menuBind, parameterBind));
            return Ui.Last();
        }
    };
}
