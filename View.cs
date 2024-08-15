using SFML.Graphics;
namespace To_The_Stars_hashtag_version
{
    internal class View
    {
        private List<Sprite> PlayerSprites = new List<Sprite>();
        private List<Sprite> EnemySprites = new List<Sprite>();
        private List<Sprite> BulletSprites = new List<Sprite>();
        private List<Sprite> BoosterSprites = new List<Sprite>();
        private List<Sprite> BackgroundSprites = new List<Sprite>();

        private Sprite HpIco = new Sprite(Storage.GetIco());
        private Text HpText = new("5", Program.uiFont, 80);
        private Text ScoreText = new("0", Program.uiFont, 80);
        public View()
        {
            HpText.Position = new(15, -15);
            ScoreText.Position = new(500, -15);
            HpIco.Position = new(60, 15);
        }
        private static Sprite AddSprite(Entity entity)
        {
            Sprite newSprite = new Sprite();
            newSprite.Texture = Storage.GetTexture((int)entity.CurrFrame, entity.Tag, entity.CurrAnimation.Name);
            newSprite.Position = new((float)(entity.EntityRect.Left - ((newSprite.TextureRect.Width - entity.EntityRect.Width) / 2)), (float)(entity.EntityRect.Top - ((newSprite.TextureRect.Height - entity.EntityRect.Height) / 2)));
            return newSprite;
        }
        private void UpdateBackground(Background bg)
        {
            BackgroundSprites.Clear();
            Sprite currBgSprite = new();
            currBgSprite.Texture = Storage.GetBackgroundImage(bg.CurrSprite);
            currBgSprite.Position = new((float)bg.CurrSpritePos.First, (float)bg.CurrSpritePos.Second);
            BackgroundSprites.Add(new(currBgSprite));

            Sprite nextBgSprite = new();
            nextBgSprite.Texture = Storage.GetBackgroundImage(bg.NextSprite);
            nextBgSprite.Position = new((float)bg.NextSpritePos.First, (float)bg.NextSpritePos.Second);
            BackgroundSprites.Add(new(nextBgSprite));
        }
        private void UpdateSprites(Background bg, List<Player> players, List<Enemy> enemies, List<Bullet> bullets, List<Booster> boosters)
        {
            PlayerSprites.Clear();
            EnemySprites.Clear();
            BulletSprites.Clear();
            BoosterSprites.Clear();
            foreach (Player p in players)
            {
                if (p != null)
                    PlayerSprites.Add(new(AddSprite(p)));
            }
            foreach (Enemy e in enemies)
            {
                EnemySprites.Add(new(AddSprite(e)));
            }
            foreach (Bullet b in bullets)
            {
                BulletSprites.Add(new(AddSprite(b)));
            }
            foreach (Booster b in boosters)
            {
                BoosterSprites.Add(new(AddSprite(b)));
            }
            UpdateBackground(bg);
        }
        private void UpdateSprites(Background bg)
        {
            UpdateBackground(bg);
        }
        public void UpdateGameView(Model model, RenderWindow window)
        {
            UpdateSprites(Model.Background, new List<Player>() { model.Player, model.Comrade }, model.Enemies, model.Bullets, model.Boosters);
            window.Clear();
            foreach (Sprite bs in BackgroundSprites)
                window.Draw(bs);
            window.Draw(HpIco);
            window.Draw(HpText);
            window.Draw(ScoreText);
            foreach (Sprite bulletSprite in BulletSprites)
                window.Draw(bulletSprite);
            foreach (Sprite boosterSprite in BoosterSprites)
                window.Draw(boosterSprite);
            foreach (Sprite playerSprite in PlayerSprites)
                window.Draw(playerSprite);
            foreach (Sprite enemySprite in EnemySprites)
                window.Draw(enemySprite);
            window.Display();
        }
        public void UpdateText(Model model)
        {
            HpText.DisplayedString = (model.Player.Hp / 2).ToString();//update hpText string
            if (model.Player.Hp / 2 < 3)
                HpText.FillColor = Color.Red;//change text color
            else
                HpText.FillColor = Color.White;//change text color
            ScoreText.DisplayedString = model.Player.Score.ToString();//update scoreText string
        }
        public void UpdateMenuView(RenderWindow window)
        {
            UpdateSprites(Model.Background);
            window.Clear();
            foreach (Sprite bs in BackgroundSprites)
                window.Draw(bs);
            Model.Handler.Update(window);
            window.Display();
        }
    }
}
