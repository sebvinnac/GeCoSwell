using Gestion_Connection_Carte_FPGA;
using Gestion_Objet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeCoSwell
{
    class CAN : IGB_Spéciaux
    {
        #region Listes des objets
        //objet
        public GGroupBox Gb_Principal { get; private set; }
        private LLabel L_titre_étalonnage;
        private LLabel L_entete_Mesure;
        private LLabel L_entete_10Bits;
        private LLabel L_valeur_étatlonage_max;
        private TTextBox Tb_valeur_étalonnage_max;
        private TTextBox Tb_valeur_étalonnage_max_10bits;
        private LLabel L_valeur_étatlonage_min;
        private TTextBox Tb_valeur_étalonnage_min;
        private TTextBox Tb_valeur_étalonnage_min_10bits;
        private LLabel L_titre_détection_dépassement_seuil;
        private List<LLabel> Li_L_détection_valeur = new List<LLabel>();
        private List<TTextBox> Li_détection_valeur = new List<TTextBox>();
        private List<TTextBox> Li_détection_valeur_10bits = new List<TTextBox>();
        private List<CCheckBox> Li_détection_valeur_validé = new List<CCheckBox>();
        private LLabel L_titre_mesure;
        private List<LLabel> Li_L_mesure = new List<LLabel>();
        private List<TTextBox> Li_mesure = new List<TTextBox>();
        private List<TTextBox> Li_mesure_10bits = new List<TTextBox>();
        private List<CCheckBox> Li_mesure_activé = new List<CCheckBox>();
        private Bt_aide Bt_Aide;
        private Button Bt_aff_val_fpga;
        private Button Bt_mesure_can;
        private LLabel L_can_quantum;

        #endregion
        //autre variable

        public bool A_changé { get; private set; } = true;//indique qu'une valeur à changer
        public bool EstVisible { get; private set; } = true;
        public int Index_de_départ_du_DGV { get; private set; }
        public int Nombre_dadresse { get; private set; }
        public List<UneDataFPGA> Li_data_du_dgv { get; private set; }

        public ToolTip tooltip;

        #region Constructeur
        public CAN(int xpos, int ypos)
        {
            this.Init_Textbox_et_label(6, 19);
            this.Init_Bt_aide(412, 22);
            this.Init_Button(300,290);
            this.Init_Lier_méthodes_aux_objets();


            this.Init_Groupbox(xpos, ypos);

            this.Init_tooltip();
        }

        private void Init_Button(int xpos, int ypos)
        {
            this.Bt_aff_val_fpga = new Button();
            this.Bt_mesure_can = new Button();

            this.Bt_aff_val_fpga.Location = new System.Drawing.Point(xpos, ypos);
            this.Bt_aff_val_fpga.Name = "b_aff_val_fpga";
            this.Bt_aff_val_fpga.Size = new System.Drawing.Size(111, 37);
            this.Bt_aff_val_fpga.TabIndex = 1208;
            this.Bt_aff_val_fpga.Text = "Afficher valeur stocker dans FPGA";
            this.Bt_aff_val_fpga.UseVisualStyleBackColor = true;

            this.Bt_mesure_can.Location = new System.Drawing.Point(xpos, ypos +40);
            this.Bt_mesure_can.Name = "b_mesure_can";
            this.Bt_mesure_can.Size = new System.Drawing.Size(111, 37);
            this.Bt_mesure_can.TabIndex = 20;
            this.Bt_mesure_can.Text = "Mesurer une fois";
            this.Bt_mesure_can.UseVisualStyleBackColor = true;
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Ajoute les méthodes aux objets
        /// </summary>
        private void Init_Lier_méthodes_aux_objets()
        {
            for (int i = 0; i < 8; i++)
            {
                TTextBox.Lier_ratio_entre_2textbox(this.Li_détection_valeur[i], this.Li_détection_valeur_10bits[i]);
                Li_détection_valeur_validé[i].CheckedChanged += new EventHandler(Active_les_textboxlié_chb_changed);
                if (i < this.Li_mesure.Count)
                    TTextBox.Lier_ratio_entre_2textbox(this.Li_mesure[i], this.Li_mesure_10bits[i]);
            }
        }


        /// <summary>
        /// Créer le bouton d'aide
        /// </summary>
        /// <param name="xpos">sa position en x</param>
        /// <param name="ypos">sa position en y</param>
        private void Init_Bt_aide(int xpos, int ypos)
        {
            this.Bt_Aide = new Bt_aide(xpos, ypos);
        }

        private void Init_tooltip()
        {

            // Create the ToolTip and associate with the Form container.
            this.tooltip = new ToolTip()
            {
                // Set up the delays for the ToolTip.
                InitialDelay = 100,
                ReshowDelay = 100,
                AutoPopDelay = 5000,

                //Forme de balon
                IsBalloon = true
            };
            foreach (TTextBox tb in GestionObjet.Trouver_controls_dun_type(this.Gb_Principal, typeof(TTextBox)))
            {
                tooltip.SetToolTip(tb, tb.GeneText_ToolTip());
            }
        }

        private void Init_Groupbox(int xpos, int ypos)
        {
            this.Gb_Principal = new GGroupBox()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new System.Drawing.Point(xpos, ypos),
                Name = "Fréquence",
                TabStop = false,
                Text = "Fréquence"
            };
            this.Gb_Principal.Controls.Add(this.L_titre_étalonnage);
            this.Gb_Principal.Controls.Add(this.L_entete_Mesure);
            this.Gb_Principal.Controls.Add(this.L_entete_10Bits);
            this.Gb_Principal.Controls.Add(this.L_valeur_étatlonage_max);
            this.Gb_Principal.Controls.Add(this.Tb_valeur_étalonnage_max);
            this.Gb_Principal.Controls.Add(this.Tb_valeur_étalonnage_max_10bits);
            this.Gb_Principal.Controls.Add(this.L_valeur_étatlonage_min);
            this.Gb_Principal.Controls.Add(this.Tb_valeur_étalonnage_min);
            this.Gb_Principal.Controls.Add(this.Tb_valeur_étalonnage_min_10bits);
            this.Gb_Principal.Controls.Add(this.L_titre_détection_dépassement_seuil);
            foreach (LLabel label in Li_L_détection_valeur)
                this.Gb_Principal.Controls.Add(label);
            foreach (TTextBox tb in Li_détection_valeur)
                this.Gb_Principal.Controls.Add(tb);
            foreach (TTextBox tb in Li_détection_valeur_10bits)
                this.Gb_Principal.Controls.Add(tb);
            foreach (CCheckBox chb in Li_détection_valeur_validé)
                this.Gb_Principal.Controls.Add(chb);
            this.Gb_Principal.Controls.Add(this.L_titre_mesure);
            foreach (LLabel label in Li_L_mesure)
                this.Gb_Principal.Controls.Add(label);
            foreach (TTextBox tb in Li_mesure)
                this.Gb_Principal.Controls.Add(tb);
            foreach (TTextBox tb in Li_mesure_10bits)
                this.Gb_Principal.Controls.Add(tb);
            foreach (CCheckBox chb in Li_mesure_activé)
                this.Gb_Principal.Controls.Add(chb);
            this.Gb_Principal.Controls.Add(this.Bt_Aide);
            this.Gb_Principal.Controls.Add(this.Bt_aff_val_fpga);
            this.Gb_Principal.Controls.Add(this.Bt_mesure_can);
            this.Gb_Principal.Controls.Add(this.L_can_quantum);
        }

        private void Init_Textbox_et_label(int xpos, int ypos)
        {
            int xpos2 = xpos + 124;
            int xpos3 = xpos + 165;
            int xpos4 = xpos + 210;
            int lypos = 3;

            this.L_titre_étalonnage = new LLabel("L_titre_étalonnage", xpos, ypos + lypos, "Étalonnage")
            {Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)))};
            this.L_entete_Mesure = new LLabel("L_entete_Mesure", xpos2, ypos + lypos, "Valeurs");
            this.L_entete_10Bits = new LLabel("L_entete_10Bits", xpos3, ypos + lypos, "10Bits");
            ypos += 21;
            this.L_valeur_étatlonage_max = new LLabel("L_valeur_étatlonage_max", xpos, ypos + lypos, "Max possible en entrée :");
            this.Tb_valeur_étalonnage_max = new TTextBox("tb_valeur_étalonnage_max", xpos2, ypos, "30");
            this.Tb_valeur_étalonnage_max_10bits = new TTextBox("tb_valeur_étalonnage_max_10bits", xpos3, ypos, "1023", 0, 1023, 1);
            ypos += 21;
            this.L_valeur_étatlonage_min = new LLabel("L_valeur_étatlonage_min", xpos, ypos + lypos, "min possible en entrée :");
            this.Tb_valeur_étalonnage_min = new TTextBox("Tb_valeur_étalonnage_min", xpos2, ypos, "-30");
            this.Tb_valeur_étalonnage_min_10bits = new TTextBox("Tb_valeur_étalonnage_min_10bits", xpos3, ypos, "0", 0, 1023, 1);
            ypos += 21;
            ypos = this.Init_objet_dépassement_seuil(xpos, xpos2, xpos3, xpos4, ypos);
            ypos = this.Init_objet_mesure(xpos, xpos2, xpos3, xpos4, ypos);

            this.L_can_quantum = new LLabel("L_can_quantum", xpos, ypos, "TODO");
        }

        private int Init_objet_dépassement_seuil(int xpos, int xpos2, int xpos3, int xpos4, int ypos)
        {
            this.L_titre_détection_dépassement_seuil = new LLabel("L_titre_détection_dépassement_seuil", xpos, ypos + 3, "Détection dépassement seuil")
            { Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) };
            ypos += 21;
            string type = "max";
            for (int i = 0; i < 8; i++)
            {
                this.Li_L_détection_valeur.Add(this.Init_label_dépassement_seuil(xpos, ypos + 3, type, "Valeur " + type + " CAN ", i));
                this.Li_détection_valeur.Add(this.Init_textbox(xpos2, ypos));
                this.Li_détection_valeur_10bits.Add(this.Init_textbox10bits(xpos3, ypos));
                this.Li_détection_valeur_validé.Add(Init_checkbox_dépassement_seuil(xpos4, ypos, "Activer limite"));
                ypos += 21;
                if (type == "max")
                    type = "min";
                else
                    type = "max";
            }
            return ypos;
        }

        private int Init_objet_mesure(int xpos, int xpos2, int xpos3, int xpos4, int ypos)
        {
            this.L_titre_mesure = new LLabel("L_titre_mesure", xpos, ypos + 3, "Mesure")
            { Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) };
            ypos += 21;
            for (int i = 0; i < 4; i++)
            {
                this.Li_L_mesure.Add(this.Init_label_dépassement_seuil(xpos, ypos + 3, "max", "Valeur max CAN ", i));
                this.Li_mesure.Add(this.Init_textbox(xpos2, ypos));
                this.Li_mesure_10bits.Add(this.Init_textbox10bits(xpos3, ypos));
                this.Li_mesure_activé.Add(Init_checkbox_dépassement_seuil(xpos4, ypos, "CAN activé"));
                ypos += 21;
            }
            return ypos;
        }

        private LLabel Init_label_dépassement_seuil(int xpos, int ypos, string type, string text,int index)
        {
            return new LLabel("", xpos, ypos, text + index.ToString() + " :");
        }

        private TTextBox Init_textbox(int xpos, int ypos)
        {
            return new TTextBox("", xpos, ypos, "0") { Enabled = false};
        }

        private TTextBox Init_textbox10bits(int xpos, int ypos)
        {
            return new TTextBox("", xpos, ypos, "512", 0, 1023, 1) { Enabled = false };
        }

        private CCheckBox Init_checkbox_dépassement_seuil(int xpos, int ypos, string text)
        {
            return new CCheckBox("" , xpos, ypos, false, text);
        }

        #endregion

        #region Code appelé par ses propres objets

        private void Active_les_textboxlié_chb_changed(object sender, EventArgs e)
        {
            int i = 0;
            foreach (CCheckBox chb in Li_détection_valeur_validé)
            {
                this.Li_détection_valeur[i].Enabled = chb.Checked;
                this.Li_détection_valeur_10bits[i].Enabled = chb.Checked;
                i++;
            }
            this.Nouvelle_donné(sender, e);
        }

        /// <summary>
        /// Indique qu'il y a eu un changement dans un élément
        /// </summary>
        public void Nouvelle_donné(object sender, EventArgs e)
        {
            this.A_changé = true;
        }

        #endregion

        #region extérieur

        /// <summary>
        /// Renvoi les donnée d'une manière compréhensible pour le datagrid
        /// </summary>
        /// <returns>liste de données</returns>
        public List<String> Récup_donné()
        {
            List<string> li_info = new List<string>();
            foreach (TTextBox tb in this.Li_détection_valeur_10bits)
            {
                li_info.Add(tb.Text);
            }
            string paramètre = "";
            foreach (CCheckBox chb in this.Li_détection_valeur_validé)
            {
                paramètre += chb.Checked ? "1" : "0";
            }
            paramètre += "00";
            li_info.Add(paramètre);
            this.A_changé = false;
            return li_info;
        }

        /// <summary>
        /// Génére les éléments nécéssaire pour la communication
        /// </summary>
        /// <param name="data_pour_FPGA">Le gestionaire de data</param>
        public int Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA, int index_de_départ)
        {
            this.Index_de_départ_du_DGV = index_de_départ;
            data_pour_FPGA.Add_Li_Datafpga("Valeur_max_avant_erreur_CAN1");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_min_avant_erreur_CAN1");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_max_avant_erreur_CAN2");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_min_avant_erreur_CAN2");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_max_avant_erreur_CAN3");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_min_avant_erreur_CAN3");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_max_avant_erreur_CAN4");
            data_pour_FPGA.Add_Li_Datafpga("Valeur_min_avant_erreur_CAN4");
            data_pour_FPGA.Add_Li_Datafpga("Paramètre des CAN");
            data_pour_FPGA.Add_Li_Datafpga("Valeur mesuré sur CAN1", false);
            data_pour_FPGA.Add_Li_Datafpga("Valeur mesuré sur CAN2", false);
            data_pour_FPGA.Add_Li_Datafpga("Valeur mesuré sur CAN3", false);
            data_pour_FPGA.Add_Li_Datafpga("Valeur mesuré sur CAN4", false);
            data_pour_FPGA.Add_Li_Datafpga("Quelle CAN à détecter une erreur", false);
            this.Nombre_dadresse = 14;
            return index_de_départ + this.Nombre_dadresse;
        }

        public void Lié_li_data(List<UneDataFPGA> data)
        {
            this.Li_data_du_dgv = data;
        }

        public void MAJ_Datafpga()
        {
            int index = this.Index_de_départ_du_DGV;
            List<String> li_data_donné_recup = this.Récup_donné();
            foreach (String str in li_data_donné_recup)
            {
                this.Li_data_du_dgv[index].Valeur = str;
                index++;
            }
        }

        #endregion

        /* TODO
         * 
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


                // Utiliser un backgroundworker pour demander les mesures des CANs
                private void Lancer_mesure()
                {
                    if (Etat_de_connection >= 11 && Etat_de_connection < 20)
                    {// connecté au serveur mais sans transfert en cour
                        Etat_de_connection = 22;

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
                        Etat_de_connection = 15;
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
                    Etat_de_connection = 13; // état de la mesure finie
                    //timer.Stop();   // on arrête le timer
                }
                */
    }
}
