namespace To_The_Stars_hashtag_version
{
    internal class Player : Entity
    {
        public int Hp { get; set; }
        public int Damage { get; set; }
        public int Score { get; set; }
        public double ShootTimer { get; set; }
        public bool IsShooting { get; set; } = false;
        public List<List<Pair<double, double>>> LaunchCoords { get; set; }
        public Player(Rectangle rect, Rectangle borders, string state, double accel, string tag, int hp, int damage) : base(rect, borders, state, accel, tag)
        {
            Hp = hp;
            Damage = damage;
            Score = 0;
            ShootTimer = 0;
            LaunchCoords = new();
            List<Pair<double, double>> temp = new()
            {
                new(EntityRect.Left + EntityRect.Width / 2 - 3, EntityRect.Top + EntityRect.Height / 2 - 45)
            };
            LaunchCoords.Add(new(temp));
            temp.Clear();
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 - 10, EntityRect.Top - 3));
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 + 5, EntityRect.Top - 3));
            LaunchCoords.Add(new(temp));
            temp.Clear();
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 - 3, EntityRect.Top + EntityRect.Height / 2 - 45));
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 - 10, EntityRect.Top - 3));
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 + 5, EntityRect.Top - 3));
            LaunchCoords.Add(new(temp));
            temp.Clear();
        }
        public Player(Player player) : base(player)
        {
            Hp = player.Hp;
            Damage = player.Damage;
            Score = player.Score;
            ShootTimer = player.ShootTimer;
            IsShooting = player.IsShooting;
            LaunchCoords = player.LaunchCoords;
        }
        public Player()
        {
            LaunchCoords = new();
        }
        void UpdateLaunchPoints()
        {
            LaunchCoords[0][0].First = EntityRect.Left + EntityRect.Width / 2 - 3;
            LaunchCoords[0][0].Second = EntityRect.Top + EntityRect.Height / 2 - 29;

            LaunchCoords[1][0].First = EntityRect.Left + EntityRect.Width / 2 - 10;
            LaunchCoords[1][0].Second = EntityRect.Top - 29;
            LaunchCoords[1][1].First = EntityRect.Left + EntityRect.Width / 2 + 5;
            LaunchCoords[1][1].Second = EntityRect.Top - 29;

            LaunchCoords[2][0].First = EntityRect.Left + EntityRect.Width / 2 - 3;
            LaunchCoords[2][0].Second = EntityRect.Top + EntityRect.Height / 2 - 29;
            LaunchCoords[2][1].First = EntityRect.Left + EntityRect.Width / 2 - 10;
            LaunchCoords[2][1].Second = EntityRect.Top - 29;
            LaunchCoords[2][2].First = EntityRect.Left + EntityRect.Width / 2 + 5;
            LaunchCoords[2][2].Second = EntityRect.Top - 29;
        }
        public override void UpdateBehaviour(double elapsedTime)
        {
            ShootTimer += elapsedTime;
            if (Hp == 0 && State != "destroyed")
            {
                Storage.PlayExplosion();
                SetEntityState("destroyed", true);
            }
            base.UpdateBehaviour(elapsedTime);
        }

        public void Shoot(List<Bullet> bullets, string who)
        {
            if (Hp > 10 ? ShootTimer >= 1000 : ShootTimer >= 1000 * (1 - 0.1 * (10 - Hp)) && State != "destroyed")
            {
                UpdateLaunchPoints();//update coordinates of the launch points
                Storage.PlayShot();//play sound
                foreach (var e in LaunchCoords[UpgradeLvl])//select the right one vector
                {
                    Rectangle borders = new(e.First, 0, 5, e.Second + 29);//create bullet movement borders
                    bullets.Add(new(new(e.First, e.Second, 3, 28), borders, "up", 6, "bullet", who));//place new bullet in the list
                }
                ShootTimer = 0;//reset reload timer
            }
        }
    }
}
