using InterProcessCommunication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/*
    Based on: 
        https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
 */

namespace InterProcessCommunication
{
    class SocketStateObject
    {
        /* buffer size */
        public const int mBufferSize = 1024;

        /* buffer */
        public byte[] mBuffer = new byte[mBufferSize];

        /* data string */
        public StringBuilder mSb = new StringBuilder();

        /* client socket */
        public Socket mClientSocket = null;
    }

    class ServerImpl : IServer
    {
        /*
            mre is used to block and release threads manually
         */
        public static ManualResetEvent mMre = new ManualResetEvent(false);

        public List<Socket> mConnectedClients = new List<Socket>();

        ~ServerImpl()
        {
            foreach( Socket client in mConnectedClients )
            {
                if( client.Connected )
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
        }
        public void Listening(string pIPAddress, int pPortNumber)
        {
            /*
                Set default binding address
             */
            if( pIPAddress.Length < 7 )
            {
                pIPAddress = "127.0.0.1";
            }
            /*
                Attach the program to a local socket
             */
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse(pIPAddress);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, pPortNumber);

            Socket listener = 
                new Socket(
                    ipAddress.AddressFamily, 
                    SocketType.Stream, 
                    ProtocolType.Tcp);

            /* Bind the socket */
            try {

                listener.Bind(localEndPoint);
                listener.Listen(100);
                
                while(true)
                {
                    mMre.Reset();

                    Console.WriteLine("Listening on {0} : {1}", ipAddress.ToString(), pPortNumber);
                    Console.WriteLine("Waiting for connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback), 
                        listener);

                    // Wait until a connection is made before continuing.  
                    mMre.WaitOne();
                }
            } 
            catch(Exception e)
            {
                Console.WriteLine(
                    "Can't bind the socket: {0}",
                    e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public void AcceptCallback(IAsyncResult pAr)
        {
            // Signal the main thread to continue.  
            mMre.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)pAr.AsyncState;
            Socket clientSocket = listener.EndAccept(pAr);
            mConnectedClients.Add(clientSocket);

            SocketStateObject state = new SocketStateObject();
            state.mClientSocket = clientSocket;
            clientSocket.BeginReceive(
                state.mBuffer, 
                0, 
                SocketStateObject.mBufferSize, 
                0, 
                new AsyncCallback(ReadCallback),
                state);
        }
        public void ReadCallback(IAsyncResult pAr)
        {
            string content = string.Empty;

            SocketStateObject state = (SocketStateObject)pAr.AsyncState;
            Socket handler = state.mClientSocket;

            // Read data from the socket
            int bytesRead = 0;
            try
            {
                bytesRead = handler.EndReceive(pAr);
            }
            catch(Exception e)
            {
                Console.WriteLine(
                    "Can't read data from remote end: {0} ", 
                    e.ToString()
                    );
                bytesRead = 0;
            }

            if( bytesRead > 0 )
            {
                state.mSb.Append(Encoding.ASCII.GetString(state.mBuffer, 0, bytesRead));
                content = state.mSb.ToString();

                if(content.IndexOf("<EOF>") > -1)
                {
                    Console.WriteLine(
                        "Arrived {0} bytes from socket.\n Data: {1}", 
                        content.Length, 
                        content);

                    Send(handler, content);
                }
                else
                {
                    handler.BeginReceive(
                        state.mBuffer, 
                        0, 
                        SocketStateObject.mBufferSize, 
                        0, 
                        new AsyncCallback(ReadCallback), 
                        state);
                }
            }
        }

        public void Send(Socket pHandler, string pData)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(pData);

            pHandler.BeginSend(
                byteData,
                0,
                byteData.Length,
                0,
                new AsyncCallback(SendCallBack),
                pHandler);
        }

        public void SendCallBack(IAsyncResult pAr)
        {
            try
            {
                Socket handler = (Socket)pAr.AsyncState;

                int byteSent = handler.EndSend(pAr);
                Console.WriteLine("Sent {0} bytes to client.", byteSent);

            }
            catch( Exception e)
            {
                Console.WriteLine("Can't send data to the client: {1}", e.ToString());
            }
        }
    }
}
