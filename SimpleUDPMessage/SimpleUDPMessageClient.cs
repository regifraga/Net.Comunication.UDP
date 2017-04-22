using System.Net;
using System.Text;
using System.Net.Sockets;

namespace RegiFraga.Comunication.UDP
{
    public class SimpleUDPMessageClient : SimpleUDPMessageBase
    {
        public delegate void MessageReceiveHandle(string message);
        public event MessageReceiveHandle OnMessageReceive;

        public SimpleUDPMessageClient(string address = "127.0.0.1", int port = 9669)
        {
            Address = address;
            Port = port;

            base.OnReceive += SimpleUDPMessageClient_OnReceive; ;
        }

        protected override void InitializerUdp()
        {
            base.Udp = new UdpClient(Address, Port);
        }

        public string Address { get; private set; }

        public void Send(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            base.Udp.Send(data, data.Length);
        }

        private void SimpleUDPMessageClient_OnReceive(IPEndPoint sender, string message)
        {
            OnMessageReceive?.Invoke(message);
        }
    }
}
