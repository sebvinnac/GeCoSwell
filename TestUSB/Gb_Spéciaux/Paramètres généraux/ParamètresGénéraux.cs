using DataFPGA;
using Gestion_Objet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeCoSwell
{
    class ParamètresGénéraux : IGB_Spéciaux
    {
        //Objet
        public GGroupBox Gb_Principal { get; private set; }
        private Bt_aide Bt_Aide;
        private CCheckBox Chb_Rouecodeuse;
        private TTextBox Tb_Nbbras;
        private LLabel L_Nbbras;
        private TTextBox Tb_tpsmort;
        private LLabel L_tpsmort;
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
        public ParamètresGénéraux(int xpos, int ypos, bool visible)
        {

            this.Init_Textbox_et_label(146, 20);
            this.Init_Bt_aide(245, 80);
            this.Init_Groupbox(xpos, ypos, visible);
            this.Init_Lier_méthodes_aux_objets();
            this.EstVisible = visible;
            this.Init_tooltip();



        }

        /// <summary>
        /// Ajoute les méthodes aux objets
        /// </summary>
        private void Init_Lier_méthodes_aux_objets()
        {
            this.Gb_Principal.VisibleChanged += new EventHandler(this.Nouvelle_donné);
            this.Chb_Rouecodeuse.CheckedChanged += new EventHandler(this.Nouvelle_donné);
            this.Tb_tpsmort.Validated += new EventHandler(this.Nouvelle_donné);
            this.Tb_Nbbras.TextChanged += new EventHandler(this.GestionPluriel);
        }

        private void Init_Bt_aide(int xpos, int ypos)
        {
            this.Bt_Aide = new Bt_aide(xpos, ypos);
        }

        #endregion

        #region Initialisation

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
                Name = "Paramètresgénéraux",
                TabStop = false,
                Text = "Paramètres généraux"
            };
            this.Gb_Principal.Controls.Add(this.Bt_Aide);
            this.Gb_Principal.Controls.Add(this.Chb_Rouecodeuse);
            this.Gb_Principal.Controls.Add(this.Tb_Nbbras);
            this.Gb_Principal.Controls.Add(this.L_Nbbras);
            this.Gb_Principal.Controls.Add(this.Tb_tpsmort);
            this.Gb_Principal.Controls.Add(this.L_tpsmort);
        }

        private void Init_Textbox_et_label(int xpos, int ypos)
        {
            int lxpos = 6;
            int lypos = 3;


            this.Chb_Rouecodeuse = new CCheckBox("Chb_Rouecodeuse", lxpos, ypos, false, "Utiliser roue codeuse pour temps mort");
            ypos += 21;
            this.Tb_tpsmort = new TTextBox("Tb_tpsmort", xpos, ypos, "100", 0, 2555, 5);
            this.L_tpsmort = new LLabel("L_Pulse_t_1", lxpos, ypos + lypos, "Temps mort par pas de 5ns :                  ns");
            ypos += 21;
            this.L_Nbbras = new LLabel("L_Nbbras", lxpos, ypos + lypos, "Nombre de bras actifs :");
            this.Tb_Nbbras = new TTextBox("Tb_Nbbras", xpos, ypos, "2", 0, 4, 1) { Enabled = false };
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
        /// Permet de rajotuer le s au label
        /// </summary>
        private void GestionPluriel(object sender, EventArgs e)
        {
            this.L_Nbbras.Text = "Nombre de bras actif" + (int.Parse(this.Tb_Nbbras.Text) > 1 ? "s" : "") + " :";
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
                (this.Chb_Rouecodeuse.Checked ? "1" : "0") + Méthod_outil.Outil_text.StringDec_versStringBinaire(this.Tb_tpsmort.Text, 9),
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
            data_pour_FPGA.Add_Li_Datafpga("{ Use RoueCodeuse (1bits); Retard (9bits)}");
        }

        public int MAJ_Datafpga(List<UneDataFPGA> data, int index)
        {
            if (this.A_changé)//si il y a eu du changement et si visible
            {
                List<String> li_data = this.Récup_donné();
                data[index].Valeur = li_data[0];//diviseur
            }
            return index + 1;
        }

            #endregion
        }
}
