using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GeCoSwell
{
    //----------------------------------------------------------------------
    //class qui contient les infos de la carte FPGA
    //----------------------------------------------------------------------
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
        public string etat_com = "";
    }

    public class AsynchronousClient
    {
        // The port number for the remote device.  
        private const int port = 2540;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;
        private static System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        public static StateObject cartefpga;


        //----------------------------------------------------------------------
        //Fonction qui établie la communication avec le serveur
        //
        //Renvoie un stateObjet avec le socket
        //Renvoie le message d'erreur ou un ok_com
        //----------------------------------------------------------------------
        public static string StartClient()
        {
            cartefpga = new StateObject();

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the remote device is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");//se connecte sur un serveur qui tourne sur le pc
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                Socket client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();//attends que la connection ai eu lieu
                cartefpga.workSocket = client;
                return "co_ok";//connection établie
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());//inscrit l'erreur dnas le log
                cartefpga.etat_com = e.ToString();
                return "error";
            }
        }

        //----------------------------------------------------------------------
        //Fonction qui Déconnecte du serveur 
        //
        //client composant avec qui on communique
        //data donnée que l'on souhaite envoyer
        //----------------------------------------------------------------------
        public static void Deco_serveur()
        {
            cartefpga.workSocket.Shutdown(SocketShutdown.Both);
            cartefpga.workSocket.Close();
        }


        //----------------------------------------------------------------------
        //Fonction qui envoie des données 
        //
        //client composant avec qui on communique
        //data donnée que l'on souhaite envoyer
        //----------------------------------------------------------------------
        public static string Send_data(string data)
        {
            try
            {
                IAsyncResult r = Send(cartefpga.workSocket, data);
                bool sendok = sendDone.WaitOne(2000);
                
                if (!sendok)
                {
                    return "-1";
                }
                Receive(cartefpga.workSocket);
                sendok = receiveDone.WaitOne(2000);


                receiveDone.Reset();
                if (sendok)
                {
                    return response;
                }
                else
                {
                    return "-1";
                }
                
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
                return "-1";
            }
        }


        //----------------------------------------------------------------------
        //Fonction qui reçoit les paramètres actuel du FPGA
        //
        //client composant avec qui on communique
        //data donnée que l'on souhaite envoyer
        //----------------------------------------------------------------------
        public static String Receive_data(string data)
        {
            try
            {

                IAsyncResult r = Send(cartefpga.workSocket, data);
                sendDone.WaitOne();


                Receive(cartefpga.workSocket);
                bool sendok = receiveDone.WaitOne(2000);


                receiveDone.Reset();
                if (sendok)
                {
                    return response;
                }
                else
                {
                    return "-1";
                }
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
                return "erreur";
            }
        }

        //-----------------------------------------------------
        //fonction qui indique quand la connection fonctionne
        //-----------------------------------------------------
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
            }
        }
        
        //-----------------------------------------------------
        //fonction qui initilise le début de réception
        //
        //et créer une callback pour savoir quand la donné a été reçu
        //-----------------------------------------------------
        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject()
                {   workSocket = client,
                };

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
            }
        }

        //----------------------------------------------------------------------
        //Fonction réception appeler par la réception de donné
        //va tourné en récursive jusqu'a ce que la valeur a été reçu en entier
        //
        //le retour se fera sur la variable response
        //----------------------------------------------------------------------
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Check if there is anymore data on the socket
                    if (client.Available > 0)
                    {
                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                    }
                
                }

                // si pas de donné lu, ou si le client n'a plus rien à dire
                if (bytesRead == 0 || client.Available == 0)
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }

            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
            }
        }

        //----------------------------------------------------------------------
        //Fonction qui envoie des données 
        //
        //client = à qui il envoie les donnée
        //data = la donné qu'il transforme en code ascii
        //----------------------------------------------------------------------
        private static IAsyncResult Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data + "\n");


            // Begin sending the data to the remote device.
            //client.BeginSend(byteData, 0, 1, 0, new AsyncCallback(SendCallback), client);
            return client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }


        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
     
                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
            }
        }

    }
}
