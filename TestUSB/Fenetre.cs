using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Gestion_Objet;
using Gestion_Serveur;
using DataFPGA;

namespace GeCoSwell
{
    public partial class Fenetre1 : Form
    {

        //nouvelle version
        private List<IGB_Spéciaux> Li_Gb_spéciaux = new List<IGB_Spéciaux>();
        private List<Consigne> Li_consigne = new List<Consigne>();
        private List<Bras> Li_Bras = new List<Bras>();
        private DoublePulse doublepulse;
        private ParamètresGénéraux paramètresGénéraux;
        private Fréquence fréquence;
        private CAN can;

        private DataGridView_pour_FPGA DGV;

        // textbox et checkbox qui contiennent les variables de la fenetre option.
        TextBox tb_quartus_stpexe;
        TextBox tb_TCL;
        CheckBox chb_AutoLoad;
        CheckBox chb_ModeDaltonien;
        CheckBox chb_valid_expert;
        
        // enregistrement contenant l'etat de la connexion
        // et le message d'erreur en cas d'erreur de transmission 
        public struct SEtat_co
        {
            public int etat;
            public string erreur;

            public SEtat_co(int p1, string p2)
            {
                this.etat = p1;
                this.erreur = p2;
            }
         
        }

        private string erreur;  // message d'erreur lors de l'envoi
        private int etat_co;    // etat de la connexion avec le serveur
        // private object contenu_graphique;

        // propriété chargé de gérer le changement de connexion
        public int Etat_de_connection
        {
            get
            {
                return this.etat_co;
            }
            set
            {
                if (etat_co != value)   // changement de l'etat de la connexion
                {
                    etat_co = value;    // affecter la nouvelle valeur
                    this.Changement_Etat_Connect(this, EventArgs.Empty); // changer l'etat de la connexion
                }
            }
        }
        
        public Fenetre1()
        {
            InitializeComponent();
            

            StateObject cartefpga = new StateObject();
            this.Closing += new CancelEventHandler(this.MainForm_Closing);

            // initialiser la valeur de l'etat de connexion
            Etat_de_connection = -99;

            // gerer le chargement des valeurs dans le .ini
            // les objets sont générés mais n'existent pas vraiment
            this.tb_quartus_stpexe = new TextBox() { Name = "tb_quartus_stpexe" };                      // création d'une textbox pour contenir l'info de l'option
            this.tb_TCL = new TextBox() { Name = "tb_TCL" };                                            // création d'une textbox pour contenir l'info de l'option
            this.chb_AutoLoad = new CheckBox() { Name = "ch_AutoLoad", Checked = false };               // création d'une checkbox pour contenir l'info de l'option
            this.chb_ModeDaltonien = new CheckBox() { Name = "chb_ModeDaltonien", Checked = false };    // création d'une textbox pour contenir l'info de l'option
            this.chb_valid_expert = new CheckBox() { Name = "chb_valid_expert", Checked = false };      // création d'une textbox pour contenir l'info de l'option

            this.Chargement_des_Options();

            // initialiser la liste des adresses FPGA et leurs correspondances
        }

        
        // gerer les boutons et labels à chaque changement d'état de connexion 
        // Etat_de_connection < 0 : serveur down
        // 0 < Etat_de_connection < 10 : serveur on mais non connecté
        // 10 < Etat_de_connection < 20 : serveur connecté sans transfert 
        // 20 < Etat_de_connection : trandfert de données en cours
        public void Changement_Etat_Connect(object sender, EventArgs e)
        {   
            // initialement tout est désactivé
            Changer_Enabled_bt_serveur_en(false);          
            Changer_Enabled_bt_co_serveur_en(false);       
            Changer_Enabled_bt_transfert_data_en(false);   

            if (10 < this.Etat_de_connection && this.Etat_de_connection < 20)
            {
                Changer_Enabled_bt_transfert_data_en(true);
            }
            else
            {
                if (Etat_de_connection < 10 && Etat_de_connection >= 0)
                {
                    Changer_Enabled_bt_co_serveur_en(true);
                }
                else
                {
                    if (Etat_de_connection < 0)
                    {
                        Changer_Enabled_bt_serveur_en(true);
                    }
                }

            }

            this.l_info_connection.ForeColor = Color.Black; // couleur par défaut des infos sur la connexion

            // afficher des indications de fonctionnement de l'application et/ou
            // l'état de fonctionnement des composants de l'application
            switch (Etat_de_connection)
            {
                case -4:
                    this.l_info_connection.Text = "Les options sont correctes, vous pouvez lancer le serveur.";
                    break;
                case -3:
                    this.l_info_connection.ForeColor = Color.Red;
                    this.l_info_connection.Text = "Le serveur s'est fermé, veuillez le relancer.";
                    break;
                case -2:
                    this.l_info_connection.ForeColor = Color.Red;
                    this.l_info_connection.Text = "Options à mettre à jour, cliquer sur outil puis option.";
                    Changer_Enabled_bt_serveur_en(false);
                    break;
                case 0:
                    this.l_info_connection.Text = "Serveur lancé, attendez que la ligne :\n \"Started Socket Server on port - 2540\" apparaissent avant de cliquer sur connexion.";
                    break;
                case 1:
                    this.l_info_connection.Text = "Déconnexion réussie.";
                    break;
                case 8:
                    this.l_info_connection.ForeColor = Color.Red;
                    this.l_info_connection.Text = "Connection avec la mauvaise carte FPGA";
                    break;
                case 9:
                    this.l_info_connection.ForeColor = Color.Red;
                    this.l_info_connection.Text = "La connexion a échoué avec le serveur. Réessayez, si l'échec persiste relancez le serveur.";
                    break;
                case 10:
                    this.déconnectéToolStripMenuItem.Enabled = true;
                    this.l_info_connection.Text = "Connexion en cours avec le serveur...";
                    break;
                case 11:
                    this.l_info_connection.Text = "Connexion réussie, vous pouvez commencer à échanger des données.";
                    break;
                case 12:
                    this.l_info_connection.Text = "Envoi terminé.";
                    this.b_Envoi.Image = GeCoSwell.Properties.Resources.icons8_send_16;
                    break;
                case 13:
                    this.l_info_connection.Text = "Réception terminé.";
                    break;
                case 14:
                    l_info_connection.ForeColor = Color.Red;
                    this.l_info_connection.Text = "L'envoi a échoué." + "\n" + /*gestion_etat_co.*/erreur;
                    break;
                case 15:
                    this.l_info_connection.ForeColor = Color.Red;
                    this.l_info_connection.Text = "La réception a échoué.";
                    break;
                case 20:
                    this.l_info_connection.Text = "Envoi en cours.";
                    break;
                case 21:
                    this.l_info_connection.Text = "Réception en cours.";
                    break;
                case 22:
                    this.l_info_connection.Text = "Mesure en cours.";
                    break;
            }
        }


        // Attribuer un état au bouton qui gère le serveur
        // Paramètre : 
        //      bool etat a affecté aux boutons
        private void Changer_Enabled_bt_serveur_en(bool etat)
        {
            // modifier l'état du bouton pour lancer le serveur
            this.b_lancer_serveur.Enabled = etat;
            this.tsm_serveur.Enabled = etat;
        }


        // Attribuer un état au bouton qui gèrent la connexion au serveur
        // Paramètre :
        //      bool etat a affecé aux boutons
        private void Changer_Enabled_bt_co_serveur_en(bool etat)
        {
            //bouton de connection au serveur
            this.b_Connection.Enabled = etat;
            this.tsm_connect.Enabled = etat;
        }


        // Attribuer un nouvel état aux boutons qui gèrent l'envoi
        // ou la réception de la donnée data
        // Paramètre :
        //      bool etat a affecté aux boutons
        private void Changer_Enabled_bt_transfert_data_en(bool etat)
        {
            //bouton de transfert de data
            this.b_Envoi.Enabled = etat;
            this.b_Reception.Enabled = etat;
            this.tsm_send.Enabled = etat;
            this.tsm_reception.Enabled = etat;
            this.tsm_deconnecte.Enabled = etat;
        }

        
        // Demander la connexion au serveur et gérer les boutons en 
        // fonction de la réponse du serveur
        private void B_Communication_Click(object sender, EventArgs e)
        {
            Etat_de_connection = 10; // état de connexion en cours

            // Lancer la demande de connexion de côté sans bloquer l'utilisateur
            BackgroundWorker bgw_co_serv = new BackgroundWorker()
            {WorkerReportsProgress = true,
            };
            
            bgw_co_serv.DoWork += new DoWorkEventHandler(Bgw_co_serv_DoWork);
            bgw_co_serv.ProgressChanged += new ProgressChangedEventHandler(Bgw_co_serv_ProgressChanged);
            bgw_co_serv.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bgw_co_serv_RunWorkerCompleted);
            bgw_co_serv.RunWorkerAsync();
        }
        
        // Lancer la connexion de côté
        private void Bgw_co_serv_DoWork(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0,AsynchronousClient.StartClient("0000000001"));
        }
        
        private void Bgw_co_serv_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string test = e.UserState.ToString();
            if (test == "co_ok")
            {
                this.Etat_de_connection = 11; // connexion en cours
            }
            else if (test == "Mauvaise_carte")
            {
                this.Etat_de_connection = 8;
            }
            else
            {
                this.Etat_de_connection = 9; // connection échouée
            }
            //voir si j'en fait quelque chose
        }


        private void Bgw_co_serv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //voir si je fait quelque chose quand le code est fini
        }

        // Envoyer un signal à la carte FPGA
        private void B_Envoi_Click(object sender, EventArgs e)
        {
            Etat_de_connection = 20;// état d'envoi en cours
            string send_ok = "";



                if (send_ok != "")
                {// une erreur de transfert est survenue
                    //gestion_etat_co.erreur = send_ok;
                    erreur = send_ok;
                    Etat_de_connection = 14;
                    return;                                         // arrêter le transfert
                }

                List<String> type_connexion = new List<string>() { "ALL", "BO" };
                

                for (int i = 0; i < type_connexion.Count; i++)
                {
                    if (send_ok != "")
                    {// une erreur de transfert est survenue
                        //gestion_etat_co.erreur = send_ok;
                        erreur = send_ok;
                        Etat_de_connection = 14;
                        return;                                         // arrêter le transfert
                    }
                }
                
            // INDIQUER QUE LA DONNEE A ETE ENVOYEE
            //b_Envoi.Image = GeCoSwell.Properties.Resources.icons8_send_16; 
            Etat_de_connection = 12; // état de l'envoi terminé
        }

        // Récupérer les informations de la carte quand on clique
        // sur le bouton réception de l'interface
        private void B_Reception(object sender, EventArgs e)
        {
            Etat_de_connection = 21;//réception en cour
            /*
            if (send_ok == "-1")//si erreur de transfert
            {
                Etat_de_connection = 15;
                return;//dans ce cas la on arrête le transfert
            }*/

            Etat_de_connection = 13;//réception terminé
        }


        // Ouvrir le fichier log
        private void B_open_log_Click(object sender, EventArgs e)
        {
            GestionLog.Log_Open();
        }
        

        // Fermer l'application
        private void QuitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        // Saisir la confirmation pour quitter l'application
        // Retour :
        //      bool true si oui et false si non
        private bool AskConfirmQuitAppli()
        {
            // message confirmation quitter l'application
            if (MessageBox.Show("Quitter l'application?",
                               "Message de confirmation",
                               MessageBoxButtons.YesNo) == DialogResult.No)
                // non
                return false;

            // oui, quitter
            return true;
        }


        // S'activer lors de la fermeture de l'application
        // pour demander s'il faut réellement fermer l'application
        private void MainForm_Closing(Object sender, CancelEventArgs e)
        {
            if (AskConfirmQuitAppli() == false)
                e.Cancel = true;
        }

        // Initialiser tous les composants de la fenêtre de l'application
        private void Load_fenetre(object sender, EventArgs e)
        {
            Lancer_serveur();
        }

        
        // Lancer le serveur et bloquer les boutons correspondant
        private void B_lancer_serveur_Click(object sender, EventArgs e)
        {
            Lancer_serveur();
        }

        // Lancer la connexion au serveur : l'utilisation d'un 
        // backgroundworker permet à ihm de tourner même quand 
        // la connexion est tenté ou même si elle subit un ralentissement.
        private void Lancer_serveur()
        {
            Etat_de_connection = 0; // serveur se lance

            // Lancer la demande de connexion de coté sans bloquer l'utilisateur
            BackgroundWorker bgw_serveur = new BackgroundWorker()
            {
                WorkerReportsProgress = true
            };
            
            bgw_serveur.DoWork += new DoWorkEventHandler(Bgw_serveur_DoWork);
            bgw_serveur.ProgressChanged += new ProgressChangedEventHandler(Bgw_serveur_ProgressChanged);
            bgw_serveur.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bgw_serveur_RunWorkerCompleted);
            bgw_serveur.RunWorkerAsync();
        }
        
        // Lancer de coté la connexion sur un backgroundworker et 
        // détecter si quartus_stp fonctionne toujours et envoiyer un message
        // quand il est coupé. Si quartus_stp n'est pas lancé, il le lance
        private void Bgw_serveur_DoWork(object sender, DoWorkEventArgs e)
        {
            bool b = true;
            int quartus_exist = 0;
            while (b)
            {
                System.Threading.Thread.Sleep(1000);
                List<string> list_process = new List<string>(); // lister le nom de tous les process
                Process[] processes = Process.GetProcesses();   // option la liste des noms des process

                foreach (Process process in processes)
                {
                    list_process.Add(process.ProcessName);      // les mettre dans la liste de string
                }

                if (list_process.Contains("quartus_stp"))
                {// si quartus_stp tourne
                    quartus_exist = 1;
                }
                else
                {// si quartus n'existe pas
                    if (quartus_exist == 0)
                    {// si c'est le premier tour, il faut le lancer
                        if (File.Exists(tb_quartus_stpexe.Text) && File.Exists(tb_TCL.Text))
                        {
                            try // essayer de lancer le serveur
                            {
                                Process serveur = Process.Start(tb_quartus_stpexe.Text, "-t " + tb_TCL.Text);
                            }
                            catch // si un bug apparait
                            { 
                                ((BackgroundWorker)sender).ReportProgress(-2); // propager l'erreur
                                b = false; // finir le bgw
                            }

                        ((BackgroundWorker)sender).ReportProgress(0);
                            quartus_exist = 1;
                        }
                        else
                        {
                            ((BackgroundWorker)sender).ReportProgress(-2);
                            b = false;
                        }
                    }
                    else
                    {
                        ((BackgroundWorker)sender).ReportProgress(-3);
                        b = false;
                    }
                }
            }
        }

        // Recevoir les informations sur l'évolution du BGworker et les traiter
        private void Bgw_serveur_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // l_info_connection.Text = (string)e.UserState; // récupérer le texte de progession
            Etat_de_connection = e.ProgressPercentage;  // Envoiyer l'état de la connexion
        }


        // S'effectuer quand le serveur meurt
        private void Bgw_serveur_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {    
            // rien
        }

        // Se déconnecter et gérer tous les boutons (rendre enable et disable les bons boutons)
        private void Tsm_deco_Click(object sender, EventArgs e)
        {
            AsynchronousClient.Deco_serveur();
            Etat_de_connection = 1;
        }

        private void Maj_Inversion(object sender,CheckBox chb_bras, TextBox tb_val, int position)
        {
            if (tb_val == sender)
            {
                GestionObjet.Conv_chb_string(1, 0, chb_bras,  tb_val.Text.Substring(position,1));
            }

            if (chb_bras == sender)
            {
                GestionObjet.String_replace(tb_val, GestionObjet.Conv_chb_string(1, 1, chb_bras), position);
            }
        }


        //----------------------------------------------------------------------
        //Calcule des cases manuel ou maj le reste par rapport à manuel
        //----------------------------------------------------------------------
        private void Link_tb_manuel(object sender)
        {

            // INDIQUER QUE LA DONNEE A ETE MODIFIEE MAIS PAS ENVOYEE
            this.b_Envoi.Image = GeCoSwell.Properties.Resources.icons8_no_send_16;
            
        }

        //----------------------------------------------------------------------
        //Fonction pour afficher ou cacher les textbox suivant deux paramètre
        //----------------------------------------------------------------------
        private void Cmb_gestion_consigne(TextBox tb_cons,TextBox tb_limit,ComboBox cmb_CAN,int choix,TextBox tb_div_sinus,GroupBox gb_sinus_fpga,Label l_consigne_can, ComboBox cmb_consigne_can,TextBox amplitude, TextBox offset)
        {
            tb_cons.Visible = (choix == 0) ? true : false;//si choix 1 = 0 alors visible sinon caché
            //tb_limit.Visible = (choix1 == 0) ? false : true;//si choix 1 = 0 alors caché sinon visible
            tb_limit.Visible = true;
            cmb_CAN.Visible = (choix == 2) ? true : false;//si choix 1 = 2 alors visible sinon caché
            tb_div_sinus.Visible = (choix == 1) ? true : false;
            gb_sinus_fpga.Visible = (choix == 1) ? true : false;
            l_consigne_can.Location = new Point(l_consigne_can.Location.X, gb_sinus_fpga.Location.Y +3);
            cmb_consigne_can.Location = new Point(cmb_consigne_can.Location.X, gb_sinus_fpga.Location.Y);
            offset.Text = "512";
            amplitude.Text = "1023";

        }

        //----------------------------------------------------------------------
        //Fonction qui refait les dessins quand on change l'index
        //----------------------------------------------------------------------
        private void Cb_Choix_DP_Transistor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Link_tb_manuel(sender);
        }
        
        //----------------------------------------------------------------------
        //Fonction qui met tout les paramètres important dans une liste et appel la fonction sauvegarde
        //----------------------------------------------------------------------
        private void SauvegarderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GestionSave save = new GestionSave("Programation des paramètres|*.txt", "Sauvegarder tout les paramètre actuel");
        }

        //----------------------------------------------------------------------
        //Fonction appeler par le bouton chargment
        //----------------------------------------------------------------------
        private void ChargementToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GestionLoad Charg = new GestionLoad("Programation des paramètres|*.txt", "Chargement des paramètres");

        }
        
        /// <summary>
        /// Charge les valeur du fichier Option.ini et met à jour l'interface en correspondance
        /// </summary>
        private void Chargement_des_Options()
        {/*
            GestionLoad chargement = new GestionLoad();
            chargement.Chargement(this.tb_quartus_stpexe, "Options.ini");
            if (tb_quartus_stpexe.Text != "")
            {
                chargement.Chargement(this.tb_TCL, "Options.ini");
                chargement.Chargement(this.chb_AutoLoad, "Options.ini");
                chargement.Chargement(this.chb_ModeDaltonien, "Options.ini");
                chargement.Chargement(this.chb_valid_expert, "Options.ini");
                //this.chb_send_auto.Enabled = this.chb_valid_expert.Checked;

                if (this.chb_AutoLoad.Checked) // si la checkbox est true alors on charge le fichier autosave
                {
                    chargement.Chargement(this.tp_manuel, "Autosave.txt");
                }
            }*/
        }

        //----------------------------------------------------------------------
        //Fonction qui ouvre les options
        //----------------------------------------------------------------------
        private void Tsm_option_Click(object sender, EventArgs e)
        {
            new Options().ShowDialog(this);

            this.Chargement_des_Options();//chargement des nouvelles valeurs

            this.Refresh();
            if (File.Exists(this.tb_quartus_stpexe.Text) && File.Exists(this.tb_TCL.Text)) // si les fichier existe
            {
                List<string> list_process = new List<string>(); //fait une liste qui va contenir le nom de tout les process
                Process[] processes = Process.GetProcesses();   // optien la liste des noms des process
                foreach (Process process in processes)
                {
                    list_process.Add(process.ProcessName);  //les mets dans la liste de string
                }

                if (!list_process.Contains("quartus_stp")) // si la liste contient quartus_stp on déduit qu'il tourne
                {
                    Etat_de_connection = -4;
                }
            }
        }

        //----------------------------------------------------------------------
        //Fonction pour désactiver la bonne checkbox pour mesure CAN
        //----------------------------------------------------------------------
        private void Cmb_consigne_CAN_SelectedIndexChanged(object sender, EventArgs e) //fonction quand on change la valeur du combobox qui choisis la voie pour la consigne
        {
            this.Link_tb_manuel(sender);
        }

        //----------------------------------------------------------------------
        //Fonction qui ouvre la classform gene table sinus pour quartus
        //----------------------------------------------------------------------
        private void Open_Gene_sinusMenuItem_Click(object sender, EventArgs e)
        {
            new Tab_sinus().ShowDialog(this);//ouvre et bloque la fenetre derrière
        }


        //----------------------------------------------------------------------
        //Fonction qui affiche la fenetre à propos
        //----------------------------------------------------------------------
        private void AProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Apropos().ShowDialog(this);//ouvre et bloque la fenetre derrière
        }

        //----------------------------------------------------------------------
        //Fonction activé quand on check la validation d'utilisation d'un can pour détecter une erreur
        //----------------------------------------------------------------------
        private void ChB_CAN_CheckedChanged(object sender, EventArgs e)
        {
            this.Link_tb_manuel(sender);
        }
        
        //----------------------------------------------------------------------
        //Affiche et rend active tout les controls
        //----------------------------------------------------------------------
        private void SuperUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Control> control_T = new List<Control>();
            control_T  = GestionObjet.Crealist(this);
            control_T.AddRange(GestionObjet.Trouver_controls_dun_type(this, typeof(Button)));
            foreach(Control ct in control_T)
            {
                ct.Enabled = true;
            }
            b_lancer_serveur.Enabled = true;
            b_Connection.Enabled = true;
        }
        
        // Format the value with the indicated number of significant digits.
        public string ToSignificantDigits(double value, int significant_digits)
        {

            string format = value.ToString().Substring(0, significant_digits-1);
            return format.EndsWith(".") ? format.Remove(format.Length-1) : format;
        }

        // Modifier la valeur du bit inversion switch dans le manuel
        // pour le bras dont la case "inversion bras haut-bas" est coché
        private void Chb_invbras_CheckedChanged(object sender, EventArgs e)
        {
            this.Link_tb_manuel(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Li_consigne = Consigne.Créer_x_fois_consigne(6, 6, 3, 3);
            this.Li_Bras = Bras.Géné_x_consigne(6, 127, 4, 4);
            foreach (GGroupBox gb in Consigne.Li_Gb_consigne)
            {
                this.panel_modulaire.Controls.Add(gb);
            }
            foreach (GGroupBox gb in Bras.Li_Gb_bras)
            {
                this.panel_modulaire.Controls.Add(gb);
            }
            Consigne.Lié_position_dessous(Bras.Li_Gb_bras);
            this.doublepulse = new DoublePulse(6, 6, false);
            this.panel_modulaire.Controls.Add(doublepulse.Gb_Principal);
            this.paramètresGénéraux = new ParamètresGénéraux(782, 140, true);
            this.Controls.Add(paramètresGénéraux.Gb_Principal);
            this.fréquence = new Fréquence(1063, 140, true);
            this.Controls.Add(this.fréquence.Gb_Principal);
            this.can = new CAN(782, 262);
            this.Controls.Add(this.can.Gb_Principal);


            this.Li_Gb_spéciaux.AddRange(this.Li_Bras);
            this.Li_Gb_spéciaux.AddRange(this.Li_consigne);
            this.Li_Gb_spéciaux.Add(this.doublepulse);
            this.Li_Gb_spéciaux.Add(this.paramètresGénéraux);
            this.Li_Gb_spéciaux.Add(this.fréquence);
            this.Li_Gb_spéciaux.Add(this.can);
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            Consigne.Gb_Pid_visibilité(((CheckBox)sender).Checked);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DGV = new DataGridView_pour_FPGA(6, 6,this.Li_Gb_spéciaux);
            this.panel_modulaire.Controls.Add(this.DGV);
            this.DGV.BringToFront();
        }

        private void Cmb_mode_puissance_SelectedIndexChanged(object sender, EventArgs e)
        {
            Consigne.Changement_mode_de_consigne(this.cmb_mode_puissance.SelectedIndex);

/*
Boucle ouverte
Boucle fermé
Opposition
Complet
Mode Triphasé
*/
        }
    }
}
 