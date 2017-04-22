using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace RegiFraga.Comunication.UDP
{
    public class SimpleUDPMessageServer : SimpleUDPMessageBase
    {
        public delegate void MessageReceiveHandle(string senderAddress, string senderPort, string message);
        public event MessageReceiveHandle OnMessageReceive;

        public SimpleUDPMessageServer(int port = 9669)
        {
            Port = port;
            base.OnReceive += SimpleUDPMessageServer_OnMessageReceive;
        }

        protected override void InitializerUdp()
        {
            base.Udp = new UdpClient(Port);
        }

        private void SimpleUDPMessageServer_OnMessageReceive(IPEndPoint sender, string message)
        {
            OnMessageReceive?.Invoke(sender.Address.ToString(), sender.Port.ToString(), message);

            var response = Encoding.ASCII.GetBytes(DateTime.Now.ToString());
            base.Udp.Send(response, response.Length, sender);
        }
    }
}
