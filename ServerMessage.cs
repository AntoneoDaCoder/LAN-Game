using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_The_Stars_hashtag_version
{
    internal class ServerMessage
    {
        public Player ServerPlayer { get; set; }
        public Player ClientPlayer { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Bullet> Bullets { get; set; }
        public List<Booster> Boosters { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public int PacketId { get; set; }
        public ServerMessage(int id, Player serverPlayer, Player clientPlayer, List<Enemy> enemies, List<Bullet> bullets, List<Booster> boosters)
        {
            PacketId = id;
            ServerPlayer = new(serverPlayer);
            ClientPlayer = new(clientPlayer);
            Enemies = new List<Enemy>(enemies.Count);
            foreach (var enemy in enemies)
                Enemies.Add(new(enemy));
            Bullets = new List<Bullet>(bullets.Count);
            foreach (var b in bullets)
                Bullets.Add(new(b));
            Boosters = boosters;
            if (ServerPlayer.CurrAnimation != null)
                if (ServerPlayer.CurrFrame >= ServerPlayer.CurrAnimation.FrameQuantity)
                    ServerPlayer.CurrFrame = ServerPlayer.CurrAnimation.FrameQuantity - 1;
            if (ClientPlayer.CurrAnimation != null)
                if (ClientPlayer.CurrFrame >= ClientPlayer.CurrAnimation.FrameQuantity)
                    ClientPlayer.CurrFrame = ClientPlayer.CurrAnimation.FrameQuantity - 1;
            foreach (Enemy enemy in Enemies)
                if (enemy.CurrAnimation != null)
                    if (enemy.CurrFrame >= enemy.CurrAnimation.FrameQuantity)
                        enemy.CurrFrame = enemy.CurrAnimation.FrameQuantity - 1;
        }
        public ServerMessage()
        { }
    }
}
