using System;

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
                var client = new ClientImpl();

                try
                {
                    client.Connect(args[1]);
                    // connectionList.append( client.getConnectionId() );
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to connect to the server: {0}", e.ToString());
                }

                if (client.IsConnected)
                {
                    // client.asyncWaitForMsg(dataProcessCallback);
                    client.AsyncWaitForInput();
                }
            }
            else if( args[0] == "-s" )
            {
                ServerImpl server = new ServerImpl();
                if (args.Length < 3)
                {
                    server.Listening("", Int32.Parse(args[1]));
                }
                else
                {
                    server.Listening(args[1], Int32.Parse(args[2]));
                }
            }
            else
            {
                printUsage();
            }

        }
    }
}
