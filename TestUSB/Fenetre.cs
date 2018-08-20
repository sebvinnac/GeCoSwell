using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Gestion_Objet;
using Gestion_Serveur;
using Gestion_Connection_Carte_FPGA;

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
        private Etat_de_connection Etat_co;

        private DataGridView_pour_FPGA DGV_fpga;
        private Gestionaire_denvoi G_denvoi;

        // textbox et checkbox qui contiennent les variables de la fenetre option.
        TextBox tb_quartus_stpexe;
        TextBox tb_TCL;
        CheckBox chb_AutoLoad;
        CheckBox chb_ModeDaltonien;
        CheckBox chb_valid_expert;
        
        public Fenetre1()
        {
            this.InitializeComponent();
            this.Init_Etat_de_connection();
            this.Init_Gestion_des_options();
            this.Init_GB_spéciaux();
            this.Init_Gestionnaire_de_connection();

            this.Closing += new CancelEventHandler(this.MainForm_Closing);
        }


        #region Initialisation de la fenetre


        private void Init_Gestionnaire_de_connection()
        {
            this.G_denvoi = new Gestionaire_denvoi(this.DGV_fpga);
            MAJ_DATA_pour_Envoi.Lier_à_état_co(this.Etat_co);
            MAJ_DATA_pour_Envoi.Lier_à_li_Gb_Spéciaux(this.Li_Gb_spéciaux);
        }

        private void Init_GB_spéciaux()
        {
            this.Li_consigne = Consigne.Créer_x_fois_consigne(6, 6, 3, 1);
            this.Li_Bras = Bras.Géné_x_consigne(6, 127, 4, 2);

            Consigne.Lié_position_dessous(Bras.Li_Gb_bras);
            this.doublepulse = new DoublePulse(6, 6, false);
            this.paramètresGénéraux = new ParamètresGénéraux(782, 140, true);
            this.fréquence = new Fréquence(1063, 140, true);
            this.can = new CAN(782, 262);

            foreach (GGroupBox gb in Consigne.Li_Gb_consigne)
            {
                this.panel_modulaire.Controls.Add(gb);
            }
            foreach (GGroupBox gb in Bras.Li_Gb_bras)
            {
                this.panel_modulaire.Controls.Add(gb);
            }
            this.panel_modulaire.Controls.Add(doublepulse.Gb_Principal);
            this.Controls.Add(paramètresGénéraux.Gb_Principal);
            this.Controls.Add(this.fréquence.Gb_Principal);
            this.Controls.Add(this.can.Gb_Principal);


            this.Li_Gb_spéciaux.AddRange(this.Li_Bras);
            this.Li_Gb_spéciaux.AddRange(this.Li_consigne);
            this.Li_Gb_spéciaux.Add(this.doublepulse);
            this.Li_Gb_spéciaux.Add(this.paramètresGénéraux);
            this.Li_Gb_spéciaux.Add(this.fréquence);
            this.Li_Gb_spéciaux.Add(this.can);
        }

        private void Init_Gestion_des_options()
        {            
            // gerer le chargement des valeurs dans le .ini
            this.tb_quartus_stpexe = new TextBox() { Name = "tb_quartus_stpexe" };
            this.tb_TCL = new TextBox() { Name = "tb_TCL" };                                            
            this.chb_AutoLoad = new CheckBox() { Name = "ch_AutoLoad", Checked = false };
            this.chb_ModeDaltonien = new CheckBox() { Name = "chb_ModeDaltonien", Checked = false };
            this.chb_valid_expert = new CheckBox() { Name = "chb_valid_expert", Checked = false };

            this.Chargement_des_Options();

            if (this.chb_AutoLoad.Checked) // si la checkbox est true alors on charge le fichier autosave
            {
                //TODO
                //chargement.Chargement(this.tp_manuel, "Autosave.txt");
            }
        }

        private void Init_Etat_de_connection()
        {
            Etat_co = new Etat_de_connection(this);
        }

        #endregion

        #region Gestion des boutons du serveur et connection

        public void Changer_letat_Enabled_des_bt_Lancer_serveur(bool etat)
        {
            this.b_lancer_serveur.Enabled = etat;
            this.tsm_serveur.Enabled = etat;
        }

        public void Changer_letat_Enabled_des_bt_connection_serveur(bool etat)
        {
            this.b_Connection.Enabled = etat;
            this.tsm_connect.Enabled = etat;
        }

        public void Changer_letat_Enabled_des_bt_de_transfert_data(bool etat)
        {
            this.b_Envoi.Enabled = etat;
            this.b_Reception.Enabled = etat;
            this.tsm_send.Enabled = etat;
            this.tsm_reception.Enabled = etat;
            this.tsm_deconnecte.Enabled = etat;
            this.déconnectéToolStripMenuItem.Enabled = etat;
        }

        public void Changer_text_l_info_connection(string message,Color couleur)
        {
            this.l_info_connection.Text = message;
            this.l_info_connection.ForeColor = couleur;
        }

        #endregion

        private void B_Connection_serveur_Click(object sender, EventArgs e)
        {
            Connection_au_serveur.Gestion_tentative_connection(this.Etat_co);
        }

        private void B_Envoi_donné_Click(object sender, EventArgs e)
        {
            MAJ_DATA_pour_Envoi.MAJ_Data_puis_envoi(G_denvoi);

            // INDIQUER QUE LA DONNEE A ETE ENVOYEE
            //b_Envoi.Image = GeCoSwell.Properties.Resources.icons8_send_16; 
        }

        // Récupérer les informations de la carte quand on clique
        // sur le bouton réception de l'interface
        private void B_Reception(object sender, EventArgs e)
        {
            this.Etat_co.Etat_de_connection_actuel = 21;//réception en cour
            /*
            if (send_ok == "-1")//si erreur de transfert
            {
                Etat_de_connection = 15;
                return;//dans ce cas la on arrête le transfert
            }*/

            this.Etat_co.Etat_de_connection_actuel = 13;//réception terminé
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
            this.Etat_co.Etat_de_connection_actuel = 0; // serveur se lance

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
            this.Etat_co.Etat_de_connection_actuel = e.ProgressPercentage;  // Envoiyer l'état de la connexion
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
            this.Etat_co.Etat_de_connection_actuel = 1;
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
        {
            GestionLoad chargement = new GestionLoad();
            chargement.Chargement(this.tb_quartus_stpexe, "Options.ini");
            if (tb_quartus_stpexe.Text != "")
            {
                chargement.Chargement(this.tb_TCL, "Options.ini");
                chargement.Chargement(this.chb_AutoLoad, "Options.ini");
                chargement.Chargement(this.chb_ModeDaltonien, "Options.ini");
                chargement.Chargement(this.chb_valid_expert, "Options.ini");
                //this.chb_send_auto.Enabled = this.chb_valid_expert.Checked;
            }
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
                    this.Etat_co.Etat_de_connection_actuel = -4;
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
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            Consigne.Change_Visibilité_Pid(((CheckBox)sender).Checked);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DGV_fpga = new DataGridView_pour_FPGA(6, 6,this.Li_Gb_spéciaux);
            this.panel_modulaire.Controls.Add(this.DGV_fpga);
            this.DGV_fpga.BringToFront();
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
 