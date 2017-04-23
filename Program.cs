using System;

namespace UDP.Simple.Message.Tests
{
    class Example
    {
        static void Main(string[] args)
        {
            Console.Write("Press S to start SERVER, C to CLIENT CC to CLIENT with CALLBACK opction: ");
            var progType = Console.ReadLine();

            if (progType.ToUpper() == "S")
            {
                using (var server = new SimpleUDPMessageServer())
                {
                    server.OnMessageReceive += Server_OnMessageReceive;
                    server.OnResponseMessage += Server_OnResponseMessage;
                    server.Start();

                    Console.WriteLine("SERVER started at port: {0}", server.Port);

                    WaitUserMessage((msg) => {
                        Console.WriteLine("> {0}", msg);
                    });

                    Console.WriteLine("Server - End!");
                }
            }
            else if (progType.ToUpper() == "C")
            {
                using (var client = new SimpleUDPMessageClient())
                {
                    client.OnMessageReceive += Client_OnMessageReceive;
                    client.Start();
                    Console.WriteLine("CLIENT started at: {0}:{1}", client.Address, client.Port);

                    WaitUserMessage((msg) => {
                        client.Send(msg);
                    });
                }

                Console.WriteLine("Client - End!");
            }
            else
            {
                using (var client = new SimpleUDPMessageClient())
                {
                    client.Start();
                    Console.WriteLine("CLIENT(with callbak) started at: {0}:{1}", client.Address, client.Port);

                    WaitUserMessage((msg) => {
                        client.Send(msg, (response) => {
                            Console.WriteLine("[callbak]> {0}", response);
                        });
                    });
                }

                Console.WriteLine("Client(with callbak) - End!");
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

        private static void Server_OnMessageReceive(string senderAddress, string senderPort, string message)
        {
            Console.WriteLine("[{0}:{1}] >> {2}", senderAddress, senderPort, message);
        }

        private static void Server_OnResponseMessage(string receivedMessage, out string responseToSend)
        {
            switch (receivedMessage.ToUpper())
            {
                case "DATE":
                    responseToSend = DateTime.Now.ToString();
                    break;
                case "TEST":
                    responseToSend = "Ok, it's just a test!";
                    break;
                case "LIST":
                    var files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
                    responseToSend = string.Join(Environment.NewLine, files);
                    break;
                case "END":
                    responseToSend = "Tchau... :)";
                    break;
                default:
                    responseToSend = "";
                    break;
            };
        }
    }
}
