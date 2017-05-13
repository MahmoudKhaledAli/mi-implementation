using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace HexaBotImplementation
{
    class Communication
    {
        Socket S;
        public void StartConnectionAsServer(string ipaddress)
        {
            string address = ipaddress;
            int intAddress = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes(), 0);
            string ip2Address = new IPAddress(BitConverter.GetBytes(intAddress)).ToString();
            IPEndPoint localEndPoint = new IPEndPoint(intAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    Console.WriteLine("Whekkaaan...");
                    S = handler;
                    listener.Close();

                
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
           
            }
        }
        public void StartConnectionAsClient(string ipaddress)
        {
            try
            {
                string address = ipaddress;
                int intAddress = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes(), 0);
                string ip2Address = new IPAddress(BitConverter.GetBytes(intAddress)).ToString();
                IPEndPoint remoteEP = new IPEndPoint(intAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
          
                
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());





                S= sender;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
            }
        }
        public void send(Move M)
        {
            byte[] ack = new byte[1024];
            

            String data = M.GetMoveI().ToString();
            data = data + ",";
            data = data + M.GetMoveJ().ToString();
            int bytesSent = S.Send(Encoding.ASCII.GetBytes(data + "<EOF>"));

            StreamWriter file = new StreamWriter("log.txt", true);
            file.WriteLine(DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "(" + M.GetMoveI() + "," + M.GetMoveJ() + ")" + " received");
            file.Close();
            int bytesRec = S.Receive(ack);

        }
        public Move Receive()
        {
            byte[] bytes = new byte[1024];
            

            string data = null;

            data += Encoding.ASCII.GetString(bytes, 0, S.Receive(bytes));

            int i = Int32.Parse(data.Substring(0, data.Length - 5).Split(',')[0]);
            int j = Int32.Parse(data.Substring(0, data.Length - 5).Split(',')[1]);
            int bytesSend = S.Send(bytes);
            Move M = new Move(i, j);

            StreamWriter file = new StreamWriter("log.txt", true);
            file.WriteLine(DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "(" + i + "," + j + ")" + " sent");
            file.Close();
            return M;
        }
        public void closeconnection()
        {
            S.Close();
        }
    }
    
}
