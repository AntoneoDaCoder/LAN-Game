namespace To_The_Stars_hashtag_version
{
    internal class Enemy : Entity
    {
        static List<string> States = new() { "idle", "left", "right", "up", "down" };
        public int Hp { get; set; }
        public int Damage { get; set; }
        public List<List<Pair<double, double>>> LaunchCoords { get; set; }
        public double DirectionTimer { get; set; }
        public double ShootTimer { get; set; }
        public Enemy(Rectangle rect, Rectangle borders, string state, double accel, string tag, int hp, int damage, int upgradeLvl) : base(rect, borders, state, accel, tag)
        {
            Hp = hp;
            Damage = damage;
            UpgradeLvl = upgradeLvl;
            ShootTimer = 0;
            DirectionTimer = GetRandNum(0, 500);
            LaunchCoords = new();
            List<Pair<double, double>> temp = new()
            {
                new(EntityRect.Left + EntityRect.Width / 2 - 3, EntityRect.Top + EntityRect.Height +1)
            };
            LaunchCoords.Add(new(temp));
            temp.Clear();
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 - 15, EntityRect.Top + EntityRect.Height + 1));
            temp.Add(new(EntityRect.Left + EntityRect.Width / 2 + 12, EntityRect.Top + EntityRect.Height + 1));
            LaunchCoords.Add(new(temp));
            temp.Clear();
        }
        public Enemy()
        {
            LaunchCoords = new();
        }
        public Enemy(Enemy enemy) : base(enemy)
        {
            Hp = enemy.Hp;
            Damage = enemy.Damage;
            DirectionTimer = enemy.DirectionTimer;
            ShootTimer = enemy.ShootTimer;
            LaunchCoords = enemy.LaunchCoords;
        }
        public void SetStates()
        {
            States.Clear();
            if ((EntityRect.Left > Borders.Left + 5) && (EntityRect.Left + EntityRect.Width) < (Borders.Left + Borders.Width - 5)) //if enemy is somewhere in the horizontal center
            {
                States.Add("left");
                States.Add("right");
            }
            else
                if (EntityRect.Left <= Borders.Left + 5) //if enemy is near the left border
                States.Add("right");
            else
                if (EntityRect.Left + EntityRect.Width >= Borders.Left + Borders.Width - 5)//if enemy is near the right border
                States.Add("left");
            if ((EntityRect.Top > Borders.Top + 5) && EntityRect.Top + EntityRect.Height < (Borders.Top + Borders.Height - 5)) //if enemy is somewhere in the vertical center
            {
                States.Add("up");
                States.Add("down");
            }
            else
                if (EntityRect.Top <= Borders.Top + 5) //if enemy is near the top border
                States.Add("down");
            else
                if (EntityRect.Top + EntityRect.Height >= Borders.Top + Borders.Height - 5) //if enemy is near the bottom border
                States.Add("up");
        }
        string ChangeDirection()
        {
            SetStates();
            int min = 0, max = States.Count, pos;
            pos = GetRandNum(min, max);
            return States[pos];
        }
        void UpdateLaunchPoints()//this method updates bullet launch coordinates
        {
            LaunchCoords[0][0].First = EntityRect.Left + EntityRect.Width / 2 - 3;
            LaunchCoords[0][0].Second = EntityRect.Top + EntityRect.Height + 1;

            LaunchCoords[1][0].First = EntityRect.Left + EntityRect.Width / 2 - 15;
            LaunchCoords[1][0].Second = EntityRect.Top + EntityRect.Height + 1;
            LaunchCoords[1][1].First = EntityRect.Left + EntityRect.Width / 2 + 12;
            LaunchCoords[1][1].Second = EntityRect.Top + EntityRect.Height + 1;
        }
        public override void UpdateBehaviour(double elapsedTime)
        {
            ShootTimer += elapsedTime;
            DirectionTimer += elapsedTime;
            if (Hp == 0 && State != "destroyed") //check state
            {
                Storage.PlayExplosion(); //playe sound
                SetEntityState("destroyed", true);
            }
            else
            {
                string newState = ChangeDirection();//select the new state
                if (DirectionTimer >= 2000)
                {
                    if (SetEntityState(newState))//if state was succesfully changed
                    {
                        DirectionTimer = 0;
                    }
                    else
                        if (!AnimIsReversing)//otherwise check if anim reset is needed
                        Animator.ResetCurrentAnimation(this);
                }
            }
            base.UpdateBehaviour(elapsedTime);
        }
        public void Shoot(List<Bullet> bullets, string who)
        {
            UpdateLaunchPoints();
            Storage.PlayShot();
            foreach (var e in LaunchCoords[UpgradeLvl])
            {
                Rectangle borders = new(e.First - 1, e.Second, 8, 1200);
                bullets.Add(new(new(e.First, e.Second, 6, 20), borders, "down", (float)(2 + UpgradeLvl * 1.2), "bullet", who));
            }
        }
    }
}
