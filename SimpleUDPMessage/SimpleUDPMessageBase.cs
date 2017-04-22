using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RegiFraga.Comunication.UDP
{
    public abstract class SimpleUDPMessageBase : ISimpleUDPMessage
    {
        protected delegate void ReceiveHandle(IPEndPoint sender, string message);
        protected event ReceiveHandle OnReceive;

        private Task _task;
        private bool _cancel = false;

        public int Port { get; protected set; }
        public UdpClient Udp { get; protected set; }

        protected abstract void InitializerUdp();

        public virtual void Start()
        {
            InitializerUdp();

            var readInput = new Action(() => {
                while ((!_cancel))
                {
                    Receive();
                }
            });

            _task = new Task(readInput);

            _task.Start();
        }

        public virtual void Stop()
        {
            _cancel = true;
            _task.Wait();
        }

        public virtual void Dispose()
        {
            Stop();

            if (_task != null) _task.Dispose();
        }

        protected virtual void Receive()
        {
            var sender = new IPEndPoint(IPAddress.Any, Port);
            var data = new byte[1024];

            data = Udp.Receive(ref sender);
            var message = Encoding.ASCII.GetString(data, 0, data.Length);

            OnReceive?.Invoke(sender, message);
        }
    }
}
