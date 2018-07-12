using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace GeCoSwell
{
    public partial class Fenetre1 : Form
    {
        List<TextBox> list_val_tri = new List<TextBox>();
        List<ComboBox> list_sens_tri = new List<ComboBox>();
        List<ComboBox> list_cmb_cons = new List<ComboBox>();


        // textbox et checkbox qui contiennent les variables en option.
        TextBox tb_quartus_stpexe;
        TextBox tb_TCL;
        CheckBox chb_AutoLoad;
        CheckBox chb_ModeDaltonien;
        CheckBox chb_valid_expert;

        // declarer les objets du graphique mesure
        /*Graphique graphique = new Graphique("test", "axe X", "axe Y");
        PointPairList paquet = new PointPairList();
        Stopwatch timer = new Stopwatch();
        int nb_valeurs = 50;    // nombre de valeurs stockees
        bool actif = true;      // etat du bouton mesure
        bool en_cours = true;   // etat des mesures
        double temps = 0;*/

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
        public int Etat_co
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
                    Changement_Etat_Connect(this, EventArgs.Empty); // changer l'etat de la connexion
                }
            }
        }

        // enregistrement définissant une mesure
        struct Mesure
        {
            public string Nom;
            public CheckBox Chb;
            public TextBox Tb;
            public Color Couleur;

            public Mesure(string nom, CheckBox cb, TextBox tb, Color c)
            {
                this.Nom = nom;
                this.Chb = cb;
                this.Tb = tb;
                this.Couleur = c;
            }
        }

        List<Mesure> list_mesures = new List<Mesure>();

        public Fenetre1()
        {
            // initialiser tous les objets de la fenetre
            InitializeComponent();

            // ajouter les quatre mesures des CANs à la liste
            list_mesures.Add(new Mesure("can1", chb_mes_can1, tb_mes_can1, Color.DarkRed));
            list_mesures.Add(new Mesure("can2", chb_mes_can2, tb_mes_can2, Color.Blue));
            list_mesures.Add(new Mesure("can3", chb_mes_can3, tb_mes_can3, Color.GreenYellow));
            list_mesures.Add(new Mesure("can4", chb_mes_can4, tb_mes_can4, Color.Magenta));

            // initialiser les courbes du graphique

            StateObject cartefpga = new StateObject();
            this.Closing += new CancelEventHandler(this.MainForm_Closing);

            // initialiser la valeur de l'etat de connexion
            Etat_co = -99;


            this.list_val_tri.Add(this.tb_tri1);
            this.list_val_tri.Add(this.tb_tri2);
            this.list_sens_tri.Add(this.cmb_tri1);
            this.list_sens_tri.Add(this.cmb_tri2);

            // initialisation de la liste list_cmb_cons
            this.list_cmb_cons.Add(this.cmb_cons_B1);
            this.list_cmb_cons.Add(this.cmb_cons_B2);
            this.list_cmb_cons.Add(this.cmb_cons_B3);
            this.list_cmb_cons.Add(this.cmb_cons_B4);

            // initialiser les 4 courbes du graphique mesure

            // gerer le chargement des valeurs dans le .ini
            // les objets sont générés mais n'existent pas vraiment
            this.tb_quartus_stpexe = new TextBox() { Name = "tb_quartus_stpexe" };                      // création d'une textbox pour contenir l'info de l'option
            this.tb_TCL = new TextBox() { Name = "tb_TCL" };                                            // création d'une textbox pour contenir l'info de l'option
            this.chb_AutoLoad = new CheckBox() { Name = "ch_AutoLoad", Checked = false };               // création d'une checkbox pour contenir l'info de l'option
            this.chb_ModeDaltonien = new CheckBox() { Name = "chb_ModeDaltonien", Checked = false };    // création d'une textbox pour contenir l'info de l'option
            this.chb_valid_expert = new CheckBox() { Name = "chb_valid_expert", Checked = false };      // création d'une textbox pour contenir l'info de l'option
            GestionFichier.Chargement(tb_quartus_stpexe, "Options.ini");                                // si la checkbox est true alors on charge le fichier autosave
            if (tb_quartus_stpexe.Text != "")
            {
                GestionFichier.Chargement(this.tb_TCL, "Options.ini");
                GestionFichier.Chargement(this.chb_AutoLoad, "Options.ini");
                GestionFichier.Chargement(this.chb_ModeDaltonien, "Options.ini");
                GestionFichier.Chargement(this.chb_valid_expert, "Options.ini");
                this.chb_send_auto.Enabled = this.chb_valid_expert.Checked;

                if (this.chb_AutoLoad.Checked)
                {
                    GestionFichier.Chargement(this.tp_manuel, "Autosave.save");
                }
            }

        
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip = new ToolTip()
            {
                // Set up the delays for the ToolTip.
                InitialDelay = 100,
                ReshowDelay = 100,
                AutoPopDelay = 5000,

                //Forme de balon
                IsBalloon = true
            };

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip.SetToolTip(this.tb_db_T_Pulse1, "(min 0.1 ; max 102.3 ; par pas de 0.1µS)");
            toolTip.SetToolTip(this.tb_db_T_Pulse2, "(min 0.1 ; max 102.3 ; par pas de 0.1µS)");
            toolTip.SetToolTip(this.tb_db_T_Pulse3, "(min 0.1 ; max 102.3 ; par pas de 0.1µS)");
            toolTip.SetToolTip(this.tb_tpsmort, "(min 5 ; max 5115; par pas de 5nS");
            toolTip.SetToolTip(this.tb_consigne1, "min : consigne min ; max : consigne max ; par pas de 1");
            toolTip.SetToolTip(this.tb_consigne2, "min : consigne min ; max : consigne max ; par pas de 1");
            toolTip.SetToolTip(this.tb_con_max1, "min : consigne min ; max 1023 ; par pas de 1");
            toolTip.SetToolTip(this.tb_con_max2, "min : consigne min ; max 1023 ; par pas de 1");
            toolTip.SetToolTip(this.tb_con_min1, "min 0 ; max : consigne max ; par pas de 1");
            toolTip.SetToolTip(this.tb_con_min2, "min 0 ; max : consigne max ; par pas de 1");
            toolTip.SetToolTip(this.tb_con_pour1, "min : pourcentage min ; max : pourcentage max ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_con_pour2, "min : pourcentage min ; max : pourcentage max ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_con_max_pour1, "min : % min ; max 100 ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_con_max_pour2, "min : % min ; max 100 ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_con_min_pour1, "min 0 ; max % max ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_con_min_pour2, "min 0 ; max % max ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_tri1, "min 0 ; max 1023 ; par pas de 1");
            toolTip.SetToolTip(this.tb_tri2, "min 0 ; max 1023 ; par pas de 1");
            toolTip.SetToolTip(this.tb_tri3, "min 0 ; max 1023 ; par pas de 1");
            toolTip.SetToolTip(this.tb_tri4, "min 0 ; max 1023 ; par pas de 1");
            toolTip.SetToolTip(this.tb_tri1_pour, "min 0 ; max 100 ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_tri2_pour, "min 0 ; max 100 ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_tri3_pour, "min 0 ; max 100 ; par pas de 0.1");
            toolTip.SetToolTip(this.tb_tri4_pour, "min 0 ; max 100 ; par pas de 0.1");
            //-------------
            // CAN
            //-------------
            toolTip.SetToolTip(this.tb_can_etal_mes_max, "min " + tb_can_etal_mes_min.Text + " ; par pas de 1");
            toolTip.SetToolTip(this.tb_can_etal_mes_min, "max " + tb_can_etal_mes_max.Text + " ; par pas de 1");
            toolTip.SetToolTip(this.tb_can_etal_rep_max, "max 1023 ; min " + tb_can_etal_rep_min.Text + " ; par pas de 1");
            toolTip.SetToolTip(this.tb_can_etal_rep_min, "max " + tb_can_etal_rep_max.Text + " ; min 0 ; par pas de 1");
            toolTip.SetToolTip(this.tb_canreel_max, "max " + tb_can_etal_mes_max.Text + " ; min " + tb_canreel_min.Text + " ; par pas de 1");
            toolTip.SetToolTip(this.tb_canreel_min, "max " + tb_canreel_max.Text + " ; min " + tb_can_etal_rep_min.Text + " ; par pas de 1");
            toolTip.SetToolTip(this.tb_can10b_max, "max " + tb_can_etal_rep_max.Text + " ; min " + tb_can10b_min.Text + " ; par pas de 1");
            toolTip.SetToolTip(this.tb_can10b_min, "max " + tb_can10b_max.Text + " ; min " + tb_can_etal_rep_min.Text + " ; par pas de 1");

            // initialiser la liste des adresses FPGA et leurs correspondances
            GCfpga.Init_adress_nom();
        }

        
        // gerer les boutons et labels à chaque changement d'état de connexion 
        // Etat_co < 0 : serveur down
        // 0 < Etat_co < 10 : serveur on mais non connecté
        // 10 < Etat_co < 20 : serveur connecté sans transfert 
        // 20 < Etat_co : trandfert de données en cours
        public void Changement_Etat_Connect(object sender, EventArgs e)
        {   
            // initialement tout est désactivé
            Gestion_bt_serveur(false);          // mettre tous les boutons qui gérent le serveur en disabled
            Gestion_bt_co_serveur(false);       // mettre tous les boutons qui gérent la connexion au serveur en disabled
            Gestion_bt_transfert_data(false);   // mettre tous les boutons qui gérent le transfert de data en disabled

            if (10 < Etat_co && Etat_co < 20)
            {
                // activer les boutons de gestion de transfert
                Gestion_bt_transfert_data(true);
            }
            else
            {
                if (Etat_co < 10 && Etat_co >= 0)
                {
                    // activer les boutons de gestion du serveur
                    Gestion_bt_co_serveur(true);
                }
                else
                {
                    if (Etat_co < 0)
                    {
                        // activer les boutons de gestion du serveur
                        Gestion_bt_serveur(true);
                    }
                }

            }

            l_info_connection.ForeColor = Color.Black; // couleur par défaut des infos sur la connexion

            // afficher des indications de fonctionnement de l'application et/ou
            // l'état de fonctionnement des composants de l'application
            switch (Etat_co)
            {
                case -4:
                    this.l_info_connection.Text = "Les options sont correctes, vous pouvez lancer le serveur.";
                    break;
                case -3:
                    this.l_info_connection.Text = "Le serveur s'est fermé, veuillez le relancer.";
                    break;
                case -2:
                    this.l_info_connection.Text = "Options à mettre à jour, cliquer sur outil puis option.";
                    Gestion_bt_serveur(false);
                    break;
                case 0:
                    this.l_info_connection.Text = "Serveur lancé, attendez que la ligne :\n \"Started Socket Server on port - 2540\" apparaissent avant de cliquer sur connexion.";
                    break;
                case 1:
                    this.l_info_connection.Text = "Déconnexion réussie.";
                    break;
                case 9:
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
        private void Gestion_bt_serveur(bool etat)
        {
            // modifier l'état du bouton pour lancer le serveur
            this.b_lancer_serveur.Enabled = etat;
            this.tsm_serveur.Enabled = etat;
        }


        // Attribuer un état au bouton qui gèrent la connexion au serveur
        // Paramètre :
        //      bool etat a affecé aux boutons
        private void Gestion_bt_co_serveur(bool etat)
        {
            //bouton de connection au serveur
            this.b_Connection.Enabled = etat;
            this.tsm_connect.Enabled = etat;
        }


        // Attribuer un nouvel état aux boutons qui gèrent l'envoi
        // ou la réception de la donnée data
        // Paramètre :
        //      bool etat a affecté aux boutons
        private void Gestion_bt_transfert_data(bool etat)
        {
            //bouton de transfert de data
            this.b_Envoi.Enabled = etat;
            this.b_Reception.Enabled = etat;
            this.tsm_send.Enabled = etat;
            this.tsm_reception.Enabled = etat;
            this.b_mesure_can.Enabled = etat;
            this.tsm_deconnecte.Enabled = etat;
            this.chb_send_auto.Enabled = etat;
            this.b_aff_val_fpga.Enabled = etat;
        }

        
        // Demander la connexion au serveur et gérer les boutons en 
        // fonction de la réponse du serveur
        private void B_Communication_Click(object sender, EventArgs e)
        {
            Etat_co = 10; // état de connexion en cours

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
            ((BackgroundWorker)sender).ReportProgress(0,AsynchronousClient.StartClient());
            
        }


        private void Bgw_co_serv_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string test = e.UserState.ToString();
            if (test == "co_ok")
            {
                Etat_co = 11; // connexion en cours
            }
            else
            {
                Etat_co = 9; // connection échouée
            }
            //voir si j'en fait quelque chose
        }


        private void Bgw_co_serv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //voir si je fait quelque chose quand le code est fini
        }


        // Utiliser un backgroundworker pour demander les mesures des CANs
        private void Lancer_mesure()
        {
            if (Etat_co >= 11 && Etat_co < 20)
            {// connecté au serveur mais sans transfert en cour
                Etat_co = 22;

                // lancer la demande les mesures
                BackgroundWorker bgw_mesure = new BackgroundWorker(){
                    WorkerReportsProgress = true,
                };
                //timer.Restart();  // on déclenche le timer
                bgw_mesure.WorkerReportsProgress = true;
                bgw_mesure.DoWork += new DoWorkEventHandler(Bgw_mesure_DoWork);
                bgw_mesure.ProgressChanged += new ProgressChangedEventHandler(Bgw_mesure_ProgressChanged);
                bgw_mesure.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bgw_mesure_RunWorkerCompleted);
                bgw_mesure.RunWorkerAsync();
                //------------------------------------------------------------------------
            }
        }


        // Demander au FPGA la dernière valeur mesurée pour chaque CAN validé
        // Ne pas oublié quand on envoi X on ne reçoit la réponse qu'à l'envoie suivant /!\
        private void Bgw_mesure_DoWork(object sender, DoWorkEventArgs e)
        {
            string receive;     // valeur reçue
            int i = 0;          // numéro de l'expéditeur de la dernière valeur envoyée
                                // Initialiser à 0 pour ne pas prendre en compte la première valeur
            
            while (true)
            {
                if (this.chb_mes_can1.Checked)
                {
                    receive = GCfpga.Mesure_CAN(this.tb_val_20);            // envoyer la requête pour avoir la donnée du CAN 1
                    ((BackgroundWorker)sender).ReportProgress(i, receive);  // envoyer la dernière valeur et le numéro de l'expéditeur
                    i = 1;                                                  // stocker le numéro de l'expéditeur de la dernière requête
                }
                if (this.chb_mes_can2.Checked)
                {
                    receive = GCfpga.Mesure_CAN(this.tb_val_21);            // envoyer la requête pour avoir la donnée du CAN 2
                    ((BackgroundWorker)sender).ReportProgress(i, receive);  // envoyer la dernière valeur et le numéro de l'expéditeur
                    i = 2;                                                  // stocker le numéro de l'expéditeur de la dernière requête
                }
                if (this.chb_mes_can3.Checked)
                {
                    receive = GCfpga.Mesure_CAN(this.tb_val_22);            // envoyer la requête pour avoir la donnée du CAN 3
                    ((BackgroundWorker)sender).ReportProgress(i, receive);  // envoyer la dernière valeur et le numéro de l'expéditeur
                    i = 3;                                                  // stocker le numéro de l'expéditeur de la dernière requête
                }
                if (this.chb_mes_can4.Checked)
                {
                    receive = GCfpga.Mesure_CAN(this.tb_val_23);            // envoyer la requête pour avoir la donnée du CAN 4
                    ((BackgroundWorker)sender).ReportProgress(i, receive);  // envoyer la dernière valeur et le numéro de l'expéditeur
                    i = 4;                                                  // stocker le numéro de l'expéditeur de la dernière requête
                }

                // si la case mesure en continue n'est pas coché, on envoie
                // une dernière valeur pour récupérer la dernière mesure
                if (!this.chb_continue_can.Checked) 
                {
                    receive = GCfpga.Mesure_CAN(this.tb_val_0);              // envoyer une requête quelquonque pour récupérer la dernière donnée
                    ((BackgroundWorker)sender).ReportProgress(i, receive);      // envoyer la dernière valeur et le numéro de l'expéditeur
                    return;                                                     // finir la boucle
                }
                
            }
        }


        // Recevoir les informations de l'évolution du BGworker et les traiter
        private void Bgw_mesure_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString() == "-1")
            {
                this.chb_continue_can.Checked = false;
                Etat_co = 15;
            }

            // envoyer la bonne valeur à la bonne txbox
            switch (e.ProgressPercentage)
            {

                case 1:
                    this.tb_val_20.Text = e.UserState.ToString();
                    break;
                case 2:
                    this.tb_val_21.Text = e.UserState.ToString();
                    break;
                case 3:
                    this.tb_val_22.Text = e.UserState.ToString();
                    break;
                case 4:
                    this.tb_val_23.Text = e.UserState.ToString();
                    break;
            }
        }


        // Affecter la valeur de l'état de fin de mesure à l'état de connexion
        // Exécuter cette méthode quand la mesure est finie
        private void Bgw_mesure_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Etat_co = 13; // état de la mesure finie
            //timer.Stop();   // on arrête le timer
        }


        // Envoyer un signal à la carte FPGA
        private void B_Envoi_Click(object sender, EventArgs e)
        {
            Etat_co = 20;// état d'envoi en cours
            string send_ok = "";

            if (this.tc_mode_commande.SelectedIndex == 0)
            {// on est en train d'afficher le double pulse

                send_ok = GCfpga.Send_info(this.tp_manuel, "ALL");  // envoyer ce qui est commun à tous

                if (send_ok != "")
                {// une erreur de transfert est survenue
                    //gestion_etat_co.erreur = send_ok;
                    erreur = send_ok;
                    Etat_co = 14;
                    return;                                         // arrêter le transfert
                }

                send_ok = GCfpga.Send_info(this.tp_manuel, "DP");   // envoyer le double pulse
                Test_et_Conv_des_tb(tb_val_12, new CancelEventArgs()); // envoyer le nombre de bras actif a tb_val_12
            }
            else if (this.tc_mode_commande.SelectedIndex == 1)
            {
                GValeurForm.String_replace(tb_val_12, "0000", 0); // 4 premiers bits de tb_val_12 à 0

                List<String> type_connexion = new List<string>() { "ALL", "BO" };
                if (this.cmb_mode_puissance.SelectedIndex == 1 || this.cmb_mode_puissance.SelectedIndex == 3 || this.cmb_mode_puissance.SelectedIndex == 4) // mode boucle fermé ou tout ou triphasé
                    type_connexion.Add("BF"); // alors on envoie Boucle fermé
                if (this.cmb_consigne1.SelectedIndex == 1) // mode boucle fermé ou tout
                    type_connexion.Add("SF1"); // alors on envoie Boucle fermé
                if (this.cmb_consigne2.SelectedIndex == 1) // mode boucle fermé ou tout
                    type_connexion.Add("SF2"); // alors on envoie Boucle fermé
                if (this.cmb_consigne3.SelectedIndex == 1) // mode boucle fermé ou tout
                    type_connexion.Add("SF3"); // alors on envoie Boucle fermé
                if (this.cmb_mode_puissance.SelectedIndex == 3 || this.cmb_mode_puissance.SelectedIndex == 4) // mode triphasé ou tout
                    type_connexion.Add("TRI"); // alors on envoie Boucle fermé
                                               //cas multiniv
                if (this.chb_mode_multiniv.Checked) // mode boucle fermé ou tout
                    type_connexion.Add("SP"); // alors on envoie superposition
                if (this.cmb_mode_puissance.SelectedIndex == 2 || this.cmb_mode_puissance.SelectedIndex == 3) // mode opposition ou tout
                    type_connexion.Add("OP"); // envoyer opposition

                for (int i = 0; i < type_connexion.Count; i++)
                {
                    send_ok = GCfpga.Send_info(this.tp_manuel, type_connexion[i]);  // envoyer ce qui est commun à tous
                    if (send_ok != "")
                    {// une erreur de transfert est survenue
                        //gestion_etat_co.erreur = send_ok;
                        erreur = send_ok;
                        Etat_co = 14;
                        return;                                         // arrêter le transfert
                    }
                }
            }
                

            // INDIQUER QUE LA DONNEE A ETE ENVOYEE
            //b_Envoi.Image = GeCoSwell.Properties.Resources.icons8_send_16; 
            Etat_co = 12; // état de l'envoi terminé
        }

        

        // Récupérer les informations de la carte quand on clique
        // sur le bouton réception de l'interface
        private void B_Reception(object sender, EventArgs e)
        {
            Etat_co = 21;//réception en cour
            string send_ok = GCfpga.Reception(this.tp_manuel);

            if (send_ok == "-1")//si erreur de transfert
            {
                Etat_co = 15;
                return;//dans ce cas la on arrête le transfert
            }

            Valid_Select();//fonction pour mettre à jour les %
            Etat_co = 13;//réception terminé
        }


        // Ouvrir le fichier log
        private void B_open_log_Click(object sender, EventArgs e)
        {
            GestionFichier.Log_Open();
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
            if (chb_AutoLoad.Checked)
                GestionFichier.Sauvegarde(this.tp_manuel, "Autosave.save");
        }


        // Initialiser tous les composants de la fenêtre de l'application
        private void Load_fenetre(object sender, EventArgs e)
        {
            Lancer_serveur();
            
            foreach (TabPage tp in this.tc_mode_commande.TabPages)  // initialier tous les controls de chaque page
            {
                tp.Show();
            }
            foreach (Control ct in GValeurForm.Crealist(this))      // initialiser tous les controls de chaque page
            {
                bool temp = ct.Visible;
                ct.Show();
                ct.Select();
                if (ct.GetType() != typeof(TextBox))
                    ct.Visible = temp;
            }

            // Initialiser à la première valeur tous les combobox
            List<Control> cbox = new List<Control>();
            GValeurForm.FindAllControlForOneType(this, typeof(ComboBox), cbox);

            foreach (ComboBox cb in cbox)
            {
                cb.SelectedIndex = 0;
            }

            this.tc_mode_commande.SelectedIndex = 1;
            this.tc_mode_commande.SelectedIndex = 0;

            //Decale les éléments des groupesbox bras pour l'affichage
            this.Initialiser_chb_inv_bras(false);
            this.Initialiser_chb_multi_bras(false);
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
            Etat_co = 0; // serveur se lance

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
            Etat_co = e.ProgressPercentage;  // Envoiyer l'état de la connexion
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
            Etat_co = 1;
        }
                

        // Vérifier les valeurs des cases modifiées et les corriger en cas d'erreur de saisie
        private void Test_et_Conv_des_tb(object sender, CancelEventArgs e)
        {

            if (sender == this.tb_db_T_Pulse1 ||sender == this.tb_db_T_Pulse2 || sender == this.tb_db_T_Pulse3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0.09f, 102.3f, 0.1, 1, "10");
            }
            else if (sender == this.tb_bras_DP)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 1, 4, 1, 0, "1") ;
                // GCfpga.Conv_1023_vers_10bits est passée de privée à publique
                GValeurForm.String_replace(tb_val_12, GCfpga.Conv_1023_vers_10bits(tb_bras_DP.Text, 1, 1d), 0);
            }
            else if (sender == this.tb_tpsmort)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 5115, 5, 1, "100");
            }
            else if (sender == this.tb_consigne1)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 1023, 1, 0, "512");
                this.tb_con_pour1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_consigne2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 1023, 1, 0, "512");
                this.tb_con_pour2.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_consigne3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 1023, 1, 0, "512");
                this.tb_con_pour3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_con_min1)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(this.tb_con_max1.Text), 1, 0, "51");
                this.tb_con_min_pour1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_con_min2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(this.tb_con_max2.Text), 1, 0, "51");
                this.tb_con_min_pour2.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_con_min3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(this.tb_con_max3.Text), 1, 0, "51");
                this.tb_con_min_pour3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_con_max1)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(this.tb_con_min1.Text), 1023, 1, 0, "972");
                this.tb_con_max_pour1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_con_max2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(this.tb_con_min2.Text), 1023, 1, 0, "972");
                this.tb_con_max_pour2.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }
            else if (sender == this.tb_con_max3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(this.tb_con_min3.Text), 1023, 1, 0, "972");
                this.tb_con_max_pour3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 1, false);
            }

            else if (sender == this.tb_con_pour1)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_consigne1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_pour2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_consigne2.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_pour3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_consigne3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_min_pour1)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(this.tb_con_max_pour1.Text), 0.1, 1, "5");
                this.tb_con_min1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_min_pour2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(this.tb_con_max_pour2.Text), 0.1, 1, "5");
                this.tb_con_min2.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_min_pour3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(this.tb_con_max_pour3.Text), 0.1, 1, "5");
                this.tb_con_min3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_max_pour1)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(this.tb_con_min_pour1.Text), 100, 0.1, 1, "95");
                this.tb_con_max1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_max_pour2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(this.tb_con_min_pour2.Text), 100, 0.1, 1, "95");
                this.tb_con_max1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }
            else if (sender == this.tb_con_max_pour3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(this.tb_con_min_pour3.Text), 100, 0.1, 1, "95");
                this.tb_con_max3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, 1023, 0, true);
            }

            else if (sender == this.tb_tri1)
            {//max = valeur max des triangle, en cas d'erreur on envoie valeur max /2)
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, double.Parse(this.tb_freq_countmax.Text), 1, 0, (Math.Ceiling(double.Parse(tb_freq_countmax.Text)/ 2)).ToString());
                this.tb_tri1_pour.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 1, false);
                this.Test_general_proxi_change_sens();
            }
            else if (sender == this.tb_tri2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, double.Parse(this.tb_freq_countmax.Text), 1, 0, (Math.Ceiling(double.Parse(tb_freq_countmax.Text) / 2)).ToString());
                this.tb_tri2_pour.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 1, false);
                this.Test_general_proxi_change_sens();
            }
            else if (sender == this.tb_tri3)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, double.Parse(this.tb_freq_countmax.Text), 1, 0, (Math.Ceiling(double.Parse(tb_freq_countmax.Text) / 2)).ToString());
                this.tb_tri3_pour.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 1, false);
                this.Test_general_proxi_change_sens();
            }
            else if (sender == this.tb_tri4)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, double.Parse(this.tb_freq_countmax.Text), 1, 0, (Math.Ceiling(double.Parse(tb_freq_countmax.Text) / 2)).ToString());
                this.tb_tri4_pour.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 1, false);
                this.Test_general_proxi_change_sens();
            }


            else if (sender == this.tb_tri1_pour)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_tri1.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
            }
            else if (sender == this.tb_tri2_pour)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_tri2.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
            }
            else if (sender == this.tb_tri3_pour)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_tri3.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
            }
            else if (sender == this.tb_tri4_pour)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 100, 0.1, 1, "50");
                this.tb_tri4.Text = CalculFeuille.Conv_10bitpour(((TextBox)sender).Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
            }

            //--------------------------------
            //Can
            //--------------------------------

            else if (sender == this.tb_can_etal_mes_max)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_can_etal_mes_min.Text), 9999, 1, 0, "50");
            }
            else if (sender == this.tb_can_etal_mes_min)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, -9999, float.Parse(tb_can_etal_mes_max.Text), 1, 0, "-50");
            }
            else if (sender == this.tb_can_etal_rep_max)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_can_etal_rep_min.Text), 1024, 1, 0, "1023");
            }
            else if (sender == this.tb_can_etal_rep_min)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, float.Parse(tb_can_etal_rep_max.Text), 1, 0, "0");
            }
            else if (sender == this.tb_canreel_max)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_canreel_min.Text), float.Parse(tb_can_etal_mes_max.Text), 1, 0, "30");
            }
            else if (sender == this.tb_canreel_min)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_can_etal_mes_min.Text), float.Parse(tb_canreel_max.Text), 1, 0, "-30");
            }
            else if (sender == this.tb_can10b_max)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_can10b_min.Text), float.Parse(tb_can_etal_rep_max.Text), 1, 0, "1000");
            }
            else if (sender == this.tb_can10b_max)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_can10b_min.Text), float.Parse(tb_can_etal_rep_max.Text), 1, 0, "1000");
            }
            else if (sender == this.tb_can10b_min)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, float.Parse(tb_can_etal_rep_min.Text), float.Parse(tb_can10b_max.Text), 1, 0, "100");
            }
            //----------------------------------
            //Choix de la fréquence fdec
            //----------------------------------
            else if (sender == this.tb_freq_div)
            {
                
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 1, 512, 1, 0, "1");
                this.tb_fdec.Text = Calc_freq(this.tb_freq_div.Text, tb_freq_countmax.Text);
                this.tb_val_13.Text = (double.Parse(this.tb_freq_div.Text) - 2).ToString();
                if (this.tb_val_13.Text.Equals("-1")) this.tb_val_13.Text = "1023";
            }
            else if (sender == this.tb_freq_countmax)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 32, 1023, 1, 0, "1023");
                //this.tb_fdec.Text = Calc_freq(tb_freq_div.Text, tb_freq_countmax.Text);
                
                //mise à jour des nouveaux init triangle pour conservé le %
                this.tb_tri1.Text = CalculFeuille.Conv_10bitpour(this.tb_tri1_pour.Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
                this.tb_tri2.Text = CalculFeuille.Conv_10bitpour(this.tb_tri2_pour.Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
                this.tb_tri3.Text = CalculFeuille.Conv_10bitpour(this.tb_tri3_pour.Text, double.Parse(this.tb_freq_countmax.Text), 0, true);
                this.tb_tri4.Text = CalculFeuille.Conv_10bitpour(this.tb_tri4_pour.Text, double.Parse(this.tb_freq_countmax.Text), 0, true);

                if (this.chb_mode_multiniv.Checked)//si mode superposition check on peux affecter les cases intermédiaires
                {
                    // modifier la valeur brut des maxima des bras en conservant le même pourcentage
                    this.Bras_1_extremum_Changed(this.tb_multinivmax_P1, new EventArgs());
                    this.Bras_2_extremum_Changed(this.tb_multinivmax_P2, new EventArgs());
                    this.Bras_3_extremum_Changed(this.tb_multinivmax_P3, new EventArgs());
                    this.Bras_4_extremum_Changed(this.tb_multinivmax_P4, new EventArgs());
                }
                else
                {
                    this.tb_val_36.Text = this.tb_freq_countmax.Text;
                    this.tb_val_38.Text = this.tb_freq_countmax.Text;
                    this.tb_val_40.Text = this.tb_freq_countmax.Text;
                    this.tb_val_42.Text = this.tb_freq_countmax.Text;

                }

                this.tb_fdec.Text = Calc_freq(this.tb_freq_div.Text, tb_freq_countmax.Text);
            }
            
            else if (sender == this.tb_PID_cible1 || ((TextBox)sender) == this.tb_PID_cible2)
            {
                ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 1023, 1, 0, "512");
            }
            this.Link_tb_manuel(sender);
        }

        //----------------------------------------------------------------------
        //fonction appeler par les case de l'onglet manuel pour mettre à jour le reste
        //----------------------------------------------------------------------
        private void Val_manuel_Validating(object sender, CancelEventArgs e)
        {
            if (chb_send_auto.Checked)//si envoie auto activé
            {
                GCfpga.Send_auto(sender);
            }
            this.Link_tb_manuel(sender);
        }

        private void Maj_Inversion(object sender,CheckBox chb_bras, TextBox tb_val, int position)
        {
            if (tb_val == sender)
            {
                GValeurForm.Conv_chb_string(1, 0, chb_bras,  tb_val.Text.Substring(position,1));
            }

            if (chb_bras == sender)
            {
                GValeurForm.String_replace(tb_val, GValeurForm.Conv_chb_string(1, 1, chb_bras), position);
            }
            /*
            if (chb_bras.Checked == true)
            {
                // le checkbox est coché mais inv_switch n'est pas modifié
                GValeurForm.String_replace(tb_val, GValeurForm.Conv_chb_string(1, 1, chb_bras), position);
            }
            else
            {
                GValeurForm.String_replace(tb_val, GValeurForm.Conv_chb_string(1, 1, chb_bras), position);
            }
            
            if (String.Equals(tb_val.Text.Substring(position, 1), "1"))
            {
                // inv_switch est modifé mais pas le checkbox associé
                GValeurForm.Conv_chb_string(1, 0, chb_bras, "1");
            }
            else
            {
                GValeurForm.Conv_chb_string(1, 0, chb_bras, "0");
            }*/
        }


        //----------------------------------------------------------------------
        //Calcule des cases manuel ou maj le reste par rapport à manuel
        //----------------------------------------------------------------------
        private void Link_tb_manuel(object sender)
        {
            // mettre à jour la valeur des des bits de tb_val_12
            Maj_Inversion(sender, this.chb_inv_bras_1, this.tb_val_12, 5);
            Maj_Inversion(sender, this.chb_inv_bras_2, this.tb_val_12, 6);
            Maj_Inversion(sender, this.chb_inv_bras_3, this.tb_val_12, 7);
            Maj_Inversion(sender, this.chb_inv_bras_4, this.tb_val_12, 8);

            // INDIQUER QUE LA DONNEE A ETE MODIFIEE MAIS PAS ENVOYEE
            this.b_Envoi.Image = GeCoSwell.Properties.Resources.icons8_no_send_16;


            //------------------
            //vers case tb_val_5
            //------------------
            if (this.cmb_tri1 == sender)//si c'est la combobox x
            {//inscrire son état au bit de poids 9 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_cmb_string(1, 1, sender), 0);
            }
            else if (this.cmb_tri2 == sender)
            {//inscrire son état au bit de poids 8 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_cmb_string(1, 1, sender), 1);
            }
            else if (this.cmb_tri3 == sender)
            {//inscrire son état au bit de poids 7 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_cmb_string(1, 1, sender), 2);
            }
            else if (this.cmb_tri4 == sender)
            {//inscrire son état au bit de poids 6 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_cmb_string(1, 1, sender), 3);
            }
            else if (this.chb_ActiveB1 == sender)//si la checkbox est l'objet send
            {//inscrire son état au bit de poids 5 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_chb_string(1, 1, sender), 4);
            }
            else if (this.chb_ActiveB2 == sender)//si la checkbox est l'objet send
            {//inscrire son état au bit de poids 4 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_chb_string(1, 1, sender), 5);
            }
            else if (this.chb_ActiveB3 == sender)//si la checkbox est l'objet send
            {//inscrire son état au bit de poids 3 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_chb_string(1, 1, sender), 6);
            }
            else if (this.chb_ActiveB4 == sender)//si la checkbox est l'objet send
            {//inscrire son état au bit de poids 2 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_chb_string(1, 1, sender), 7);
            }/*
            else if (this.tb_niv_actif == sender)//si la textbox est l'objet send
            {//inscrire sa valeur au bit de poids 1 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, this.tb_niv_actif.Text, 8);
            }*/
            else if (this.chb_Rouecodeuse == sender)//si la checkbox est l'objet send
            {//inscrire son état au bit de poids 2 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_5, GValeurForm.Conv_chb_string(1, 1, sender), 9);
            }
            //------------------
            //retour case tb_val_5
            //------------------
            else if (this.tb_val_5 == sender)
            {
                //Sens A..B..C..D
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_tri1, this.tb_val_5.Text.Substring(0, 1));
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_tri2, this.tb_val_5.Text.Substring(1, 1));
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_tri3, this.tb_val_5.Text.Substring(2, 1));
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_tri4, this.tb_val_5.Text.Substring(3, 1));

                //Actif A..B..C..D
                GValeurForm.Conv_chb_string(1, 0, this.chb_ActiveB1, this.tb_val_5.Text.Substring(4, 1));
                GValeurForm.Conv_chb_string(1, 0, this.chb_ActiveB2, this.tb_val_5.Text.Substring(5, 1));
                GValeurForm.Conv_chb_string(1, 0, this.chb_ActiveB3, this.tb_val_5.Text.Substring(6, 1));
                GValeurForm.Conv_chb_string(1, 0, this.chb_ActiveB4, this.tb_val_5.Text.Substring(7, 1));
                //niv actif
                //this.tb_niv_actif.Text = this.tb_val_5.Text.Substring(8, 1);
                //utilisation de la roue codeuse
                GValeurForm.Conv_chb_string(1, 0, this.chb_Rouecodeuse, this.tb_val_5.Text.Substring(9, 1));
            }
            //------------------
            //case tb_val_8 tps mort
            //------------------
            else if (this.tb_tpsmort == sender)
            {
                this.tb_val_8.Text = (int.Parse(this.tb_tpsmort.Text) / 5).ToString();
            }
            else if (this.tb_val_8 == sender)
            {
                this.tb_tpsmort.Text = (int.Parse(this.tb_val_8.Text) * 5).ToString();
            }
            //------------------
            //case tb_val_9 tps pulse 1
            //------------------
            else if (this.tb_db_T_Pulse1 == sender)
            {
                this.tb_val_9.Text = (double.Parse(this.tb_db_T_Pulse1.Text) * 10).ToString();
            }
            else if (this.tb_val_9 == sender)
            {
                this.tb_db_T_Pulse1.Text = (double.Parse(this.tb_val_9.Text) / 10).ToString();
            }
            //------------------
            //case tb_val_10 tps pulse 2
            //------------------
            else if (this.tb_db_T_Pulse2 == sender)
            {
                this.tb_val_10.Text = (double.Parse(this.tb_db_T_Pulse2.Text) * 10).ToString();
            }
            else if (this.tb_val_10 == sender)
            {
                this.tb_db_T_Pulse2.Text = (double.Parse(this.tb_val_10.Text) / 10).ToString();
            }
            //------------------
            //case tb_val_11 tps pulse 3
            //------------------
            else if (this.tb_db_T_Pulse3 == sender)
            {
                this.tb_val_11.Text = (double.Parse(this.tb_db_T_Pulse3.Text) * 10).ToString();
            }
            else if (this.tb_val_11 == sender)
            {
                this.tb_db_T_Pulse3.Text = (double.Parse(this.tb_val_11.Text) / 10).ToString();
            }


            //------------------
            //vers case tb_val_12 quelle bras pour DP et quelle transistor
            //------------------
            else if (this.tb_bras_DP == sender)
            {
                switch(this.tb_bras_DP.Text)
                {//mise en forme de la textbox en signal compréhensible par la carte FPGA
                    case "1":
                        GValeurForm.String_replace(this.tb_val_12, "1000", 0);
                        break;
                    case "2":
                        GValeurForm.String_replace(this.tb_val_12, "0100", 0);
                        break;
                    case "3":
                        GValeurForm.String_replace(this.tb_val_12, "0010", 0);
                        break;
                    case "4":
                        GValeurForm.String_replace(this.tb_val_12, "0001", 0);
                        break;
                }


            }

            else if (this.cmb_Choix_DP_Transistor == sender)//si la checkbox est l'objet send
            {//inscrire son état au bit de poids 2 dans tb_val_5
                GValeurForm.String_replace(this.tb_val_12, GValeurForm.Conv_cmb_string(1, 1, sender), 4);
            }
            //------------------
            //retour case tb_val_12 quelle bras pour DP et quelle transistor
            //------------------
            else if (this.tb_val_12 == sender)
            {
                switch (this.tb_val_12.Text.Substring(0,4))
                {//mise en forme de la textbox en signal compréhensible par la carte FPGA
                    case "1000":
                        tb_bras_DP.Text = "1";
                        break;
                    case "0100":
                        tb_bras_DP.Text = "2";
                        break;
                    case "0010":
                        tb_bras_DP.Text = "3";
                        break;
                    case "0001":
                        tb_bras_DP.Text = "4";
                        break;
                }
                //choix du transistor
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_Choix_DP_Transistor, this.tb_val_12.Text.Substring(4, 1));

                Maj_Inversion(sender, this.chb_inv_bras_1, this.tb_val_12, 5);
                Maj_Inversion(sender, this.chb_inv_bras_2, this.tb_val_12, 6);
                Maj_Inversion(sender, this.chb_inv_bras_3, this.tb_val_12, 7);
                Maj_Inversion(sender, this.chb_inv_bras_4, this.tb_val_12, 8);
            }
            else if (sender == tb_val_13)
            {
                int freq = int.Parse(this.tb_val_13.Text) + 2;
                if (freq == 1025) freq = 1;
                this.tb_freq_div.Text = freq.ToString();
            }





            //------------------
            //vers case tb_val_18 choix des modes de consigne et qui détecte l'erreur
            //------------------
            else if (this.cmb_consigne1 == sender || cmb_consigne_CAN1 == sender)
            {
                switch (this.cmb_consigne1.SelectedIndex)
                {//mise en forme de la textbox en signal compréhensible par la carte FPGA
                    case 0:
                        GValeurForm.String_replace(this.tb_val_18, "100", 6);
                        break;
                    case 1:
                        if (chb_surmodulation.Checked)//si mode surmodulation activé
                        {
                            GValeurForm.String_replace(this.tb_val_18, "110", 6);
                        }
                        else
                        {
                            GValeurForm.String_replace(this.tb_val_18, "101", 6);
                        }
                        break;
                    case 2:
                        GValeurForm.String_replace(this.tb_val_18, "0", 6);
                        GValeurForm.String_replace(this.tb_val_18, GValeurForm.Conv_cmb_string(2, 1, this.cmb_consigne_CAN1), 7);
                        break;
                }
            }
            else if (this.cmb_consigne2 == sender || cmb_consigne_CAN2 == sender)
            {
                switch (this.cmb_consigne2.SelectedIndex)
                {//mise en forme de la textbox en signal compréhensible par la carte FPGA
                    case 0:
                        GValeurForm.String_replace(this.tb_val_18, "100", 3);
                        break;
                    case 1:
                        if (chb_surmodulation.Checked)//si mode surmodulation activé
                        {
                            GValeurForm.String_replace(this.tb_val_18, "110", 3);
                        }
                        else
                        {
                            GValeurForm.String_replace(this.tb_val_18, "101", 3);
                        }
                        break;
                    case 2:
                        GValeurForm.String_replace(this.tb_val_18, "0", 3);
                        GValeurForm.String_replace(this.tb_val_18, GValeurForm.Conv_cmb_string(2, 1, this.cmb_consigne_CAN2), 4);
                        break;
                }
            }
            else if (this.cmb_consigne3 == sender || cmb_consigne_CAN3 == sender)
            {
                switch (this.cmb_consigne2.SelectedIndex)
                {//mise en forme de la textbox en signal compréhensible par la carte FPGA
                    case 0:
                        GValeurForm.String_replace(this.tb_val_18, "100", 0);
                        break;
                    case 1:
                        if (chb_surmodulation.Checked)//si mode surmodulation activé
                        {
                            GValeurForm.String_replace(this.tb_val_18, "110", 0);
                        }
                        else
                        {
                            GValeurForm.String_replace(this.tb_val_18, "101", 0);
                        }
                        break;
                    case 2:
                        GValeurForm.String_replace(this.tb_val_18, "0", 0);
                        GValeurForm.String_replace(this.tb_val_18, GValeurForm.Conv_cmb_string(2, 1, this.cmb_consigne_CAN2), 1);
                        break;
                }
            }
            else if (this.chB_CAN1 == sender)//si la checkbox est l'objet send vérif si on utilise le can pour détecter une erreur
            {
                GValeurForm.String_replace(this.tb_val_19, GValeurForm.Conv_chb_string(1, 1, sender), 4);
            }
            else if (this.chB_CAN2 == sender)//si la checkbox est l'objet send vérif si on utilise le can pour détecter une erreur
            {
                GValeurForm.String_replace(this.tb_val_19, GValeurForm.Conv_chb_string(1, 1, sender), 5);
            }
            else if (this.chB_CAN3 == sender)//si la checkbox est l'objet send vérif si on utilise le can pour détecter une erreur
            {
                GValeurForm.String_replace(this.tb_val_19, GValeurForm.Conv_chb_string(1, 1, sender), 6);
            }
            else if (this.chB_CAN4 == sender)//si la checkbox est l'objet send vérif si on utilise le can pour détecter une erreur
            {
                GValeurForm.String_replace(this.tb_val_19, GValeurForm.Conv_chb_string(1, 1, sender), 7);
            }
            //------------------
            //retour case tb_val_18 choix des modes de consigne et qui détecte l'erreur
            //------------------
            else if (this.tb_val_18 == sender)
            {
                switch (this.tb_val_18.Text.Substring(0, 3))
                {
                    case "100":
                        this.cmb_consigne1.SelectedIndex = 0;
                        break;
                    case "101":
                        this.cmb_consigne1.SelectedIndex = 1;
                        break;
                    case "110":
                        this.cmb_consigne1.SelectedIndex = 1;
                        this.chb_surmodulation.Checked = true;
                        break;
                    default://autre cas
                        this.cmb_consigne1.SelectedIndex = 2;
                        GValeurForm.Conv_cmb_string(2, 0, this.cmb_consigne_CAN1, this.tb_val_18.Text.Substring(1, 2));
                        break;

                }
                switch (this.tb_val_18.Text.Substring(3, 3))
                {
                    case "100":
                        this.cmb_consigne2.SelectedIndex = 0;
                        break;
                    case "101":
                        this.cmb_consigne2.SelectedIndex = 1;
                        break;
                    default://autre cas
                        this.cmb_consigne2.SelectedIndex = 2;
                        GValeurForm.Conv_cmb_string(2, 0, this.cmb_consigne_CAN2, this.tb_val_18.Text.Substring(4, 2));
                        break;
                }
            }

            //------------------
            //retour case tb_val_18 choix des modes de consigne et qui détecte l'erreur
            //------------------
            else if (this.tb_val_19 == sender)
            {
                GValeurForm.Conv_chb_string(1, 0, this.chB_CAN1, this.tb_val_18.Text.Substring(4, 1));
                GValeurForm.Conv_chb_string(1, 0, this.chB_CAN2, this.tb_val_18.Text.Substring(5, 1));
                GValeurForm.Conv_chb_string(1, 0, this.chB_CAN3, this.tb_val_18.Text.Substring(6, 1));
                GValeurForm.Conv_chb_string(1, 0, this.chB_CAN4, this.tb_val_18.Text.Substring(7, 1));
            }
            //------------------
            //vers case tb_val_33 choix de quelle consigne affecte les bras
            //------------------
            else if (this.cmb_cons_B4 == sender)//si c'est la combobox x
            {//inscrire son état au bit de poids 9 dans tb_val_33
                GValeurForm.String_replace(this.tb_val_33, GValeurForm.Conv_cmb_string(2, 1, sender), 0);
            }
            else if (this.cmb_cons_B3 == sender)//si c'est la combobox x
            {//inscrire son état au bit de poids 9 dans tb_val_33
                GValeurForm.String_replace(this.tb_val_33, GValeurForm.Conv_cmb_string(2, 1, sender), 2);
            }
            else if (this.cmb_cons_B2 == sender)//si c'est la combobox x
            {//inscrire son état au bit de poids 9 dans tb_val_33
                GValeurForm.String_replace(this.tb_val_33, GValeurForm.Conv_cmb_string(2, 1, sender), 4);
            }
            else if (this.cmb_cons_B1 == sender)//si c'est la combobox x
            {//inscrire son état au bit de poids 9 dans tb_val_33
                GValeurForm.String_replace(this.tb_val_33, GValeurForm.Conv_cmb_string(2, 1, sender), 6);
            }
            //------------------
            //retour case tb_val_33 choix de quelle consigne affecte les bras
            //------------------
            else if (this.tb_val_33 == sender)
            {
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_cons_B4, this.tb_val_33.Text.Substring(0, 1));
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_cons_B3, this.tb_val_33.Text.Substring(1, 1));
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_cons_B2, this.tb_val_33.Text.Substring(2, 1));
                GValeurForm.Conv_cmb_string(1, 0, this.cmb_cons_B1, this.tb_val_33.Text.Substring(3, 1));
            }

            this.Refresh();
            this.Invalidate();
        }

        //----------------------------------------------------------------------
        //Calcule le Fdec avec les différents paramètres
        //----------------------------------------------------------------------
        private string Calc_freq(string div, string countmax)
        {
            return Chiffre.Significant((200000 / (double.Parse(div)) / double.Parse(countmax) / 2).ToString());
        }

        //----------------------------------------------------------------------
        //Fonction qui gére la picturebox
        //----------------------------------------------------------------------
        private void Pbox_courbe_paint(object sender, PaintEventArgs e)
        {
            PictureBox courbeencour = (PictureBox)sender;
            float longueur = courbeencour.Width;
            float hauteur = courbeencour.Height;
            if (this.tc_mode_commande.SelectedIndex==0)//mode double pulse
            {
                Dessin.Pbox_courbe_paint_2pulse(longueur, hauteur, float.Parse(this.tb_db_T_Pulse1.Text), float.Parse(this.tb_db_T_Pulse2.Text), float.Parse(this.tb_db_T_Pulse3.Text), /*int.Parse(this.tb_niv_actif.Text)*/ 0, cmb_Choix_DP_Transistor.SelectedIndex, float.Parse(tb_tpsmort.Text), e);
            }
            else if (this.tc_mode_commande.SelectedIndex ==1)//mode boucle ouverte
            {
                List<CourbePuiTriangle> courbe_pui = new List<CourbePuiTriangle>();
                if (this.chb_ActiveB1.Checked)
                {
                    courbe_pui.Add(new CourbePuiTriangle(this.tb_tri1.Text, this.cmb_tri1.SelectedIndex.ToString(), "Bras 1", chb_ModeDaltonien.Checked ? new Pen(Color.Chartreuse, 2) : new Pen(Color.Navy, 2)));
                }
                if (this.chb_ActiveB2.Checked)
                {
                    courbe_pui.Add(new CourbePuiTriangle(this.tb_tri2.Text, this.cmb_tri2.SelectedIndex.ToString(), "Bras 2", chb_ModeDaltonien.Checked ? new Pen(Color.Navy, 2) : new Pen(Color.Maroon, 2)));
                }
                if (this.chb_ActiveB3.Checked)
                {
                    courbe_pui.Add(new CourbePuiTriangle(this.tb_tri3.Text, this.cmb_tri3.SelectedIndex.ToString(), "Bras 3", chb_ModeDaltonien.Checked ? new Pen(Color.LightSkyBlue, 2) : new Pen(Color.DarkMagenta, 2)));
                }
                if (this.chb_ActiveB4.Checked)
                {
                    courbe_pui.Add(new CourbePuiTriangle(this.tb_tri4.Text, this.cmb_tri4.SelectedIndex.ToString(), "Bras 4", chb_ModeDaltonien.Checked ? new Pen(Color.OrangeRed, 2) : new Pen(Color.LightSeaGreen, 2)));
                }
                if (courbe_pui.Count > 0)
                {
                    Dessin.Pbox_courbe_paint_boucleouverte(longueur, hauteur, courbe_pui, tb_freq_countmax.Text, e);
                }
            }
        }
        
        //----------------------------------------------------------------------
        //Fonction qui gére les conséquence de changer le combobox consigne
        //( bref si choix consigne fixe permet de choisir la valeur)
        //----------------------------------------------------------------------
        private void Cmb_consigne_changed(object sender, EventArgs e)
        {
            //afficher ou cacher les bonnes tb des consignes
            if (((ComboBox)sender).Name == "cmb_consigne1")
            {
                this.Cmb_gestion_consigne(this.tb_consigne1, this.tb_con_max1, this.cmb_consigne_CAN1, this.cmb_consigne1.SelectedIndex, this.tb_div_sinus1, this.gb_sinus_fpga1, this.l_consigne_CAN1, this.cmb_consigne_CAN1,this.tb_amplitude1,this.tb_offset1);
            }
            if (((ComboBox)sender).Name == "cmb_consigne2")
            {
                this.Cmb_gestion_consigne(this.tb_consigne2, this.tb_con_max2, this.cmb_consigne_CAN2, this.cmb_consigne2.SelectedIndex, this.tb_div_sinus2, this.gb_sinus_fpga2, this.l_consigne_CAN2, this.cmb_consigne_CAN2, this.tb_amplitude2, this.tb_offset2);
            }
            if (((ComboBox)sender).Name == "cmb_consigne3")
            {
                this.Cmb_gestion_consigne(this.tb_consigne3, this.tb_con_max3, this.cmb_consigne_CAN3, this.cmb_consigne3.SelectedIndex, this.tb_div_sinus3, this.gb_sinus_fpga3, this.l_consigne_CAN3, this.cmb_consigne_CAN3, this.tb_amplitude3, this.tb_offset3);
            }

            this.Link_tb_manuel(sender);//pour mettre à jour les variables à envoyé
            
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
        //Fonction qui permet d'activer le controle et l'affichage des options du bras en question
        //calcul aussi le nombre de bras active
        //----------------------------------------------------------------------
        private void Cb_Active_changed(object sender, EventArgs e)
        {
            int temp = 0;
            this.list_sens_tri.Clear();
            this.list_val_tri.Clear();
            if (this.chb_ActiveB1.Checked)
            {
                this.list_val_tri.Add(tb_tri1);
                this.list_sens_tri.Add(cmb_tri1);
                temp++;
            }
            if (this.chb_ActiveB2.Checked)
            {
                this.list_val_tri.Add(tb_tri2);
                this.list_sens_tri.Add(cmb_tri2);
                temp++;
            }
            if (this.chb_ActiveB3.Checked)
            {
                this.list_val_tri.Add(tb_tri3);
                this.list_sens_tri.Add(cmb_tri3);
                temp++;
            }
            if (this.chb_ActiveB4.Checked)
            {
                this.list_val_tri.Add(tb_tri4);
                this.list_sens_tri.Add(cmb_tri4);
                temp++;
            }
            this.tb_Nbbras.Text = temp.ToString();
            this.Link_tb_manuel(sender);
        }

        //----------------------------------------------------------------------
        //Fonction qui calcule les valeur pour le mode entrelacé
        //met les valeurs met à jour les %
        //----------------------------------------------------------------------
        private void B_com_multi_niv_Click(object sender, EventArgs e)
        {
            Calc_conv_puissance.Multiniv(list_val_tri, list_sens_tri, int.Parse(tb_freq_countmax.Text), true);
            Valid_Select();
        }

        //----------------------------------------------------------------------
        //Fonction qui met les valeurs met à jour les %
        //----------------------------------------------------------------------
        private void Valid_Select()
        {
            this.tb_tri1.Select();// pour mettre à jour les %
            this.tb_tri2.Select();
            this.tb_tri3.Select();
            this.tb_tri4.Select();
            this.tb_consigne1.Select();
            this.tb_consigne2.Select();
            this.tb_con_max1.Select();
            this.tb_con_min1.Select();
            this.tb_con_max2.Select();
            this.tb_con_min2.Select();
            this.b_com_multi_niv.Select();
        }

        //----------------------------------------------------------------------
        //Fonction qui permet lors de la validation de l'utilisation de la roue codeuse de disable tout les textbox tpsmort
        //----------------------------------------------------------------------
        private void Chb_Rouecodeuse_CheckedChanged(object sender, EventArgs e)
        {
            this.tb_tpsmort.Enabled = !((CheckBox)sender).Checked;
            this.l_tps_mortns.Enabled = !((CheckBox)sender).Checked;
            this.l_tpsmort.Enabled = !((CheckBox)sender).Checked;
            this.Link_tb_manuel(sender);
        }

        //----------------------------------------------------------------------
        //Fonction qui refait les dessins quand on change l'index
        //----------------------------------------------------------------------
        private void Tc_mode_commande_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tc_mode_commande.SelectedIndex == 0)   //si double pulse afficher
            {
                this.gB_CAN.Visible = false;                     //alors on désactive la gestion des CANs
                this.gB_freq.Visible = false;                   // et l'utilisation de la fréquence
                GValeurForm.String_replace(this.tb_val_5, "0000", 4); //si DP on agis avec les bras inactifs
            }
            else
            {
                this.gB_CAN.Visible = true;                      //sinon on l'active
                this.gB_freq.Visible = true;
                this.Link_tb_manuel(chb_ActiveB1);//on prend en compte quelle bras est actif
                this.Link_tb_manuel(chb_ActiveB2);//on prend en compte quelle bras est actif
                this.Link_tb_manuel(chb_ActiveB3);//on prend en compte quelle bras est actif
                this.Link_tb_manuel(chb_ActiveB4);//on prend en compte quelle bras est actif
                if (this.tc_mode_commande.SelectedIndex == 1)//si menu boucle ouverte
                {
                    this.Cmb_consigne_changed(this.cmb_consigne1, e);//les consignes sont mise a jour suivant les combobox
                    this.Cmb_mode_puissance_SelectedIndexChanged(sender, e);
                }
            }
            this.Refresh();
        }

        //----------------------------------------------------------------------
        //Fonction qui refait les dessins quand on change l'index
        //----------------------------------------------------------------------
        private void Cb_Choix_DP_Transistor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Link_tb_manuel(sender);
        }

        //----------------------------------------------------------------------
        //Fonction qui refait les dessins quand on change l'index
        //----------------------------------------------------------------------
        private void Cmb_tri_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Test_general_proxi_change_sens();
            this.Link_tb_manuel(sender);
        }

        //----------------------------------------------------------------------
        //test pour savoir si on est à 2 ou moins d'un changement de sens si oui
        //change le sens, car sinon il y aura un bug sur le FPGA
        //----------------------------------------------------------------------
        private void Test_proxi_change_sens(ComboBox cmb,TextBox tb)
        {
            //test pour savoir si on est à 2 ou moins d'un changement de sens si oui
            //change le sens, car sinon il y aura un bug sur le FPGA
            if(int.Parse(tb.Text) >= int.Parse(this.tb_freq_countmax.Text)-2 && cmb.SelectedIndex == 0)
            {
                cmb.SelectedIndex = 1;//si trop proche en monté on passe en descente
            }
            else if (int.Parse(tb.Text) <= 2 && cmb.SelectedIndex == 1)
            {
                cmb.SelectedIndex = 0;//si trop proche en monté on passe en descente
            }
        }

        //----------------------------------------------------------------------
        //Fonction qui lance le test de proximité de changement pour chaque bras
        //
        //----------------------------------------------------------------------
        private void Test_general_proxi_change_sens()
        {
            this.Test_proxi_change_sens(this.cmb_tri1, this.tb_tri1);
            this.Test_proxi_change_sens(this.cmb_tri2, this.tb_tri2);
            this.Test_proxi_change_sens(this.cmb_tri3, this.tb_tri3);
            this.Test_proxi_change_sens(this.cmb_tri4, this.tb_tri4);
        }

        //----------------------------------------------------------------------
        //Fonction qui met tout les paramètres important dans une liste et appel la fonction sauvegarde
        //----------------------------------------------------------------------
        private void SauvegarderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GestionFichier.Sauvegarde(this.tp_manuel, "-1");
        }

        //----------------------------------------------------------------------
        //Fonction appeler par le bouton chargment
        //----------------------------------------------------------------------
        private void ChargementToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GestionFichier.Chargement(this.tp_manuel, "-1");
            // charge les valeurs sauvegarder dans le fichier qui sera choisis
            // this => cette form, permet d'affecter tout les control (textbox combobox, checkbox)
        }

        //----------------------------------------------------------------------
        //Fonction qui ouvre les options
        //----------------------------------------------------------------------
        private void Tsm_option_Click(object sender, EventArgs e)
        {
            new Options().ShowDialog(this);

            //Chargement des nouvelles options qui ont été enregistré dans le .ini
            GestionFichier.Chargement(this.tb_quartus_stpexe, "Options.ini");
            GestionFichier.Chargement(this.tb_TCL, "Options.ini");
            GestionFichier.Chargement(this.chb_AutoLoad, "Options.ini");
            GestionFichier.Chargement(this.chb_ModeDaltonien, "Options.ini");
            GestionFichier.Chargement(this.chb_valid_expert, "Options.ini");
            this.chb_send_auto.Enabled = this.chb_valid_expert.Checked;
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
                    Etat_co = -4;
                }
            }
        }
        
        //----------------------------------------------------------------------
        //Fonction qui lit les combos box pour choisir le signe V ou A des cans
        //----------------------------------------------------------------------
        private void Cb_Can_symb_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cb_can_symb_max.SelectedIndex = ((ComboBox)sender).SelectedIndex;
            this.cb_can_symb_null.SelectedIndex = ((ComboBox)sender).SelectedIndex;
            this.cb_can_symb_min.SelectedIndex = ((ComboBox)sender).SelectedIndex;
            this.l_can_max_symbole.Text = ((ComboBox)sender).Text;
            this.l_can_min_symbole.Text = ((ComboBox)sender).Text;
        }

        //----------------------------------------------------------------------
        //Fonction qui calcul le quantum des can et moyenne
        //
        //type => si 0 alors on recalcul les valeurs réels
        //     => si 1 alors on recalcul les valeurs en 10bits
        //     => si 2 alors on recalcul les valeur mes10bits en valeur bruts
        //     => sinon et dans tout les cas, on calcul la valeur moyenne et met a jour le quantum
        //----------------------------------------------------------------------
        private void Can_Quantum(int type)
        {
            double quantum;
            //Fonction qui calcul la valeur moyenne entre min et max
            tb_can_etal_mes_null.Text = Math.Round((double.Parse(tb_can_etal_mes_max.Text) + double.Parse(tb_can_etal_mes_min.Text)) / 2).ToString();
            tb_can_etal_rep_null.Text = Math.Round((double.Parse(tb_can_etal_rep_max.Text) + double.Parse(tb_can_etal_rep_min.Text)) / 2).ToString();

            double tb_can_etal_mes_moy = double.Parse(tb_can_etal_mes_max.Text);
            double tb_can_etal_rep_moy = double.Parse(tb_can_etal_rep_max.Text);

            //calcul du quantum
            quantum = (double.Parse(tb_can_etal_mes_max.Text) - double.Parse(tb_can_etal_mes_min.Text)) / (double.Parse(tb_can_etal_rep_max.Text) - double.Parse(tb_can_etal_rep_min.Text));
            l_can_quantum.Text = "chaque bit représente " + (Math.Round(quantum * 10000) / 10).ToString() + " m" + cb_can_symb_max.Text; // mise a jour du label

            if (type == 0) //calcul des valeurs réels
            {
                tb_canreel_max.Text = Math.Round(quantum * (double.Parse(tb_can10b_max.Text) - double.Parse(tb_can_etal_rep_null.Text)) + double.Parse(tb_can_etal_mes_null.Text)).ToString();
                tb_canreel_min.Text = Math.Round(quantum * (double.Parse(tb_can10b_min.Text) - double.Parse(tb_can_etal_rep_null.Text)) + double.Parse(tb_can_etal_mes_null.Text)).ToString();
            }
            else if (type == 1)
            {
                tb_can10b_max.Text = Math.Round((double.Parse(tb_canreel_max.Text) - double.Parse(tb_can_etal_mes_null.Text)) / quantum + double.Parse(tb_can_etal_rep_null.Text)).ToString();
                tb_can10b_min.Text = Math.Round((double.Parse(tb_canreel_min.Text) - double.Parse(tb_can_etal_mes_null.Text)) / quantum + double.Parse(tb_can_etal_rep_null.Text)).ToString();
            }
            else if (type == 2) // si on veux transformer les valeur mesuré en valeur brut
            {
                // quantum = quantum * 100;//pour après avoir 2 chiffre après la virgule
                if (chb_mes_can1.Checked && tb_10b_can1.Text != "")//si on a valider can1 et que la case tb_10b_can1 n'est pas vide
                    //tb_mes_can1.Text = (Math.Round(quantum * (double.Parse(tb_10b_can1.Text) - double.Parse(tb_can_etal_rep_null.Text))) / 100 + double.Parse(tb_can_etal_mes_null.Text)).ToString();                    
                    tb_mes_can1.Text = (quantum * (double.Parse(tb_10b_can1.Text) - tb_can_etal_rep_moy) + tb_can_etal_mes_moy).ToString();
                tb_mes_can1.Text = Chiffre.Significant(tb_mes_can1.Text);
                if (chb_mes_can2.Checked && tb_10b_can2.Text != "")
                    //tb_mes_can2.Text = (Math.Round(quantum * (double.Parse(tb_10b_can2.Text) - double.Parse(tb_can_etal_rep_null.Text))) / 100 + double.Parse(tb_can_etal_mes_null.Text)).ToString();
                    tb_mes_can2.Text = (quantum * (double.Parse(tb_10b_can2.Text) - tb_can_etal_rep_moy) + tb_can_etal_mes_moy).ToString();
                tb_mes_can2.Text = Chiffre.Significant(tb_mes_can2.Text);
                if (chb_mes_can3.Checked && tb_10b_can3.Text != "")
                    //tb_mes_can3.Text = (Math.Round(quantum * (double.Parse(tb_10b_can3.Text) - double.Parse(tb_can_etal_rep_null.Text))) / 100 + double.Parse(tb_can_etal_mes_null.Text)).ToString();
                    tb_mes_can3.Text = (quantum * (double.Parse(tb_10b_can3.Text) - tb_can_etal_rep_moy) + tb_can_etal_mes_moy).ToString();
                tb_mes_can3.Text = Chiffre.Significant(tb_mes_can3.Text);
                if (chb_mes_can4.Checked && tb_10b_can4.Text != "")
                    //tb_mes_can4.Text = (Math.Round(quantum * (double.Parse(tb_10b_can4.Text) - double.Parse(tb_can_etal_rep_null.Text))) / 100 + double.Parse(tb_can_etal_mes_null.Text)).ToString();
                    tb_mes_can4.Text = (quantum * (double.Parse(tb_10b_can4.Text) - tb_can_etal_rep_moy) + tb_can_etal_mes_moy).ToString();
                tb_mes_can4.Text = Chiffre.Significant(tb_mes_can4.Text);
            }

        }

        //----------------------------------------------------------------------
        //Fonction appelé quand une textbox sur les can change
        //
        //appel la fonction quantum avec les bonnes options
        //----------------------------------------------------------------------
        private void Can_etal_mes_Validating(object sender, CancelEventArgs e)
        {
            this.Test_et_Conv_des_tb(sender, e); //vérifier que le signal respecte sa mise en forme

            //regarde si la valeur est en forme brut
            if (((TextBox)sender) == this.tb_can_etal_mes_max || ((TextBox)sender) == this.tb_can_etal_mes_min || ((TextBox)sender) == this.tb_canreel_max || ((TextBox)sender) == this.tb_canreel_min)
            {
                this.Can_Quantum(1); // si oui mettre à jour les cases en 10bits
            }
            else
            {
                this.Can_Quantum(0); // sinon mettre à jour les cases en brut
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
        //Fonction qui lance le calcul pour les valeur mesurée du CAN
        //en gros on prend la valeur 10 bits et on calcul ce que ça représente en brut
        //----------------------------------------------------------------------
        private void Conv_mes10b_vers_mes(object sender, EventArgs e)
        {
            this.Can_Quantum(2);
        }

        //----------------------------------------------------------------------
        //Fonction pour le s à bras
        //----------------------------------------------------------------------
        private void Tb_Nbbras_TextChanged(object sender, EventArgs e)
        {
            if (int.Parse(this.tb_Nbbras.Text) > 1) // si nombre de bras superrieur à 1
            {
                this.l_Nbbras.Text = "Nombre de bras actifs :"; // alors on met le s
            }
            else
            {
                this.l_Nbbras.Text = "Nombre de bras actif :"; // sinon on l'enlève
            }
        }

        //----------------------------------------------------------------------
        //Fonction appelé par un click qui appel la fonction mesure
        //----------------------------------------------------------------------
        private void B_mesure_can_Click(object sender, EventArgs e)
        {
            this.Lancer_mesure(); //environs 385 mesures /seconde
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
        //Fonction qui affiche les bons mode textbox suivant le choix
        //permet de choisir son mode de fonctionnement
        //----------------------------------------------------------------------
        private void Cmb_mode_puissance_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chb_surmodulation.Enabled = false;
            this.chb_surmodulation.Checked = false;
            switch (this.cmb_mode_puissance.SelectedIndex)
            {
                case 0://mode boucle ouverte
                    this.gb_consigne1.Visible = true;
                    this.gb_consigne2.Visible = false;
                    this.gb_consigne3.Visible = false;
                    this.cmb_cons_B1.Visible = false;//les 4 autres suivent
                    this.gb_PID1.Visible = false;//l'autre suit
                    this.tabPage2.Text = "Boucle ouverte";
                    break;
                case 1://mode boucle fermé
                    this.gb_consigne1.Visible = true;
                    this.gb_consigne2.Visible = false;
                    this.gb_consigne3.Visible = false;
                    this.cmb_cons_B1.Visible = false;//les 4 autres suivent
                    this.gb_PID1.Visible = true;//l'autre suit
                    this.tabPage2.Text = "Boucle fermé";
                    break;
                case 2://mode opposition
                    this.gb_consigne1.Visible = true;
                    this.gb_consigne2.Visible = true;
                    this.gb_consigne3.Visible = false;
                    this.cmb_cons_B1.Visible = true;//les 4 autres suivent
                    this.gb_PID1.Visible = false;//l'autre suit
                    this.tabPage2.Text = "Mode opposition";
                    break;
                case 3://mode complet
                    this.gb_consigne1.Visible = true;
                    this.gb_consigne2.Visible = true;
                    this.gb_consigne3.Visible = true;
                    this.cmb_cons_B1.Visible = true;//les 4 autres suivent
                    this.gb_PID1.Visible = true;//l'autre suit
                    this.tabPage2.Text = "Tout voir";
                    break;
                case 4://mode Triphasé
                    this.gb_consigne1.Visible = true;
                    this.gb_consigne2.Visible = true;
                    this.gb_consigne3.Visible = true;
                    this.cmb_cons_B1.Visible = true;//les 4 autres suivent
                    this.gb_PID1.Visible = false;//l'autre suit
                    this.tabPage2.Text = "Mode Triphasé";
                    if (cmb_cons_B1.Items.IndexOf("Consigne 3") == -1)
                    {
                        this.cmb_cons_B1.Items.Add("Consigne 3");
                        this.cmb_cons_B2.Items.Add("Consigne 3");
                        this.cmb_cons_B3.Items.Add("Consigne 3");
                        this.cmb_cons_B4.Items.Add("Consigne 3");
                    }
                    this.chb_surmodulation.Enabled = true;
                    break;
            }
            this.Cmb_consigne_changed(this.cmb_consigne1, e);
            GValeurForm.String_replace(tb_val_12, "0000", 0); // 4 premiers bits de tb_val_12 à 0
        }



        //----------------------------------------------------------------------
        //Fonction pour bien placer consigne 2 suivant la taille de consigne 1
        //----------------------------------------------------------------------
        private void Gb_consigne_Resize(object sender, EventArgs e)
        {
            this.gb_consigne2.Location = new Point(13 + this.gb_consigne1.Width, this.gb_consigne2.Location.Y);
            this.gb_consigne3.Location = new Point(7 + this.gb_consigne2.Location.X + this.gb_consigne1.Width, this.gb_consigne2.Location.Y);

            //taille maximum des consignes pour le placement des groupesbox des bras
            int tailley = Math.Max(this.gb_consigne2.Height, this.gb_consigne1.Height);
            tailley = Math.Max(tailley, this.gb_consigne3.Height)+7;
            this.gb_ActB1.Location = new Point(this.gb_ActB1.Location.X, tailley);
            this.gb_ActB2.Location = new Point(this.gb_ActB2.Location.X, tailley);
            this.gb_ActB3.Location = new Point(this.gb_ActB3.Location.X, tailley);
            this.gb_ActB4.Location = new Point(this.gb_ActB4.Location.X, tailley);
        }

        //----------------------------------------------------------------------
        //Affiche et rend active tout les controls
        //----------------------------------------------------------------------
        private void SuperUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Control> control_T = new List<Control>();
            control_T  = GValeurForm.Crealist(this);
            control_T.AddRange(GValeurForm.Crealist(this, typeof(Button)));
            foreach(Control ct in control_T)
            {
                ct.Enabled = true;
            }
            b_lancer_serveur.Enabled = true;
            b_Connection.Enabled = true;
        }
        

        private void Bt_aide1_Click(object sender, EventArgs e)
        {
            new aide.Parametres_generaux().ShowDialog();
        }

        private void Bt_aide2_Click(object sender, EventArgs e)
        {
            new aide.Frequence().ShowDialog();
        }

        private void Bt_aide3_Click(object sender, EventArgs e)
        {
            new aide.Etalon().ShowDialog();
        }

        private void Bt_aide4_Click(object sender, EventArgs e)
        {
            new aide.Seuil_defaillance().ShowDialog();
        }

        private void Bt_aide5_Click(object sender, EventArgs e)
        {
            new aide.Mesure().ShowDialog();
        }

        private void Bt_aide6_Click(object sender, EventArgs e)
        {
            new aide.Double_pulse().ShowDialog();
        }

        private void Bt_aide7_Click(object sender, EventArgs e)
        {
            new aide.Mode_fonctionnement().ShowDialog();
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

        // afficher les boutons "inverser bras haut-bas"  
        private void Chb_mode_onduleur_CheckedChanged(object sender, EventArgs e)
        {
            this.Initialiser_chb_inv_bras(this.chb_mode_onduleur.Checked);
        }

        //détermine qui demande la fonction calcule l'offset
        private void Tb_amplitude_et_offset_Validating(object sender, CancelEventArgs e)
        {
            if (sender == this.tb_amplitude1 || sender == this.tb_offset1)
            {
                tb_val_57.Text = Calcul_Offset(this.tb_amplitude1, this.tb_offset1).ToString();
            }
            else if (sender == this.tb_amplitude2)
            {
                tb_val_58.Text = Calcul_Offset(this.tb_amplitude2, this.tb_offset2).ToString();
            }
            else if (sender == this.tb_amplitude3)
            {
                tb_val_59.Text = Calcul_Offset(this.tb_amplitude3, this.tb_offset3).ToString();

            }
        }

        //calcul l'offset a envoyer au FPGA pour remplir les conditions demandé
        //met a jour les conditions si elles ne sont pas atteignables
        private double Calcul_Offset(TextBox amplitude,TextBox offset)
        {
            double damplitude = double.Parse(amplitude.Text);
            double doffset = double.Parse(offset.Text);
            double temp = Math.Floor((1023 - damplitude) / 2) +doffset - 512;
            if (temp < 0)
            {
                offset.Text = (double.Parse(offset.Text) - temp).ToString();
                temp = Calcul_Offset(amplitude, offset);
            }
            else if (damplitude + temp >1023)
            {
                offset.Text = (double.Parse(offset.Text) + (1023 - (damplitude + temp))).ToString();
                temp = Calcul_Offset(amplitude, offset);
            }

            return temp;
        }


        // afficher les boutons "inverser bras haut-bas"  
        private void Initialiser_chb_inv_bras(Boolean etat)
        {
            this.chb_inv_bras_1.Visible = etat;
            this.chb_inv_bras_2.Visible = etat;
            this.chb_inv_bras_3.Visible = etat;
            this.chb_inv_bras_4.Visible = etat;
            int decalage = 0;
            if (etat)
            {
                decalage = this.chb_inv_bras_1.Size.Height;
            }
            else
            {
                decalage = 0 - this.chb_inv_bras_1.Size.Height;
            }
            this.Decalage_cmbbox_bras(decalage);

            if (etat == false)
            {
                chb_inv_bras_1.Checked = false;
                chb_inv_bras_2.Checked = false;
                chb_inv_bras_3.Checked = false;
                chb_inv_bras_4.Checked = false;
            }
        }

        // afficher le groubbox boutons "max min" quand 
        // le mode multiniveau est coché
        private void Chb_mode_Multiniv_CheckedChanged(object sender, EventArgs e)
        {
            this.Initialiser_chb_multi_bras(this.chb_mode_multiniv.Checked);
        }

        //met à jour les cases du type de consignes suivant sont état
        private void Chb_surmodulation_changed(object sender, EventArgs e)
        {
            if (this.cmb_consigne1.SelectedIndex == 1)
                this.Cmb_consigne_changed(cmb_consigne1, e);
            if (this.cmb_consigne2.SelectedIndex == 1)
                this.Cmb_consigne_changed(cmb_consigne2, e);
            if (this.cmb_consigne3.SelectedIndex == 1)
                this.Cmb_consigne_changed(cmb_consigne3, e);
        }

        // afficher les boutons "inverser bras haut-bas"
        // et faire le décalage
        private void Initialiser_chb_multi_bras(Boolean etat)
        {
            this.gb_multinivbras_1.Visible = etat;
            this.gb_multinivbras_2.Visible = etat;
            this.gb_multinivbras_3.Visible = etat;
            this.gb_multinivbras_4.Visible = etat;
            int decalage = 0;
            if (etat)
            {
                decalage = this.gb_multinivbras_1.Size.Height;
            }
            else
            {
                decalage = 0 - this.gb_multinivbras_1.Size.Height;
            }
            this.Decalage_cmbbox_bras(decalage);
            this.chb_inv_bras_1.Top = this.chb_inv_bras_1.Top + decalage;
            this.chb_inv_bras_2.Top = this.chb_inv_bras_1.Top;
            this.chb_inv_bras_3.Top = this.chb_inv_bras_1.Top;
            this.chb_inv_bras_4.Top = this.chb_inv_bras_1.Top;

            // affecter la valeur par défaut aux TextBox quand on décocche le mode superposition
            Initialiser_multiniveau_bras(gb_multinivbras_1, tb_multinivmax1, tb_multinivmax_P1, tb_multinivmin1, tb_multinivmin_P1);
            Initialiser_multiniveau_bras(gb_multinivbras_2, tb_multinivmax2, tb_multinivmax_P2, tb_multinivmin2, tb_multinivmin_P2);
            Initialiser_multiniveau_bras(gb_multinivbras_3, tb_multinivmax3, tb_multinivmax_P3, tb_multinivmin3, tb_multinivmin_P3);
            Initialiser_multiniveau_bras(gb_multinivbras_4, tb_multinivmax4, tb_multinivmax_P4, tb_multinivmin4, tb_multinivmin_P4);
        }

        // Décaler sur Y la position des combox sens, et choix consigne
        private void Decalage_cmbbox_bras(int decalage)
        {
            this.cmb_tri1.Top = this.cmb_tri1.Top + decalage;
            this.cmb_tri2.Top = this.cmb_tri1.Top;
            this.cmb_tri3.Top = this.cmb_tri1.Top;
            this.cmb_tri4.Top = this.cmb_tri1.Top;
            this.cmb_cons_B1.Top = this.cmb_cons_B1.Top + decalage;
            this.cmb_cons_B2.Top = this.cmb_cons_B1.Top;
            this.cmb_cons_B3.Top = this.cmb_cons_B1.Top;
            this.cmb_cons_B4.Top = this.cmb_cons_B1.Top;
        }


        // Verifier que le diviseur est compris entre 0 et 1023, 
        // calculer et afficher la valeur de la fréquence correspondante
        private void Tb_diviseur_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = CalculFeuille.Casenum(((TextBox)sender).Text, 0, 1023, 1, 1, "1");//verifie si la valeur est bonnes
            if (((TextBox)sender).Name == "tb_div_sinus1")
            {
                tb_val_freq_sinus1.Text = Chiffre.Significant((200e6 / 4096 / 2 / double.Parse(((TextBox)sender).Text)).ToString());
            }
            else if (((TextBox)sender).Name == "tb_div_sinus2")
            {
                tb_val_freq_sinus2.Text = Chiffre.Significant((200e6 / 4096 / 2 / double.Parse(((TextBox)sender).Text)).ToString());
            }
            else if (((TextBox)sender).Name == "tb_div_sinus3")
            {
                tb_val_freq_sinus3.Text = Chiffre.Significant((200e6 / 4096 / 2 / double.Parse(((TextBox)sender).Text)).ToString());
            }

        }

        // Vérifier que le contenu des textbox max 
        // et min respecte les critères suivants : 
        // 0 ou min < max < freq_max
        // 0 ou %min < %max < 100
        // 0 < min < max ou freq_max
        // 0 < %min < %max ou 100
        private void Gb_extremum_bras(object sender, TextBox max_brut, 
            TextBox max_pourcentage, TextBox min_brut, TextBox min_pourcentage)
        {
            double pas_pourcentage = 0.1;
            int pas_extremum = 1;
            int type = 1;
            double freq_max_brut = double.Parse(tb_freq_countmax.Text);


            if (sender == max_brut)
            {
                try
                {
                    max_brut.Text = CalculFeuille.Casenum(max_brut.Text, double.Parse(min_brut.Text), freq_max_brut, pas_extremum, type, tb_freq_countmax.Text);
                }
                catch
                {
                    max_brut.Text = tb_freq_countmax.Text;
                }
                max_pourcentage.Text = Math.Round(double.Parse(max_brut.Text) / freq_max_brut * 100, 1).ToString();
            }
            else if (sender == max_pourcentage)
            {
                try
                {
                    max_pourcentage.Text = CalculFeuille.Casenum(max_pourcentage.Text, double.Parse(min_pourcentage.Text), 100, pas_pourcentage, type, "0");
                }
                catch
                {
                    max_pourcentage.Text = "100";
                }
                max_brut.Text = Math.Round(double.Parse(max_pourcentage.Text) * freq_max_brut / 100).ToString();


            }
            else if (sender == min_brut)
            {
                try
                {
                    min_brut.Text = CalculFeuille.Casenum(min_brut.Text, 0, double.Parse(max_brut.Text), pas_extremum, type, "0");
                }
                    catch
                {
                    min_brut.Text = "0";
                }
                min_pourcentage.Text = Math.Round(double.Parse(min_brut.Text) / freq_max_brut * 100, 1).ToString();
            }
            else if (sender == min_pourcentage)
            {
                try
                {
                    min_pourcentage.Text = CalculFeuille.Casenum(min_pourcentage.Text, 0, double.Parse(max_pourcentage.Text), pas_pourcentage, type, "0");
                }
                catch
                {
                    min_pourcentage.Text = "0";
                }
                min_brut.Text = Math.Round(double.Parse(min_pourcentage.Text) * freq_max_brut / 100).ToString();
            }
        }

        private void Bras_1_extremum_Changed(object sender, EventArgs e)
        {
            this.Gb_extremum_bras(sender, this.tb_multinivmax1, this.tb_multinivmax_P1, this.tb_multinivmin1, this.tb_multinivmin_P1);
        }

        private void Bras_2_extremum_Changed(object sender, EventArgs e)
        {
            this.Gb_extremum_bras(sender, this.tb_multinivmax2, this.tb_multinivmax_P2, this.tb_multinivmin2, this.tb_multinivmin_P2);
        }

        private void Bras_3_extremum_Changed(object sender, EventArgs e)
        {
            this.Gb_extremum_bras(sender, this.tb_multinivmax3, this.tb_multinivmax_P3, this.tb_multinivmin3, this.tb_multinivmin_P3);
        }

        private void Bras_4_extremum_Changed(object sender, EventArgs e)
        {
            this.Gb_extremum_bras(sender, this.tb_multinivmax4, this.tb_multinivmax_P4, this.tb_multinivmin4, this.tb_multinivmin_P4);
        }

        //Calculer la valeur max et min pour chaque bras
        //dans le mode commande ???
        private void Bt_com_Click(object sender, EventArgs e)
        {
            List<TextBox> tb_max = new List<TextBox>();
            List<TextBox> tb_min = new List<TextBox>();
            if (this.chb_ActiveB1.Checked)//si le bras est actif, rajoute les tb corresondante
            {
                tb_max.Add(this.tb_multinivmax1);
                tb_min.Add(this.tb_multinivmin1);
            }
            if (chb_ActiveB2.Checked)
            {
                tb_max.Add(this.tb_multinivmax2);
                tb_min.Add(this.tb_multinivmin2);
            }
            if (chb_ActiveB3.Checked)
            {
                tb_max.Add(this.tb_multinivmax3);
                tb_min.Add(this.tb_multinivmin3);
            }
            if (chb_ActiveB4.Checked)
            {
                tb_max.Add(this.tb_multinivmax4);
                tb_min.Add(this.tb_multinivmin4);
            }
            Calc_conv_puissance.Calc_multiniv(tb_max, tb_min, this.tb_freq_countmax);

        }

        // initialiser toutes les consignes des bras sur la consigne 1 
        // quand le combo box consigne 2 n'est plus visible
        private void Gb_consigne_VisibleChanged(object sender, EventArgs e)
        {
            if (gb_consigne2.Visible == false)
            {
                foreach(ComboBox cb in list_cmb_cons)
                {
                    cb.SelectedIndex = 0;
                }
            }
            if (gb_consigne3.Visible == false)
            {
                foreach (ComboBox cb in list_cmb_cons)
                {
                    if (cb.SelectedIndex == 2)
                        cb.SelectedIndex = 0;
                    if (cb.Items.Count == 3)
                        cb.Items.RemoveAt(2);
                }
            }
        }
        // affecter la valeur par défaut au text box d'un multiniveau
        private void Initialiser_tb_bras(TextBox max_b, TextBox max_p, TextBox min_b, TextBox min_p)
        {
            max_b.Text = tb_freq_countmax.Text;
            max_p.Text = "100";
            min_b.Text = "0";
            min_p.Text = "0";
        }

        // initialiser les text box d'un group box multiniveau quand
        // le mode superposition est désactiver et qu'il est invisible
        private void Initialiser_multiniveau_bras(GroupBox gb_multi, TextBox max_b,
            TextBox max_p, TextBox min_b, TextBox min_p)
        {
            if (gb_multi.Visible == false)
            {
                Initialiser_tb_bras(max_b, max_p, min_b, min_p);
            }
        }
        

        // lancer le bgw
        private void Mesure_courbe_click(object sender, EventArgs e)
        {
            new MesureOsci().Show(this);

        }

    }
}