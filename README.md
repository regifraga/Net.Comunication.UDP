# Net.Comunication.UDP
A very simple Client/Server component comunication using "System.Net.Sockets.UdpClient".

## How to use client component

```CSharp
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new SimpleUDPMessageClient())
            {
                client.OnMessageReceive += Client_OnMessageReceive;
                client.Start();
                Console.WriteLine("CLIENT started at: {0}:{1}", client.Address, client.Port);

                WaitUserMessage((msg) => {
                    client.Send(msg);
                });

                Console.WriteLine("Client - End!");
            }
        }

        private static void WaitUserMessage(Action<string> command)
        {
            while (true)
            {
                var message = Console.ReadLine();
                if (message.ToUpper() == "")
                {
                    break;
                }

                command?.Invoke(message);
                System.Threading.Thread.Sleep(250);
            }
        }

        private static void Client_OnMessageReceive(string message)
        {
            Console.WriteLine("> {0}", message);
        }
    }
}
```

## How to use client component with callback option

```CSharp
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new SimpleUDPMessageClient())
            {
                client.Start();
                Console.WriteLine("CLIENT(with callback) started at: {0}:{1}", client.Address, client.Port);

                WaitUserMessage((msg) => {
                    client.Send(msg, (response) => {
                        Console.WriteLine("[with callback]> {0}", response);
                    });
                });

                Console.WriteLine("Client(with callback) - End!");
            }
        }

        private static void WaitUserMessage(Action<string> command)
        {
            while (true)
            {
                var message = Console.ReadLine();
                if (message.ToUpper() == "")
                {
                    break;
                }

                command?.Invoke(message);
                System.Threading.Thread.Sleep(250);
            }
        }
    }
}
```

## How to use server component

```CSharp
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new SimpleUDPMessageServer())
            {
                server.OnMessageReceive += Server_OnMessageReceive;
                server.Start();

                Console.WriteLine("SERVER started at port: {0}", server.Port);

                WaitUserMessage((msg) => {
                    Console.WriteLine("> {0}", msg);
                });

                Console.WriteLine("Server - End!");
            }
        }

        private static void WaitUserMessage(Action<string> command)
        {
            while (true)
            {
                var message = Console.ReadLine();
                if (message.ToUpper() == "")
                {
                    break;
                }

                command?.Invoke(message);
                System.Threading.Thread.Sleep(250);
            }
        }

        private static void Server_OnMessageReceive(string senderAddress, string senderPort, string message)
        {
            Console.WriteLine("[{0}:{1}] >> {2}", senderAddress, senderPort, message);
        }
    }
}
```
