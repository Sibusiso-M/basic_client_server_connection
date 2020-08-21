using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketProgrammingConsoleType
{
    class Client
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Client Side");
            ExecuteClient();
        }

        static void ExecuteClient()
        {
            /* 1. Establish the remote endpoint
             * 2. eg. 2222 port on local computer
             */

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine("Host Name ->{0}", ipHost.HostName);  
            IPAddress ipAddressObj = ipHost.AddressList[0];
            Console.WriteLine("Host Address ->{0}", ipHost.AddressList);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddressObj, 2222);

            /*Create TCP/IP socket using Socket class constructor */
            Socket sender = new Socket(ipAddressObj.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            try
            {
                /* Connect socket to the remote endpoint using method Connect() */
                sender.Connect(localEndPoint);

                /* End point informaton */
                Console.WriteLine("Socket Connected to -> {0}", sender.RemoteEndPoint.ToString());

                /* Message that will be sent to server */
                byte[] messageSent = Encoding.ASCII.GetBytes("Test By Client <EOF>");
                int byteSend = sender.Send(messageSent);

                /* Data buffer */
                byte[] messageReceived = new byte[1024];

                /* Message received using method Recieve(). Returns the number of byts received, that we'll use to convert to string */
                int byteReceived = sender.Receive(messageReceived);
                Console.WriteLine("Message Received from Server -> {0} ", Encoding.ASCII.GetString(messageReceived, 0, byteReceived));

                /* Close Socket sender and receiver, 
                 * Close Socket connection */
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketExeption : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0} ",e.ToString());
            }
            
        }
    }
}
