using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Gestion_Connection_Carte_FPGA;
using Gestion_Objet;

namespace GeCoSwell
{
    class Consigne : IGB_Spéciaux
    {

        #region liste des objets
        //Liste des objets
        public GGroupBox Gb_Principal { get; private set; }
        private Bt_aide Bt_aide_cons;
        private TTextBox Tb_consigne;
        private TTextBox Tb_con_pour;
        private TTextBox Tb_con_min;
        private TTextBox Tb_con_min_pour;
        private TTextBox Tb_con_max;
        private TTextBox Tb_con_max_pour;
        private CComboBox Cmb_type_consigne;
        private CComboBox Cmb_consigne_CAN;
        private LLabel L_pour;
        private LLabel L_10bits;
        private LLabel L_con_max;
        private LLabel L_con_min;
        private LLabel L_consigne_CAN;
        #endregion

        //Listes des autre groupebox
        public PID Gb_Pid;
        public Sinus_FPGA Gb_Sinus_fpga;

        public static ToolTip tooltip;

        private static List<Consigne> Li_consigne = new List<Consigne>();
        public static List<GGroupBox> Li_Gb_consigne = new List<GGroupBox>();
        private int Index_consigne = 0;

        public bool A_changé { get; private set; } = true;//indique qu'une valeur à changer
        public bool EstVisible { get; private set; }
        public int Index_de_départ_du_DGV { get; private set; }
        public int Nombre_dadresse { get; private set; }
        public List<UneDataFPGA> Li_data_du_dgv { get; private set; }


        #region constructeur

        /// <summary>
        /// Créer le GroupBox consigne et tout ses éléments
        /// </summary>
        /// <param name="xpos">position en x du groupebox</param>
        /// <param name="ypos">position en y du groupebox</param>
        /// <param name="visible">si il est visible</param>
        public Consigne(int xpos, int ypos, bool visible)
        {

            this.Index_consigne = Li_consigne.Count + 1;

            this.Init_Label_et_Textbox(105, 13);
            this.Init_AutreGroupBox();
            this.Init_Bt_Aide(6, 60);
            this.Init_groupBox(xpos, ypos, visible);
            this.Inittooltip();

            this.Init_Lier_méthodes_aux_objets();

            Li_consigne.Add(this);
            Li_Gb_consigne.Add(this.Gb_Principal);

        }


        /// <summary>
        /// Génére X groupbox consigne
        /// </summary>
        /// <param name="xpos">position en x du premier</param>
        /// <param name="ypos">position en y du premier</param>
        /// <param name="nombredefois">nombre d'exemplaire voulu</param>
        /// <param name="nombredevisible">nombre d'exemplaire visible</param>
        /// <returns>une liste des consigne</returns>
        public static List<Consigne> Créer_x_fois_consigne(int xpos, int ypos, int nombredefois, int nombredevisible)
        {
            List<Consigne> li_gb = new List<Consigne>();
            for (int i = 0; i < nombredefois; i++)
            {
                li_gb.Add(new Consigne(xpos, ypos, i < nombredevisible));//créer un nouveua groupbox consigne les nombredevisible premier sont visible
                xpos = GGroupBox.Position_X_droite(li_gb[i].Gb_Principal);
            }
            return li_gb;
        }

        #endregion

        #region Initialisation

        private void Init_Bt_Aide(int xpos, int ypos)
        {
            this.Bt_aide_cons = new Bt_aide(xpos, ypos);
            this.Bt_aide_cons.Click += new EventHandler(this.Ouvre_Aide);
        }

        private void Init_groupBox(int xpos, int ypos, bool visible)
        {
            this.Gb_Principal = new GGroupBox()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new System.Drawing.Point(xpos, ypos),
                Name = "gb_consigne",
                TabStop = false,
                Text = "Consigne " + this.Index_consigne,
                Visible = visible

            };
            this.EstVisible = visible;

            this.Gb_Principal.Controls.Add(this.L_pour);
            this.Gb_Principal.Controls.Add(this.L_10bits);
            this.Gb_Principal.Controls.Add(this.Cmb_consigne_CAN);
            this.Gb_Principal.Controls.Add(this.Tb_consigne);
            this.Gb_Principal.Controls.Add(this.Tb_con_pour);
            this.Gb_Principal.Controls.Add(this.Tb_con_min);
            this.Gb_Principal.Controls.Add(this.Bt_aide_cons);
            this.Gb_Principal.Controls.Add(this.L_consigne_CAN);
            this.Gb_Principal.Controls.Add(this.Tb_con_max);
            this.Gb_Principal.Controls.Add(this.Cmb_type_consigne);
            this.Gb_Principal.Controls.Add(this.Tb_con_min_pour);
            this.Gb_Principal.Controls.Add(this.L_con_max);
            this.Gb_Principal.Controls.Add(this.Tb_con_max_pour);
            this.Gb_Principal.Controls.Add(this.L_con_min);

            this.Gb_Principal.Controls.Add(this.Gb_Pid);
            this.Gb_Principal.Controls.Add(this.Gb_Sinus_fpga);

        }

        private void Init_Label_et_Textbox(int xpos,int ypos)
        {
            int lxpos = 2;
            int lypos = 3;
            int xpos2 = xpos + 46;

            this.L_10bits = new LLabel("L_10bits", lxpos + xpos, ypos, "10Bits");
            this.L_pour = new LLabel("L_pour", lxpos + xpos2, ypos, "En %");
            ypos += 21;

            this.Cmb_type_consigne = new CComboBox("Cmb_type_consigne",xpos -100,ypos-2,91,
                new List<string>() {"continue ===>", "Sinus FPGA", "extérieur"},0);
            this.Tb_consigne = new TTextBox("Tb_consigne", xpos, ypos, "512", 0, 1023, 1);
            this.Tb_con_pour = new TTextBox("Tb_con_pour", xpos2, ypos, "50", 0, 100, 0.1);

            lxpos = 30;
            ypos += 21;
            this.L_con_max = new LLabel("L_con_max", lxpos, ypos + lypos, "Consigne max");
            this.Tb_con_max = new TTextBox("Tb_con_max", xpos, ypos, "972", 0, 1023, 1);
            this.Tb_con_max_pour = new TTextBox("Tb_con_max_pour", xpos2, ypos, "95", 0, 100, 0.1);

            lxpos = 30;
            ypos += 21;
            this.L_con_min = new LLabel("L_con_min", lxpos, ypos + lypos, "Consigne min");
            this.Tb_con_min = new TTextBox("Tb_con_min", xpos, ypos, "51", 0, 1023, 1);
            this.Tb_con_min_pour = new TTextBox("Tb_con_min_pour", xpos2, ypos, "5", 0, 100, 0.1);
            lxpos = 6;
            xpos += -4;
            ypos += 26;
            this.L_consigne_CAN = new LLabel("L_consigne_CAN", lxpos, ypos + lypos, "Consigne d'origine :") { Visible = false };
            this.Cmb_consigne_CAN = new CComboBox("Cmb_type_consigne", xpos, ypos, 91,
                new List<string>() { "CAN1", "CAN2", "CAN3", "CAN4" }, 0){ Visible = false};
        }

        private void Ouvre_Aide(object sender, EventArgs e)
        {
            aide.Aide_Général AG = new aide.Aide_Général(this.Gb_Principal);
            AG.Show();
        }

        private void Init_AutreGroupBox()
        {
            this.Gb_Pid = new PID(225, 6, Index_consigne.ToString());
            this.Gb_Pid.Change_visibilité(false);
            this.Gb_Sinus_fpga = new Sinus_FPGA(0, 98,Index_consigne.ToString()) { Visible = false };
        }

        /// <summary>
        /// Génére le tooltip et ses textes
        /// </summary>
        private void Inittooltip()
        {
            tooltip = new ToolTip()
            {
                InitialDelay = 100,
                ReshowDelay = 100,
                AutoPopDelay = 5000,
                IsBalloon = true
            };
            foreach (TTextBox tb in GestionObjet.Trouver_controls_dun_type(this.Gb_Principal, typeof(TTextBox)))
            {
                tooltip.SetToolTip(tb,tb.GeneText_ToolTip());
            }
        }

        /// <summary>
        /// Met en place les méthodes des objets
        /// </summary>
        private void Init_Lier_méthodes_aux_objets()
        {
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_consigne, this.Tb_con_pour);
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_con_max, this.Tb_con_max_pour);
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_con_min, this.Tb_con_min_pour);
            this.Cmb_type_consigne.SelectedIndexChanged += new EventHandler(this.Cmb_type_consigne_changed);

            this.Cmb_type_consigne.SelectedIndexChanged += new EventHandler(Nouvelle_donné);
            this.Tb_consigne.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_con_max.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_con_min.TextChanged += new EventHandler(Nouvelle_donné);
            this.Cmb_consigne_CAN.SelectedIndexChanged += new EventHandler(Nouvelle_donné);


            this.Gb_Principal.Resize += new EventHandler(this.Gb_Déplacement_Resize);
        }

        #endregion

        #region Code appelé par ses propres objets

        /// <summary>
        /// Indique qu'il y a eu un changement dans un élément
        /// </summary>
        public void Nouvelle_donné(object sender, EventArgs e)
        {
            this.A_changé = true;
        }

        /// <summary>
        /// Gére l'affichage des éléments suivant le type de consigne
        /// </summary>
        private void Cmb_type_consigne_changed(object sender, EventArgs e)
        {
            int choix = this.Cmb_type_consigne.SelectedIndex;
            this.Tb_consigne.Visible = (choix == 0);//si choix 1 = 0 alors visible sinon caché
            this.Tb_con_pour.Visible = (choix == 0);//si choix 1 = 0 alors visible sinon caché
            this.Gb_Sinus_fpga.Change_Visibilité(choix == 1);
            this.Cmb_consigne_CAN.Visible = (choix == 2);//si choix 1 = 2 alors visible sinon caché
            this.L_consigne_CAN.Visible = this.Cmb_consigne_CAN.Visible;

            this.Gb_Sinus_fpga.Init_val_sinus();
            
        }

        /// <summary>
        /// déplace les autre groupebox consigne
        /// appelé par un changement de size
        /// </summary>
        private void Gb_Déplacement_Resize(object sender, EventArgs e)
        {
            GGroupBox.Déplacement_gb_consigne(Li_Gb_consigne, Index_consigne);
        }

        #endregion

        #region Gestion extérieur

        public void Change_Visibilité_consigne(bool visible)
        {
            this.Gb_Principal.Visible = visible;
            this.EstVisible = visible;
        }


        public static void Change_Visibilité_Pid(bool état)
        {
            foreach (Consigne gb in Li_consigne)
            {
                gb.Gb_Pid.Change_Visibilité(état);
            }
        }
        
        private static void Change_combien_sont_visible(int nombre_visible)
        {
            for (int i = 0; i < Li_consigne.Count; i++)
            {
                Li_consigne[i].Change_Visibilité_consigne(i < nombre_visible);
            }
        }


        public static void Lié_position_dessous(List<GGroupBox> l_gb2)
        {
            Li_Gb_consigne[0].Lié_position_y_deux_gb(Li_Gb_consigne, l_gb2);
        }
        #endregion

        #region Récup donné

        public List<String> Récup_donné()
        {
            List<String> li_data = new List<string>()
            {
                this.Tb_consigne.Text,
                this.Tb_con_max.Text,
                this.Tb_con_min.Text
            };
            if(!this.EstlePremier_consigne())
                this.A_changé = false;
            return li_data;
        }

        /// <summary>
        /// Récupére en mise en forme les paramètres consigne
        /// </summary>
        /// <returns>9 caractère mise en forme pour le datagrid</returns>
        internal static string Récup_Paramètre()
        {
            String str = "";
            foreach (Consigne consigne in Li_consigne)
            {
                str += consigne.Paramètre_Miseenforme();
            }
            return str;
        }

        /// <summary>
        /// Récupére en mise en forme les paramètres consigne
        /// </summary>
        /// <returns>3 caractère mise en forme pour le datagrid</returns>
        private string Paramètre_Miseenforme()
        {
            string str = "";
            switch (this.Cmb_type_consigne.SelectedIndex)
            {//mise en forme de la textbox en signal compréhensible par la carte FPGA
                case 0:
                    str = "100";
                    break;
                case 1:
                    str = "101";
                    break;
                case 2:
                    str = "0" + this.Cmb_consigne_CAN.Convertie_cmbindex_vers_stringbinaire();
                    break;
            }
            return str;
        }

        /// <summary>
        /// Génére les éléments nécéssaire pour la communication
        /// </summary>
        /// <param name="data_pour_FPGA">Le gestionaire de data</param>
        public int Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA, int index_de_départ)
        {
            this.Index_de_départ_du_DGV = index_de_départ;
            if (this.EstlePremier_consigne())
            {
                data_pour_FPGA.Add_Li_Datafpga("Paramètre commun à toute les consigne");
                index_de_départ++;
            }
            data_pour_FPGA.Add_Li_Datafpga("Consigne " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("Consigne Max " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("Consigne Min " + this.Index_consigne);
            index_de_départ = this.Gb_Sinus_fpga.Init_Datafpga(data_pour_FPGA, index_de_départ + 3);
            index_de_départ = this.Gb_Pid.Init_Datafpga(data_pour_FPGA, index_de_départ);
            this.Nombre_dadresse = index_de_départ - this.Index_de_départ_du_DGV;
            return index_de_départ;
        }

        private bool EstlePremier_consigne()
        {
            return this.Index_consigne == 1;
        }

        public void Lié_li_data(List<UneDataFPGA> data)
        {
            this.Li_data_du_dgv = data;
            this.Gb_Sinus_fpga.Li_data_du_dgv = data;
            this.Gb_Pid.Li_data_du_dgv = data;
        }

        public void MAJ_Datafpga()
        {
            MAJ_DataFPGA_boucle(this.Récup_donné());
            this.Gb_Sinus_fpga.MAJ_Datafpga();
            this.Gb_Pid.MAJ_Datafpga();
        }

        private void MAJ_DataFPGA_boucle(List<String> li_str)
        {
            int index = this.Index_de_départ_du_DGV;
            if (this.EstlePremier_consigne())
            {
                this.Li_data_du_dgv[index].Valeur = Consigne.Récup_Paramètre() + "à faire";//TODO + (surmodulation);
                index++;
            }
            foreach (string str in li_str)
            {
                this.Li_data_du_dgv[index].Valeur = str;
                index++;
            }
        }

        public static void Changement_mode_de_consigne(int mode)
        {
            /* Rappel
            Boucle ouverte = 0
            Boucle fermé = 1
            Opposition = 2
            Mode Triphasé = 3
            Complet = 4
            */
            switch (mode)
            {
                case 0:
                    Change_combien_sont_visible(1);
                    Change_Visibilité_Pid(false);
                    break;
                case 1:
                    Change_combien_sont_visible(1);
                    Change_Visibilité_Pid(true);
                    break;
                case 2:
                    Change_combien_sont_visible(2);
                    Change_Visibilité_Pid(false);
                    break;
                case 3:
                    Change_combien_sont_visible(3);
                    Change_Visibilité_Pid(false);
                    break;
                default:
                    Change_combien_sont_visible(3);
                    Change_Visibilité_Pid(true);
                    break;
            }
        }

        #endregion

    }
}
