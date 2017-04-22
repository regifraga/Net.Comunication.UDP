﻿using System;

namespace RegiFraga.Comunication.UDP.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Press S to start SERVER or C to CLIENT: ");
            var progType = Console.ReadLine();

            if (progType.ToUpper() == "S")
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
            else
            {
                using (var client = new SimpleUDPMessageClient())
                {
                    client.OnMessageReceive += Client_OnMessageReceive;
                    client.Start();
                    Console.WriteLine("CLIENT started at: {0}:{1}", client.Address, client.Port);

                    WaitUserMessage((msg) => {
                        client.Send(msg);
                    });

                    Console.WriteLine("Client - End1!");
                }

                Console.WriteLine("Client - End2!");
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
    }
}
