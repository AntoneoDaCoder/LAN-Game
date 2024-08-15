namespace To_The_Stars_hashtag_version
{
    internal class Bullet : Entity
    {
        public double Delay { get; set; }
        public string Creator { get; set; }
        public Bullet(Rectangle rect, Rectangle borders, string state, double accel, string tag, string creator) : base(rect, borders, state, accel, tag)
        {
            Delay = 0;
            Timer = 0;
            SetEntityState(state);
            Creator = creator;
        }
        public Bullet()
        {
        }
        public Bullet(Bullet other) : base(other)
        {
            Timer = other.Timer;
            Delay = other.Delay;
            Creator = other.Creator;
            SetEntityState(other.State);
        }
        public bool CheckCollision(Rectangle entityRect)//this method checks if bullet has collided with something
        {
            if (Delay >= 100)
                if (EntityRect.Intersects(entityRect))//if given rectangle intersects bullet rect
                {
                    SetEntityState("destroyed", true);//bullet has collided with enemy or player
                    return true;
                }
            return false;
        }
        public void DestroyIfOutOfRange()
        {
            if (EntityRect.Top < Borders.Top || EntityRect.Top + EntityRect.Height > Borders.Height)//check bullet coordinates
                SetEntityState("destroyed", true);
        }
        public override void UpdatePosition()
        {
            if (State == "up")//if bullet is moving upward
                EntityRect.Top -= Accel;//decrease bullet y coord
            else
                EntityRect.Top += Accel;//otherwise increase bullet y coord
        }
        public override void UpdateBehaviour(double elapsedTime)//this method updates bullet game behaviour
        {
            Delay += elapsedTime;//increase delay value
            if (State != "destroyed")//if bullet is not destroyed
            {
                Timer += elapsedTime;//increase timer value
                if (Timer >= 5)// if at least 5 milliseconds have passed since the last call
                {
                    UpdatePosition();//update bullet position
                    Timer = 0;//reset timer
                }
                Animator.Update(this, elapsedTime);
                DestroyIfOutOfRange();//destroy bullet if it has left its movement borders
            }
        }
    }
}
