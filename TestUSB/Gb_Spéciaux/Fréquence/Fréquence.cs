using DataFPGA;
using Gestion_Objet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeCoSwell
{
    class Fréquence : IGB_Spéciaux
    {
        #region Listes des objets
        //objet
        public GGroupBox Gb_Principal { get; private set; }
        private LLabel L_diviseur;
        private TTextBox Tb_freq_div;
        private LLabel L_freq_countmax;
        private TTextBox Tb_freq_countmax;
        private TTextBox Tb_fdec;
        private LLabel L_frequencedecoupage;
        private LLabel L_exp_calcul;
        private Bt_aide Bt_Aide;
        #endregion
        //autre variable

        public bool A_changé { get; private set; } = true;//indique qu'une valeur à changer
        public bool EstVisible { get; private set; }

        public ToolTip tooltip;

        #region Constructeur
        /// <summary>
        /// Génération des Gb de Double pulse
        /// </summary>
        /// <param name="xpos">position en X</param>
        /// <param name="ypos">position en Y</param>
        public Fréquence(int xpos, int ypos, bool visible)
        {

            this.Init_Textbox_et_label(139, 19);
            this.Init_Bt_aide(234, 80);
            this.Init_Groupbox(xpos, ypos, visible);
            this.Init_Lier_méthodes_aux_objets();
            this.EstVisible = visible;
            this.Init_tooltip();
        }


        #endregion

        #region Initialisation

        /// <summary>
        /// Ajoute les méthodes aux objets
        /// </summary>
        private void Init_Lier_méthodes_aux_objets()
        {
            this.Tb_fdec.Validated += new EventHandler(Nouvelle_donné);
            this.Tb_freq_div.Validated += new EventHandler(this.MAJ_Freq_dec);
            this.Tb_freq_countmax.Validated += new EventHandler(this.MAJ_Freq_dec);
            this.Tb_freq_countmax.Validated += new EventHandler(this.MAJ_Max_triangle_général);
            this.Gb_Principal.VisibleChanged += new EventHandler(this.Nouvelle_donné);
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

        /// <summary>
        /// Génére le tooltip et ses textes
        /// </summary>
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
                Name = "Fréquence",
                TabStop = false,
                Text = "Fréquence"
            };
            this.Gb_Principal.Controls.Add(this.L_diviseur);
            this.Gb_Principal.Controls.Add(this.Tb_freq_div);
            this.Gb_Principal.Controls.Add(this.L_freq_countmax);
            this.Gb_Principal.Controls.Add(this.Tb_freq_countmax);
            this.Gb_Principal.Controls.Add(this.Tb_fdec);
            this.Gb_Principal.Controls.Add(this.L_frequencedecoupage);
            this.Gb_Principal.Controls.Add(this.L_exp_calcul);
            this.Gb_Principal.Controls.Add(this.Bt_Aide);

        }

        /// <summary>
        /// Initialise les textbox et les label
        /// </summary>
        /// <param name="xpos">position en x</param>
        /// <param name="ypos">position en y</param>
        private void Init_Textbox_et_label(int xpos, int ypos)
        {
            int lxpos = 6;
            int lypos = 3;

            this.L_diviseur = new LLabel("L_diviseur", lxpos, ypos + lypos, "Diviseur de fréquence :");
            this.Tb_freq_div = new TTextBox("Tb_freq_div", xpos, ypos, "1", 1, 1023, 1);
            ypos += 21;
            this.L_freq_countmax = new LLabel("L_freq_countmax", lxpos, ypos + lypos, "Valeur max des triangles :");
            this.Tb_freq_countmax = new TTextBox("Tb_freq_countmax", xpos, ypos, "1023", 16, 1023, 1);
            ypos += 21;
            this.L_frequencedecoupage = new LLabel("L_frequencedecoupage", lxpos, ypos + lypos, "Fréquence de découpage :                 kHz");
            this.Tb_fdec = new TTextBox("Tb_fdec", xpos, ypos, "97,751") { Enabled = false };
            ypos += 21;
            this.L_exp_calcul = new LLabel("L_exp_calcul", lxpos, ypos + lypos, "Fdec = 100MHz / trianglemax  / Diviseur");


        }
        #endregion

        #region Code appelé par ses propres objets
        
        /// <summary>
        /// Calcule la fréquence de découpage
        /// </summary>
        private void MAJ_Freq_dec(object sender, EventArgs e)
        {
            this.Tb_fdec.Text = Chiffre.Significant((100000 / double.Parse(this.Tb_freq_div.Text) / double.Parse(this.Tb_freq_countmax.Text) / 2).ToString());
        }

        /// <summary>
        /// Indique qu'il y a eu un changement dans un élément
        /// </summary>
        public void Nouvelle_donné(object sender, EventArgs e)
        {
            this.A_changé = true;
        }

        /// <summary>
        /// Met à jour le max triangle général
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAJ_Max_triangle_général(object sender, EventArgs e)
        {
            Bras.Changer_Max_Général(this.Tb_freq_countmax.Text);
        }

        #endregion

        #region extérieur

        /// <summary>
        /// Renvoi les donnée d'une manière compréhensible pour le datagrid
        /// </summary>
        /// <returns>liste de données</returns>
        public List<String> Récup_donné()
        {
            List<string> li_info = new List<string>()
            {
                this.Tb_freq_div.Text,
                this.Tb_freq_countmax.Text
            };
            this.A_changé = false;
            return li_info;
        }

        /// <summary>
        /// Génére les éléments nécéssaire pour la communication
        /// </summary>
        /// <param name="data_pour_FPGA">Le gestionaire de data</param>
        public void Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA)
        {
            data_pour_FPGA.Add_Li_Datafpga("Diviseur de fréquence");
            data_pour_FPGA.Add_Li_Datafpga("Valeur Max des triangles");
        }

        public int MAJ_Datafpga(List<UneDataFPGA> data, int index)
        {
            if (this.A_changé && this.EstVisible)
            {
                List<String> li_data = this.Récup_donné();
                data[index].Valeur = li_data[0];//diviseur
                data[index + 1].Valeur = li_data[1];//countmax
            }
            return index + 2;
        }

        #endregion
    }
}
