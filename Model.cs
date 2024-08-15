namespace To_The_Stars_hashtag_version
{
    internal class Model
    {
        public Player Player { get; set; }
        public Player? Comrade { get; set; }
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();
        public List<Bullet> Bullets { get; set; } = new List<Bullet>();
        public List<Booster> Boosters { get; set; } = new List<Booster>();
        public int EnemyNum { get; set; }
        public int CurrEnemyNum { get; set; }
        public int EnemyUpgradeLvl { get; set; }
        static public Background Background { get; set; }
        static public MenuHandler Handler { get; set; } = new();
        public Model()
        {
        }

    }
}
