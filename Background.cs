namespace To_The_Stars_hashtag_version
{
    internal class Background
    {
        public int CurrSprite { get; private set; }
        public int NextSprite { get; private set; }
        public double Timer { get; private set; }
        public double Speed { get; set; }
        public double LowerBound { get; private set; }
        public Pair<double, double> CurrSpritePos { get; private set; }
        public Pair<double, double> NextSpritePos { get; private set; }
        public Background(double speed, double Ysize)
        {
            Timer = 0;//initialize timer
            Speed = speed;//initialize sprite movement speed
            LowerBound = Ysize;//set lower bound
            CurrSpritePos = new(0, 0);//set the current sprite position
            NextSpritePos = new(0, -LowerBound);//set the next sprite position
        }
        public void UpdatePosition(double elapsedTime)//this method updates background sprites position
        {
            Timer += elapsedTime;//increase timer
            ResetSprites();//reset sprite (only will happen if sprite has left its lower border)
            if (Timer >= 5)//if at least 5 milliseconds have passed since the last call
            {
                CurrSpritePos = new(CurrSpritePos.First, CurrSpritePos.Second + Speed);//update first sprite pos
                NextSpritePos = new(NextSpritePos.First, NextSpritePos.Second + Speed);//update second sprite pos
                Timer = 0;//reset timer
            }
        }
        public void ResetSprites()//this method resets sprite position
        {
            if (CurrSpritePos.Second >= LowerBound)//if sprite is below the lower bound
            {
                CurrSprite = ChangeSprite(CurrSprite);//change picture
                CurrSpritePos = new(0, -LowerBound);//reset sprite position
            }
            if (NextSpritePos.Second >= LowerBound)
            {
                NextSprite = ChangeSprite(NextSprite);//change picture
                NextSpritePos = new(0, -LowerBound);//reset sprite position
            }
        }
        public static int ChangeSprite(int sprite)//this method changes background sprite
        {
            sprite = Entity.GetRandNum(0, 17);
            return sprite;
        }
    }
}
