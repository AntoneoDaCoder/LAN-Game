
namespace To_The_Stars_hashtag_version
{
    internal class ClientMessage
    {
        public string CurrentState { get; set; }
        public bool IsShooting { get; set; }
        public bool ResetAnim { get; set; }
        public ClientMessage(string state, bool isShooting, bool resetAnim)
        {
            CurrentState = state;
            IsShooting = isShooting;
            ResetAnim = resetAnim;
        }
        public ClientMessage()
        {
        }
    }
}
