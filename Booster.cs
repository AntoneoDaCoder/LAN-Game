namespace To_The_Stars_hashtag_version
{
    internal class Booster : Entity
    {
        public Booster(Rectangle rect, Rectangle borders, string state, double accel, string tag) : base(rect, borders, state, accel, tag)
        {
        }
        public Booster()
        {
        }
        public Booster(Booster other) : base(other)
        {
        }
        public bool CheckCollision(Rectangle entityRect)//this method checks if booster has collided with something
        {
            if (EntityRect.Intersects(entityRect))//if given rectangle intersects booster rect
            {
                SetEntityState("destroyed", true);//booster has collided with player
                return true;
            }
            return false;
        }
        public void DestroyIfOutOfRange()
        {
            if (EntityRect.Top < Borders.Top || EntityRect.Top + EntityRect.Height > Borders.Height)//check bullet coordinates
                SetEntityState("destroyed", true);
        }
        public override void UpdatePosition()//this method updates booster position
        {
            EntityRect.Top += Accel;//decrease booster y coord
        }
        public override void UpdateBehaviour(double elapsedTime)//this method updates booster game behaviour
        {
            if (State != "destroyed")//if booster is not destroyed
            {
                Timer += elapsedTime;//increase timer value
                if (Timer >= 5)// if at least 5 milliseconds have passed since the last call
                {
                    UpdatePosition();//update booster position
                    Timer = 0;//reset timer
                }
                Animator.Update(this, elapsedTime);
                DestroyIfOutOfRange();//destroy booster if it has left its movement borders
            }
        }
    }
}
