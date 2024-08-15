namespace To_The_Stars_hashtag_version
{
    internal class Entity
    {
        static readonly Dictionary<string, int> StateManager = new()
        {
            {"idle",0},
            {"left",1},
            {"right",2},
            {"up",3},
            {"down",4},
            {"destroyed",5},
            {"damaged",6},
            {"evaded",7}
        };
        static readonly Random rng = new Random();

        public string State { get; set; }
        public double Timer { get; set; }
        public double CurrFrame { get; set; }
        public bool AnimHasEnded { get; set; }
        public bool AnimIsReversing { get; set; }
        public double Accel { get; set; }
        public string Tag { get; set; }
        public int UpgradeLvl { get; set; }
        public Rectangle EntityRect { get; set; }
        public Rectangle Borders { get; set; }
        public Animation CurrAnimation { get; set; }
        public Entity(Rectangle rect, Rectangle borders, string state, double accel, string tag)
        {
            State = state;
            Timer = 0;
            Accel = accel;
            Tag = tag;
            Borders = borders;
            EntityRect = rect;
            CurrFrame = 0;
            AnimHasEnded = true;
            AnimIsReversing = false;
            CurrAnimation = new("idle", 1, false, false);
        }
        public Entity(Entity other)
        {
            State  = other.State;
            Timer = other.Timer;
            CurrFrame = other.CurrFrame;
            AnimHasEnded = other.AnimHasEnded;
            AnimIsReversing = other.AnimIsReversing;
            CurrAnimation = other.CurrAnimation;
            Tag = other.Tag;
            Accel = other.Accel;
            UpgradeLvl = other.UpgradeLvl;
            EntityRect = other.EntityRect;
            Borders = other.Borders;
        }
        public Entity()
        {
        }
        static public int GetRandNum(int lower, int upper)
        {
            return rng.Next(lower, upper);
        }
        public bool CheckPosition(double newX, double newY)
        {
            return ((newX > Borders.Left) && (newX + EntityRect.Width < Borders.Left + Borders.Width) && (newY > Borders.Top) && (newY + EntityRect.Height < Borders.Top + Borders.Height));
        }
        public bool SetEntityState(string newState, bool overwrite = false)
        {
            if (overwrite || AnimHasEnded)
            {
                State = newState;
                Animator.SelectAnimation(this);
                return true;
            }
            return false;
        }
        public virtual void UpdatePosition()
        {
            double leftX = EntityRect.Left, leftY = EntityRect.Top;
            switch (StateManager[State])//select movement direction according to the state
            {
                case Storage.LEFT: leftX -= Accel; break;//dec x coord
                case Storage.UP: leftY -= Accel; break;//dec y coord
                case Storage.RIGHT: leftX += Accel; break;//inc x coord
                case Storage.DOWN: leftY += Accel; break;//inc y coord
            }
            if (CheckPosition(leftX, leftY))
            {
                EntityRect.Left = leftX;
                EntityRect.Top = leftY;
            }
        }

        public virtual void UpdateBehaviour(double elapsedTime)
        {
            if (AnimHasEnded)
                SetEntityState("idle");
            if (State != "destroyed")
            {
                Timer += elapsedTime;
                if (Timer >= 5)
                {
                    UpdatePosition();
                    Timer = 0;
                }
            }
            Animator.Update(this, elapsedTime);
        }
    }
}
