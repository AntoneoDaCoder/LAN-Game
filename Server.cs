
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;

namespace To_The_Stars_hashtag_version
{
    internal class Server
    {
        private UdpClient Sender;
        private UdpClient Receiver;
        private static Model Model;
        private static Mutex Mut;
        private Thread SendThread;
        private static volatile bool CanRun = true;
        private static IPEndPoint ConnectedClient;
        public Server(Model model, Mutex mutex)
        {
            Sender = new(new IPEndPoint(IPAddress.Parse(Program.MachineIp), 2007));
            Receiver = new(new IPEndPoint(IPAddress.Parse(Program.MachineIp), 2008));
            Model = model;
            Mut = mutex;
            Receiver.Client.ReceiveTimeout = 30000;
        }

        public void StartRecSendRoutine()
        {
            SendThread = new(Send);
            SendThread.Name = "Send_Thread";
            SendThread.Start();
        }
        public bool AwaitConnection()
        {
            try
            {
                Receiver.Receive(ref ConnectedClient);
                Sender.Connect(new IPEndPoint(ConnectedClient.Address, ConnectedClient.Port + 1));
                Receiver.Client.ReceiveTimeout = 3000;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Kill()
        {
            CanRun = false;
            SendThread?.Join(1000);
            Sender.Close();
            Receiver.Close();
            Sender.Dispose();
            Receiver.Dispose();
        }
        public void Send()
        {
            while (CanRun)
            {
                Mut.WaitOne();
                ServerMessage packet = new ServerMessage(0, Model.Player, Model.Comrade, Model.Enemies, Model.Bullets, Model.Boosters);
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(packet));
                Sender.Send(data);
                try
                {
                    data = Receiver.Receive(ref ConnectedClient);
                }
                catch
                {
                    Model.Handler.SetMenuActive("cerror");
                    Mut.ReleaseMutex();
                    return;
                }
                string msg = Encoding.UTF8.GetString(data);
                ClientMessage newPacket = JsonSerializer.Deserialize<ClientMessage>(msg);
                Model.Comrade.SetEntityState(newPacket.CurrentState);
                Model.Comrade.IsShooting = newPacket.IsShooting;
                if (newPacket.ResetAnim)
                    Animator.ResetCurrentAnimation(Model.Comrade);
                Mut.ReleaseMutex();
            }
        }
    }
}
