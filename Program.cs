﻿using System;
using System.Threading;

namespace InterProcessCommunication
{
    class Program
    {
        static void printUsage()
        {
            Console.WriteLine(
                "This program purpose is to demonstrate data exchange between two processes using bidirectional communication.\n" +
                "Implemented data exchange method: Socket-JSON over TCP\n\n" +
                "Usage:\n" +
                "\t-c server_host:port_number\t\tRun in client mode, connect to the specific address:port" +
                "\t\t -c 10.0.0.1:3001\n" +
                "\t-s listening_address port_number\t\tRun in server mode, listening on specific IP and port" +
                "\t\t -s 127.0.0.1 3001" +
                "\t\t -s 3001" +
                "\t?\t\tDisplay this message.");
        }

        static void Main(string[] args)
        {

            if( args.Length < 2 )
            {
                Console.WriteLine("You missed the program parameters!");
                printUsage();
            }
            else if( args[0] == "-c" )
            {
                ClientImpl client = new ClientImpl();

                Console.WriteLine("<Running in client mode>");
                try
                {
                    int colonIndex = args[1].IndexOf(':');
                    if(colonIndex == -1)
                    {
                        throw new ArgumentException("Invalid host:port format");
                    }
                    string host = args[1].Substring(0, colonIndex);
                    string port = args[1].Substring(colonIndex + 1);
                    while(! client.Connect(host, Int32.Parse(port)) )
                    {
                        Console.WriteLine("Trying to connect...");
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to connect to the server: {0}", e.ToString());
                }

                if (client.IsConnected)
                {
                    Thread clientReceiveTH = new Thread(client.Receive);
                    clientReceiveTH.Start();

                    client.sendSampleData();

                    while (true)
                    {
                        // read input from the console
                        Console.WriteLine("Write message here (press enter to send): ");
                        string inputText = Console.ReadLine();
                        
                        if (inputText.Length >= 2 && (inputText[0] == ':' && inputText[1] == 'x'))
                        {
                            break;
                        }
                        else
                        {
                            // send it to the server
                            client.Send(inputText);
                        }
                        // send it to the server
                    }
                }
            }
            else if( args[0] == "-s" )
            {
                ServerImpl server = new ServerImpl();
                
                Console.WriteLine("<Running in server mode>");

                if (args.Length < 3)
                {
                   server.Listening("", int.Parse(args[1]));
                }
                else
                {
                    server.Listening(args[1], Int32.Parse(args[2]));
                }

                Thread serverReceiveTH = new Thread(server.Receive);
                serverReceiveTH.Start();

                // Read input from console
                while (true)
                {
                    Console.WriteLine("Write message here (press enter to send or :x Exit): ");
                    string inputText = Console.ReadLine();
                    if (inputText.Length >= 2 && (inputText[0] == ':' && inputText[1] == 'x'))
                    {
                        break;
                    }
                    else
                    {
                        server.Send(inputText);
                    }
                }

                server.sendSampleData();
            }
            else
            {
                printUsage();
            }

        }
    }
}
