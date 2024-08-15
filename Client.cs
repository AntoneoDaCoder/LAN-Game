using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;


namespace To_The_Stars_hashtag_version
{
    internal class Client
    {
        private UdpClient Sender;
        private UdpClient Receiver;
        private static Model Model;
        private static Mutex Mut;
        private Thread ReceiveThread;
        public static string CurrPlayerState;
        public static bool IsShooting;
        public static bool ResetAnim;
        private static volatile bool CanRun = true;
        private static IPEndPoint ConnectedServer;
        public Client(Model model, Mutex mutex, string serverIp)
        {
            Sender = new(new IPEndPoint(IPAddress.Parse(Program.MachineIp), 2005));
            Receiver = new(new IPEndPoint(IPAddress.Parse(Program.MachineIp), 2006));
            ConnectedServer = new IPEndPoint(IPAddress.Parse(serverIp), 2008);
            Model = model;
            Mut = mutex;
            Receiver.Client.ReceiveTimeout = 3000;
            CurrPlayerState = "idle";
            Connect();
        }
        private void Connect()
        {
            byte[] hello = Encoding.UTF8.GetBytes("hello");
            Sender.Connect(ConnectedServer);
            Sender.Send(hello);
        }
        public void StartRecSendRoutine()
        {
            ReceiveThread = new(Receive);
            ReceiveThread.Name = "Receive_Thread";
            ReceiveThread.Start();
        }

        public void Kill()
        {
            CanRun = false;
            ReceiveThread?.Join(1000);
            Sender.Close();
            Sender.Dispose();
            Receiver.Close();
            Receiver.Dispose();
        }
        public void Receive()
        {
            while (CanRun)
            {
                Mut.WaitOne();
                byte[] readByte = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new ClientMessage(CurrPlayerState, IsShooting, ResetAnim)));
                Sender.Send(readByte);
                if (ResetAnim == true)
                {
                    ResetAnim = false;
                    CurrPlayerState = "idle";
                }
                try
                {
                    IPEndPoint rep = new IPEndPoint(IPAddress.Any, ConnectedServer.Port - 1);
                    readByte = Receiver.Receive(ref rep);
                }
                catch
                {
                    Model.Handler.SetMenuActive("cerror");
                    Mut.ReleaseMutex();
                    return;
                }
                string msg = Encoding.UTF8.GetString(readByte);
                ServerMessage packet = JsonSerializer.Deserialize<ServerMessage>(msg);
                Model.Player = new(packet.ClientPlayer);
                Model.Comrade = new(packet.ServerPlayer);
                Model.Enemies.Clear();
                Model.Enemies = new(Model.Enemies.Count);
                foreach (var item in packet.Enemies)
                {
                    Model.Enemies.Add(new(item));
                }
                Model.Bullets.Clear();
                Model.Bullets = new(Model.Bullets.Count);
                foreach (var item in packet.Bullets)
                {
                    Model.Bullets.Add(new(item));
                }
                Model.Boosters.Clear();
                Model.Boosters = new(Model.Boosters.Count);
                foreach (var item in packet.Boosters)
                {
                    Model.Boosters.Add(new(item));
                }
                Mut.ReleaseMutex();
            }
        }
    }
}
