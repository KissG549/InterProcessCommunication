using InterProcessCommunication.InterProcessCommunication;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace InterProcessCommunication
{
    internal class ClientImpl : IClient
    {
        private IPEndPoint mRemoteEP;
        private Socket mSocket;

        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private static string response = string.Empty;

        public ClientImpl()
        {
        }

        ~ClientImpl()
        {
            if (mSocket.Connected) { Disconnect(); }
        }

        public bool IsConnected
        {
            get { return mSocket.Connected;  }
        }

        public bool Connect(string pServerAddr, int pPort)
        {
            try 
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(pServerAddr);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                mRemoteEP = new IPEndPoint(ipAddress, pPort);

                // Create a TCP/IP socket.  
                mSocket = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream, 
                    ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                mSocket.BeginConnect(
                    mRemoteEP,
                    new AsyncCallback(ConnectCallback), 
                    mSocket);

                connectDone.WaitOne();

                // Send test data to the remote device.  
                Send(mSocket, "This is a test<EOF>");
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(mSocket);
                receiveDone.WaitOne();

                // Write the response to the console.  
                Console.WriteLine("Response received : {0}", response);

            }
            catch (Exception e) 
            {
                Console.WriteLine("Can't connect to the server: {0}", e.ToString());
            }

            return true;
        }

        public void Disconnect()
        {
            // Release the socket.  
            mSocket.Shutdown(SocketShutdown.Both);
            mSocket.Close();
            Console.WriteLine("Socket closed");
        }

        public void ConnectCallback(IAsyncResult pAr)
        {
            try 
            {
                // get the socket from the state object
                Socket client = (Socket)pAr.AsyncState;

                client.EndConnect(pAr);

                Console.WriteLine(
                    "Connected to {0}", 
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch(Exception e)
            {
                Console.WriteLine(
                    "Can't connect to the server: {0}", 
                    e.ToString());
            }
        }

        public void Receive(Socket pSocket)
        {
            try 
            {
                // Create the state object.  
                SocketStateObject state = new SocketStateObject();
                state.mClientSocket = pSocket;

                // Begin receiving the data from the remote device.  
                pSocket.BeginReceive(
                    state.mBuffer,
                    0,
                    SocketStateObject.mBufferSize, 
                    0,
                    new AsyncCallback(ReceiveCallback), 
                    state);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Can't receive data from the server: {0}",
                    e.ToString());
            }
        }

        public void ReceiveCallback(IAsyncResult pAr)
        {
            try 
            {
                SocketStateObject state = (SocketStateObject)pAr.AsyncState;
                Socket client = state.mClientSocket;

                int bytesRead = client.EndReceive(pAr);

                if(bytesRead > 0)
                {
                    // Store the received data
                    state.mSb.Append(
                        Encoding.ASCII.GetString(
                            state.mBuffer, 
                            0, 
                            bytesRead));

                    client.BeginReceive(
                        state.mBuffer,
                        0,
                        SocketStateObject.mBufferSize,
                        0,
                        new AsyncCallback(ReceiveCallback),
                        state);
                }
                else 
                {
                    if(state.mSb.Length>1)
                    {
                        response = state.mSb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Can't receive data from the server: {0}",
                    e.ToString());
            }
        }

        public void Send(Socket pClient, string pData)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(pData);

            pClient.BeginSend(
                byteData,
                0,
                byteData.Length,
                0,
                new AsyncCallback(SendCallback),
                pClient);
        }

        public void SendCallback(IAsyncResult pAr)
        {
            try 
            {
                Socket client = (Socket)pAr.AsyncState;

                int bytesSent = client.EndSend(pAr);
                Console.WriteLine(
                    "Sent {0} bytes to server.", 
                    bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Can't send data to the server: {0}",
                    e.ToString());
            }
        }

    }
}