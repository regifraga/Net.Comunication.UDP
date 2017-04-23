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
        private Exception _lastError;

        private Task _task;
        private bool _cancel = false;

        public int Port { get; protected set; }
        public bool HasError { get { return (_lastError != null); } }
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

            _task = new Task(readInput, new System.Threading.CancellationToken());

            _task.Start();
        }

        public virtual void Stop()
        {
            _cancel = true;
            Udp.Client.Close();

            _task.Wait();
        }

        public Exception GetLastError()
        {
            return _lastError;
        }

        public virtual void Dispose()
        {
            Stop();

            if (_task != null) _task.Dispose();

            if (Udp.Client != null) Udp.Client.Dispose();
        }

        protected virtual void Receive()
        {
            try
            {
                var sender = new IPEndPoint(IPAddress.Any, Port);
                var data = new byte[1024];

                var receive = Udp.BeginReceive(null, null);
                data = Udp.EndReceive(receive, ref sender);

                var message = Encoding.ASCII.GetString(data, 0, data.Length);

                OnReceive?.Invoke(sender, message);
            }
            catch (SocketException ex) //Timeout, WSACancelBlockingCall or some other error happened.
            {
                _lastError = ex;
            }
            catch (Exception ex)
            {
                _lastError = ex;
            }
        }
    }
}
