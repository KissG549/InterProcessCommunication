using System;

namespace InterProcessCommunication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client = new ClientImpl();

            try
            {
                client.connect("127.0.0.1:3000");
                // connectionList.append( client.getConnectionId() );
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to connect to the server, ", e.ToString());
            }

            if ( client.IsConnected )
            {
                /*
                    dataProcessCallback purpose:
                        - process incoming data
                        - call Log function to print out the incoming information
                 */
                // client.asyncWaitForMsg(dataProcessCallback);
                client.asyncWaitForInput();
            }
        }
    }
}
