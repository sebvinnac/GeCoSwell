using Gestion_Connection_Carte_FPGA;
using Gestion_Objet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeCoSwell
{
    class Bras : IGB_Spéciaux
    {
        #region Liste des objets
        //Objet
        public GGroupBox Gb_Principal { get; private set; }
        private CCheckBox Chb_inv_bras;
        private CComboBox Cmb_cons_Bras;
        private LLabel L_valtri;
        private CComboBox Cmb_sens_tri;
        private LLabel L_min_bras;
        private LLabel L_max_bras;
        private TTextBox Tb_multinivmin;
        private TTextBox Tb_multinivmin_Pour;
        private TTextBox Tb_multinivmax_Pour;
        private TTextBox Tb_multinivmax;
        private TTextBox Tb_val_tri_init_Pour;
        private TTextBox Tb_val_tri_init;
        private LLabel L_départ;
        private LLabel L_l0bits;
        private LLabel L_enpour;
        private LLabel L_limite;

        #endregion

        //autre variable

        private static List<Bras> Li_bras = new List<Bras>();
        public static List<GGroupBox> Li_Gb_bras = new List<GGroupBox>();
        private int index_li_bras;

        public bool A_changé { get; private set; } = true;//indique qu'une valeur à changer
        public bool EstVisible { get; private set; }
        public int Index_de_départ_du_DGV { get; private set; }
        public int Nombre_dadresse { get; private set; }
        public List<UneDataFPGA> Li_data_du_dgv { get; private set; }

        private static int Max_général;

        public static ToolTip tooltip;

        #region Constructeur
        /// <summary>
        /// Génération des Gb de gestion de bras
        /// </summary>
        /// <param name="xpos">position en X</param>
        /// <param name="ypos">position en Y</param>
        public Bras(int xpos,int ypos,bool visible)
        {

            this.index_li_bras = Li_bras.Count + 1;
            this.Init_Textbox_et_label(47, 12);
            this.Init_Groupbox(xpos, ypos, visible);
            this.Init_tooltip();
            this.Init_Méthode_Lié_au_objet();

            Li_bras.Add(this);
            Li_Gb_bras.Add(this.Gb_Principal);
        }
        #endregion

        #region Initialisation

        /// <summary>
        /// Ajoute les méthodes au objets
        /// </summary>
        private void Init_Méthode_Lié_au_objet()
        {
            //Lié textbox ensemble
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_multinivmax, this.Tb_multinivmax_Pour);
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_multinivmin, this.Tb_multinivmin_Pour);
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_val_tri_init, this.Tb_val_tri_init_Pour);

            //Gestion nouvelle donné
            this.Tb_val_tri_init.Validated += new EventHandler(Nouvelle_donné);
            this.Tb_multinivmax.Validated += new EventHandler(Nouvelle_donné);
            this.Tb_multinivmin.Validated += new EventHandler(Nouvelle_donné);
            this.Chb_inv_bras.CheckedChanged += new EventHandler(Nouvelle_donné);
            this.Cmb_sens_tri.SelectedIndexChanged += new EventHandler(Nouvelle_donné);
            this.Cmb_cons_Bras.SelectedIndexChanged += new EventHandler(Nouvelle_donné);
            this.Gb_Principal.VisibleChanged += new EventHandler(Nouvelle_donné);

            //Gestion en cas de proximité d'init des limites
            this.Tb_multinivmax.Validated += new EventHandler(this.Test_proxi_change_sens_bt);
            this.Tb_multinivmin.Validated += new EventHandler(this.Test_proxi_change_sens_bt);
            this.Tb_val_tri_init.Validated += new EventHandler(this.Test_proxi_change_sens_bt);
            this.Cmb_sens_tri.SelectedIndexChanged += new EventHandler(this.Test_proxi_change_sens_bt);

            //méthode maj min et max
            this.Tb_multinivmax_Pour.Validated += new EventHandler(this.minmax_local_de_init_change);
            this.Tb_multinivmin_Pour.Validated += new EventHandler(this.minmax_local_de_init_change);

            //Gestion en cas de changement de taille
            this.Gb_Principal.Resize += new EventHandler(this.Gb_Déplacement_Resize);
        }
        
        /// <summary>
        /// Génére le tooltip et ses textes
        /// </summary>
        private void Init_tooltip()
        {

            // Create the ToolTip and associate with the Form container.
            tooltip = new ToolTip()
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

        /// <summary>
        /// Création du Groupbox au position demandé
        /// </summary>
        /// <param name="xpos">position en x</param>
        /// <param name="ypos">position en y</param>
        /// <param name="visible">indique si visible</param>
        private void Init_Groupbox(int xpos, int ypos, bool visible)
        {
            this.Gb_Principal = new GGroupBox()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new System.Drawing.Point(xpos, ypos),
                Visible = visible,
                Name = "gb_ActBras" + index_li_bras,
                TabStop = false,
                Text = "Bras " + index_li_bras
            };

            this.Gb_Principal.Controls.Add(this.L_limite);
            this.Gb_Principal.Controls.Add(this.L_enpour);
            this.Gb_Principal.Controls.Add(this.L_l0bits);
            this.Gb_Principal.Controls.Add(this.Tb_val_tri_init_Pour);
            this.Gb_Principal.Controls.Add(this.Chb_inv_bras);
            this.Gb_Principal.Controls.Add(this.Tb_val_tri_init);
            this.Gb_Principal.Controls.Add(this.Cmb_cons_Bras);
            this.Gb_Principal.Controls.Add(this.L_min_bras);
            this.Gb_Principal.Controls.Add(this.L_valtri);
            this.Gb_Principal.Controls.Add(this.L_max_bras);
            this.Gb_Principal.Controls.Add(this.Cmb_sens_tri);
            this.Gb_Principal.Controls.Add(this.Tb_multinivmin);
            this.Gb_Principal.Controls.Add(this.Tb_multinivmax);
            this.Gb_Principal.Controls.Add(this.Tb_multinivmin_Pour);
            this.Gb_Principal.Controls.Add(this.L_départ);
            this.Gb_Principal.Controls.Add(this.Tb_multinivmax_Pour);
        }

        /// <summary>
        /// Initialise les textbox et les label
        /// </summary>
        /// <param name="xpos">position en x</param>
        /// <param name="ypos">position en y</param>
        private void Init_Textbox_et_label(int xpos, int ypos)
        {
            int lxpos = 10;
            int lypos = 3;
            int Tb_xpos2 = xpos + 42;

            this.L_valtri = new LLabel("L_valtri", lxpos, ypos + lypos, "Valeur initiale du triangle");
            ypos += 21;
            this.L_l0bits = new LLabel("L_l0bits", xpos, ypos + lypos, "10Bits");
            this.L_enpour = new LLabel("L_enpour", Tb_xpos2, ypos + lypos, "En %");
            ypos += 21;
            this.L_départ = new LLabel("L_départ", lxpos, ypos + lypos, "Départ");
            this.Tb_val_tri_init = new TTextBox("Tb_tri", xpos, ypos, "512", 0, 1023, 1);
            this.Tb_val_tri_init_Pour = new TTextBox("Tb_tri_pour", Tb_xpos2, ypos, "50", 0, 100, 0.1);
            ypos += 21;
            this.L_limite = new LLabel("L_limite", xpos, ypos + lypos, "Limite");
            ypos += 21;
            this.L_max_bras = new LLabel("L_max_bras", lxpos, ypos + lypos, "Max :");
            this.Tb_multinivmax = new TTextBox("Tb_multinivmax", xpos, ypos, "1023", 0, 1023, 1);
            this.Tb_multinivmax_Pour = new TTextBox("Tb_multinivmax_Pour", Tb_xpos2, ypos, "100", 0, 100, 0.1);
            ypos += 21;
            this.L_min_bras = new LLabel("L_min_bras", lxpos, ypos + lypos, "Min :");
            this.Tb_multinivmin = new TTextBox("Tb_multinivmin", xpos, ypos, "0", 0, 1023, 1);
            this.Tb_multinivmin_Pour = new TTextBox("Tb_multinivmin_Pour", Tb_xpos2, ypos, "0", 0, 100, 0.1);
            ypos += 21;
            this.Chb_inv_bras = new CCheckBox("Chb_inv_bras", lxpos, ypos, false, "inversion bras haut-bas", false);
            ypos += 21;
            this.Cmb_sens_tri = new CComboBox("cmb_tri", lxpos, ypos, 116, new List<string>() {"Monté", "Descente" }, 0);
            ypos += 21;
            this.Cmb_cons_Bras = new CComboBox("Cmb_cons_Bras", lxpos, ypos, 116, new List<string>() { "Consigne 1", "Consigne 2" }, 0, false);
        }

        #endregion

        #region Code appelé par ses propres objets

        /// <summary>
        /// déplace les autre groupebox consigne
        /// appelé par un changement de size
        /// </summary>
        private void Gb_Déplacement_Resize(object sender, EventArgs e)
        {
            GGroupBox.Déplacement_gb_consigne(Li_Gb_bras, this.index_li_bras);
        }
        
        /// <summary>
        /// Indique qu'il y a eu un changement dans un élément
        /// </summary>
        public void Nouvelle_donné(object sender, EventArgs e)
        {
            this.A_changé = true;
        }

        /// <summary>
        /// Génére X groupbox bras
        /// </summary>
        /// <param name="xpos">position en x du premier</param>
        /// <param name="ypos">position en y du premier</param>
        /// <param name="nombredefois">nombre d'exemplaire voulu</param>
        /// <param name="nombredevisible">nombre d'exemplaire visible</param>
        /// <returns>une liste des bras</returns>
        public static List<Bras> Géné_x_consigne(int xpos, int ypos, int nombredefois, int nombredevisible)
        {
            List<Bras> li_gb = new List<Bras>();
            for (int i = 0; i < nombredefois; i++)
            {
                li_gb.Add(new Bras(xpos, ypos, i < nombredevisible));//créer un nouveua groupbox consigne les nombredevisible premier sont visible
                xpos = GGroupBox.Position_X_droite(li_gb[i].Gb_Principal);
            }
            return li_gb;
        }

        /// <summary>
        /// Met à jour le tooltip d'un textbox
        /// </summary>
        /// <param name="tb">Met à jour le tooltip lié au textbox</param>
        private void MAJ_tooltip(TTextBox tb)
        {
            tooltip.SetToolTip(tb, tb.GeneText_ToolTip());
        }

        #endregion

        #region extérieur

        public static void Changer_Max_Général(string val)
        {
            Max_général = int.Parse(val);
            double dmax_général = double.Parse(val);
            foreach (Bras bras in Li_bras)
            {
                bras.Tb_multinivmax.Valeur_Max_Théorique = dmax_général;
                bras.Tb_multinivmax.Tb_ratio_a_MAJ();
                bras.Tb_multinivmin.Valeur_Max_Théorique = dmax_général;
                bras.Tb_multinivmin.Tb_ratio_a_MAJ();
                bras.Changer_minmax_local_de_init();
            }
        }
        
        private void Changer_minmax_local_de_init()
        {
            this.Tb_val_tri_init.Valeur_Max_Théorique = this.Le_max_local();
            this.Tb_val_tri_init.Valeur_Min_Théorique = this.Le_min_local();
            this.Tb_val_tri_init.Tb_ratio_a_MAJ();
            this.Test_proxi_change_sens();
        }

        private void minmax_local_de_init_change(object sender, EventArgs e)
        {
            this.Changer_minmax_local_de_init();
        }

        /// <summary>
        /// Renvoi les donnée d'une manière compréhensible pour le datagrid
        /// </summary>
        /// <returns>liste de données</returns>
        public List<String> Récup_donné()
        {
            List<string> li_info = new List<string>()
            {
                Tb_val_tri_init.Text
            };
            li_info.Add(this.Le_max_local().ToString());
            li_info.Add(this.Le_min_local().ToString());
            //les autres paramètres
            string str = this.Gb_Principal.Visible ? "1" : "0";//renvoi 1 si visible
            str += this.Cmb_sens_tri.SelectedIndex;//sens du signe
            str += this.Chb_inv_bras.Checked ? "1" : "0"; //renvoi 1 si inversion haut/bas
            str += this.Cmb_sens_tri.Convertie_cmbindex_vers_stringbinaire(2);

            str += "00000";//espace libre pour plus tard
            li_info.Add(str);
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
            data_pour_FPGA.Add_Li_Datafpga("Valeur initial du triangle bras " + this.index_li_bras);
            data_pour_FPGA.Add_Li_Datafpga("Valeur max du triangle pour Bras " + this.index_li_bras);
            data_pour_FPGA.Add_Li_Datafpga("Valeur min du triangle pour Bras " + this.index_li_bras);
            data_pour_FPGA.Add_Li_Datafpga("Paramètre pour l'utilisation du Bras " + this.index_li_bras);
            this.Nombre_dadresse = 4;
            return index_de_départ + this.Nombre_dadresse;
        }

        public void Lié_li_data(List<UneDataFPGA> data)
        {
            this.Li_data_du_dgv = data;
        }

        public void MAJ_Datafpga()
        {
            MAJ_DataFPGA_boucle(this.Récup_donné());
        }

        private void MAJ_DataFPGA_boucle(List<String> li_str)
        {
            int index = this.Index_de_départ_du_DGV;
            foreach (string str in li_str)
            {
                this.Li_data_du_dgv[index].Valeur = str;
                index++;
            }
        }

        #endregion

        #region Gestion de proximité des limites

        private void Test_proxi_change_sens_bt(object sender, EventArgs e)
        {
            this.Test_proxi_change_sens();
        }

        /// <summary>
        /// Vérifie si la valeur initial du triangle est trop proche des bords
        /// et change le sens si c'est le cas
        /// </summary>
        private void Test_proxi_change_sens()
        {
            //test pour savoir si on est à 2 ou moins d'un changement de sens si oui
            //change le sens, car sinon il y aura un bug sur le FPGA
            if (int.Parse(this.Tb_val_tri_init.Text) >= Le_max_local() - 2)
            {
                this.Cmb_sens_tri.SelectedIndex = 1;//si trop proche en monté on passe en descente
            }
            else if (int.Parse(this.Tb_val_tri_init.Text) <= Le_min_local() + 2)
            {
                this.Cmb_sens_tri.SelectedIndex = 0;//si trop proche en monté on passe en descente
            }
        }

        private int Le_max_local()
        {
            return (this.Tb_multinivmax.Visible ? int.Parse(this.Tb_multinivmax.Text) : Max_général);
        }

        /// <summary>
        /// Retourne le minimum local suivant si multiniv est activé ou pas
        /// </summary>
        /// <returns>Le min local</returns>
        private int Le_min_local()
        {
            return (this.Tb_multinivmin.Visible ? int.Parse(this.Tb_multinivmin.Text) : 0);
        }

        #endregion
    }
}
