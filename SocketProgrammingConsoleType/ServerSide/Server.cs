using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerSide
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Side");
            ExecuteServer();
        }

       public static void ExecuteServer()
        {
            /*
             *Establish the LocalEndPoint for the Socket.
             * Dns.GetHostName Returns The Name of the host running the application.
             */

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine("Host Name ->{0}", ipHost.HostName);

            IPAddress ipAddressObj = ipHost.AddressList[0];
            Console.WriteLine("Host Address ->{0}",ipHost.AddressList);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddressObj, 2222);

            /* Tcp/IP Socket , using Socket Class Constructor */
            Socket listener = new Socket(ipAddressObj.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                /* Bind method to associate a network address to the server Socket
                 * All clients to connect to this Server Socket must know this network Address*/
                listener.Bind(localEndPoint);
                Console.WriteLine("Local End Point Address -> {0}",localEndPoint);

                /* Using Listen() method to create the Client list that will want to connect to the Server */

                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ...");

                    /* Suspend while waiting for incoming connection 
                     * Using Accept() method
                     * the Server will acccept the connection of client
                     */
                    Socket clientSockets = listener.Accept();

                    /* Data buffer */

                    byte[] bytes = new byte[1024];
                    string data = null;

                    while (true)
                    {
                        int numByte = clientSockets.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text Received -> {0}",data);
                    byte[] messageToClient = Encoding.ASCII.GetBytes("Test Server");

                    /* Send message to Client
                     * using Send() method
                     */
                    clientSockets.Send(messageToClient);

                    /* Close client Socket send/receives
                     * using Close() method
                     * After closing, closed Socket can be used for a new Client Connection
                     */

                    clientSockets.Shutdown(SocketShutdown.Both);
                    clientSockets.Close();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Unexpected exeption : {0}",e.ToString()); ;
            }

        }
    }
}