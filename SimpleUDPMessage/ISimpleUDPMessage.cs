using System;

namespace RegiFraga.Comunication.UDP
{
    public interface ISimpleUDPMessage : IDisposable
    {
        int Port { get; }
        bool HasError { get; }

        void Start();
        void Stop();
        Exception GetLastError();
    }
}
