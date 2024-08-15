namespace To_The_Stars_hashtag_version
{
    internal class Animation
    {
        public string Name { get; set; }
        public int FrameQuantity { get; set; }
        public bool Loop { get; set; }
        public bool IsReversible { get; set; }
        public Animation(string name, int frameQuantity, bool loop, bool isReversible)
        {
            Name = name;
            FrameQuantity = frameQuantity;
            Loop = loop;
            IsReversible = isReversible;
        }
        public Animation()
        {
        }
    }
}
