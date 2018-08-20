using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Gestion_Objet;

namespace Gestion_Serveur
{
    /// <summary>
    /// Contient les infos de connection au serveur
    /// </summary>
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

        #region Gestion connection

        /// <summary>
        /// établie la communication avec le serveur
        /// </summary>
        /// <param name="nomdecarte">Nom stocker sur la carte FPGA pour vérifier si c'est la bonne carte</param>
        /// <returns>Renvoie le message d'erreur ou un ok_com</returns>
        public static string StartClient(string nomdecarte)
        {
            cartefpga = new StateObject();

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.   
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
                return Verif_si_bonne_carte(nomdecarte);//renvoi si c'est la bonne carte
            }
            catch (Exception e)
            {
                GestionLog.Log_Write_Time(e.ToString());//inscrit l'erreur dans le log
                cartefpga.etat_com = e.ToString();
                return "error";
            }
        }
        
        /// <summary>
        /// Lance une requête adresse 0 et vérifie si le nom de carte correspond
        /// </summary>
        /// <param name="nomdecarte">nom de la carte normal</param>
        /// <returns>réponse qui indique si c'est la bonne carte</returns>
        private static string Verif_si_bonne_carte(string nomdecarte)
        {
            string réponse = Send_data("000000000000000000");
            string retour;
            if (réponse.Length == 18)//détecte si la réponse fait la bonne longueur
            {
                if (réponse.Substring(8) == nomdecarte)//vérifie si la réponse est juste
                {
                    retour = "co_ok";
                }
                else
                {
                    retour = "Mauvaise_carte";
                }
            }
            else
            {
                retour = "-1";
            }
            return retour;
        }

        /// <summary>
        /// Déconnecte du serveur
        /// </summary>
        public static void Deco_serveur()
        {
            cartefpga.workSocket.Shutdown(SocketShutdown.Both);
            cartefpga.workSocket.Close();
        }
        #endregion

        #region Envoie Réception

        /// <summary>
        /// Envoie une données
        /// </summary>
        /// <param name="data">Message sous forme de : 1bit 1=écriture/0=lecture, adresse 7bits,message 10bits</param>
        /// <returns>La valeur reçu</returns>
        public static string Send_data(string data)
        {
            string msg = "-1";
            try
            {
                Send(cartefpga.workSocket, data);
                bool sendok = sendDone.WaitOne(2000);//attend 2s ou jusqu'a ce que l'émission soit fini
                
                if (sendok)//si l'émission fini en moins de 2s
                {
                    Receive(cartefpga.workSocket);//demande la réception
                    sendok = receiveDone.WaitOne(2000);//attend 2s ou jusqu'a ce que la réception soit fini soit fini

                    receiveDone.Reset();
                    if (sendok)
                    {
                        msg = response;
                    }
                }
                return msg;
            }
            catch (Exception e)
            {
                GestionLog.Log_Write_Time(e.ToString());
                return msg;
            }
        }

        /// <summary>
        /// Lance la réception des données (même chose que l'envoie)
        /// </summary>
        /// <param name="data">Message sous forme de : 1bit 1=écriture/0=lecture, adresse 7bits,message 10bits</param>
        /// <returns>La valeur reçu</returns>
        public static String Receive_data(string data)
        {
            return Send_data(data);
        }
        
        /// <summary>
        /// indique quand la connection fonctionne
        /// </summary>
        /// <param name="ar">Statut de la connection</param>
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
                GestionLog.Log_Write_Time(e.ToString());
            }
        }
        
        /// <summary>
        /// Initialise le début de la réception
        /// Créer une callback pour savoir quand la donnée a été reçu
        /// </summary>
        /// <param name="client"></param>
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
                GestionLog.Log_Write_Time(e.ToString());
            }
        }

        /// <summary>
        /// Tourne en récursive jusqu'a ce que la valeur a été reçu en entier
        /// appelé par la réception de donnée
        /// le retour se fera sur la variable local response
        /// </summary>
        /// <param name="ar">Statut de la connection</param>
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
                GestionLog.Log_Write_Time(e.ToString());
            }
        }
        
        /// <summary>
        /// Envoie les données
        /// </summary>
        /// <param name="client">à qui il envoie les donnée</param>
        /// <param name="data">la donné qu'il transforme en code ascii</param>
        /// <returns>le statut de la connection</returns>
        private static void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            // et rajoute le retour à la ligne
            byte[] byteData = Encoding.ASCII.GetBytes(data + "\n");
            
            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }


        /// <summary>
        /// indique quand la connection fonctionne
        /// </summary>
        /// <param name="ar">Statut de la connection</param>
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
                GestionLog.Log_Write_Time(e.ToString());
            }
        }
        #endregion

    }
}
