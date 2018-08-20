using Gestion_Serveur;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Connection_Carte_FPGA
{
    static class Connection_au_serveur
    {
        static private Etat_de_connection Etat_co;
        // Demander la connexion au serveur et gérer les boutons en 
        // fonction de la réponse du serveur
        public static void Gestion_tentative_connection(Etat_de_connection etat_co)
        {
            Etat_co = etat_co;
            Etat_co.Etat_de_connection_actuel = 10; // état de connexion en cours

            // Lancer la demande de connexion de côté sans bloquer l'utilisateur
            BackgroundWorker bgw_Gestion_connect_serveur = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
            };

            bgw_Gestion_connect_serveur.DoWork += new DoWorkEventHandler(tentative_connection_serveur_DoWork);
            bgw_Gestion_connect_serveur.ProgressChanged += new ProgressChangedEventHandler(Bgw_co_serv_ProgressChanged);
            bgw_Gestion_connect_serveur.RunWorkerAsync();
        }

        // Lancer la connexion de côté
        private static void tentative_connection_serveur_DoWork(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0, AsynchronousClient.StartClient("0000000001"));
        }

        private static void Bgw_co_serv_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string test = e.UserState.ToString();
            if (test == "co_ok")
            {
                Etat_co.Etat_de_connection_actuel = 11; // connexion réussi
            }
            else if (test == "Mauvaise_carte")
            {
                Etat_co.Etat_de_connection_actuel = 8;
            }
            else
            {
                Etat_co.Etat_de_connection_actuel = 9; // connection échouée
            }
        }
    }
}
