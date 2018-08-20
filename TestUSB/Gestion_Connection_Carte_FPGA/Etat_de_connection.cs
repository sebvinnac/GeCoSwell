using GeCoSwell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Connection_Carte_FPGA
{
    class Etat_de_connection
    {

        private string MSG_dErreur_dEnvoie;
        private int Etat_co_actuel;
        private Fenetre1 Affichage;

        public int Etat_de_connection_actuel
        {
            get
            {
                return this.Etat_co_actuel;
            }
            set
            {
                if (Etat_co_actuel != value)
                {
                    Etat_co_actuel = value;
                    this.Changement_Etat_Connect();
                }
            }
        }


        public Etat_de_connection(Fenetre1 fenetre)
        {
            this.Affichage = fenetre;
            this.Etat_co_actuel = - 9;
        }

        public void Changement_Etat_Connect()
        {
            this.Changer_letat_des_textbox_de_connection();

            // afficher des indications de fonctionnement de l'application et/ou
            // l'état de fonctionnement des composants de l'application
            switch (this.Etat_co_actuel)
            {
                case -11:
                    Affichage.Changer_text_l_info_connection("Options à mettre à jour, cliquer sur outil puis option.", Color.Red);
                    break;
                case -4:
                    Affichage.Changer_text_l_info_connection("Les options sont correctes, vous pouvez lancer le serveur.", Color.Black);
                    break;
                case -3:
                    Affichage.Changer_text_l_info_connection("Le serveur s'est fermé, veuillez le relancer. \nVérifier que vous avez connecté la carte FPGA.", Color.Red);
                    break;

                case 0:
                    Affichage.Changer_text_l_info_connection("Serveur lancé, attendez que la ligne :\n \"Started Socket Server on port - 2540\" apparaissent avant de cliquer sur connexion.", Color.Black);
                    break;
                case 1:
                    Affichage.Changer_text_l_info_connection("Déconnexion réussie.", Color.Black);
                    break;
                case 8:
                    Affichage.Changer_text_l_info_connection("Connection avec la mauvaise carte FPGA", Color.Red);
                    break;
                case 9:
                    Affichage.Changer_text_l_info_connection("La connexion a échoué avec le serveur. Réessayez, si l'échec persiste relancez le serveur.", Color.Red);
                    break;
                case 10:
                    Affichage.Changer_text_l_info_connection("Connexion en cours avec le serveur...", Color.Black);
                    break;
                case 11:
                    Affichage.Changer_text_l_info_connection("Connexion réussie, vous pouvez commencer à échanger des données.", Color.Black);
                    break;
                case 12:
                    Affichage.Changer_text_l_info_connection("Envoi terminé.", Color.Black);
                    break;
                case 13:
                    Affichage.Changer_text_l_info_connection("Réception terminé.", Color.Black);
                    break;
                case 14:
                    Affichage.Changer_text_l_info_connection("L'envoi a échoué.\n" + /*gestion_etat_co.*/MSG_dErreur_dEnvoie, Color.Red);
                    break;
                case 15:
                    Affichage.Changer_text_l_info_connection("La réception a échoué.", Color.Red);
                    break;
                case 20:
                    Affichage.Changer_text_l_info_connection("Envoi en cours.", Color.Black);
                    break;
                case 21:
                    Affichage.Changer_text_l_info_connection("Réception en cours.", Color.Black);
                    break;
                case 22:
                    Affichage.Changer_text_l_info_connection("Mesure en cours.", Color.Black);
                    break;
            }
        }

        private void Changer_letat_des_textbox_de_connection()
        {
            bool bouton_connection_serveur = false;
            bool bouton_lancer_serveur = false;
            bool bouton_transfert_data_serveur = false;

            if (this.Connecté_au_serveur_sans_transfert_en_cour())
            {
                bouton_transfert_data_serveur = true;
            }
            else if (this.Serveur_on_mais_non_connecté())
            {
                bouton_connection_serveur = true;
            }
            else if (Serveur_off_a_relancer())
            {
                bouton_lancer_serveur = true;
            }
            Affichage.Changer_letat_Enabled_des_bt_Lancer_serveur(bouton_lancer_serveur);
            Affichage.Changer_letat_Enabled_des_bt_connection_serveur(bouton_connection_serveur);
            Affichage.Changer_letat_Enabled_des_bt_de_transfert_data(bouton_transfert_data_serveur);
            
        }


        // Etat_de_connection < -10 : serveur off option à changer
        // -10 < Etat_de_connection < 0 : serveur off à relancer
        // 0 < Etat_de_connection < 10 : serveur on mais non connecté
        // 10 < Etat_de_connection < 20 : serveur connecté sans transfert 
        // 20 <= Etat_de_connection : transfert de données en cours

        private bool Connecté_au_serveur_sans_transfert_en_cour()
        {
            return 10 < this.Etat_co_actuel && this.Etat_co_actuel < 20;
        }
        private bool Serveur_on_mais_non_connecté()
        {
            return this.Etat_co_actuel < 10 && this.Etat_co_actuel >= 0;
        }
        private bool Serveur_off_a_relancer()
        { 
            return this.Etat_co_actuel < 0 && this.Etat_co_actuel > -10;
        }

    }
}
