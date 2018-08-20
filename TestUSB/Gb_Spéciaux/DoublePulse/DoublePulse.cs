using DataFPGA;
using Gestion_Objet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeCoSwell
{
    class DoublePulse : IGB_Spéciaux
    {


        //Objet
        public GGroupBox Gb_Principal { get; private set; }
        private Bt_aide Bt_Aide;
        private CComboBox Cmb_Choix_DP_Transistor;
        private LLabel L_choix_dp_transistor;
        private TTextBox Tb_bras_DP;
        private TTextBox Tb_db_T_Pulse3;
        private TTextBox Tb_db_T_Pulse1;
        private LLabel L_Pulse_t_1;
        private LLabel L_Pulse_t_2;
        private LLabel L_chx_dp;
        private LLabel L_Pulse_t_3;
        private TTextBox Tb_db_T_Pulse2;

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
        public DoublePulse(int xpos, int ypos, bool visible)
        {

            this.Init_Textbox_et_label(6, 20);
            this.Init_Bt_aide(245, 105);
            this.Init_Groupbox(xpos, ypos, visible);
            this.Init_Lier_méthodes_aux_objets();
            this.EstVisible = visible;
            this.Init_tooltip();
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
                Name = "Mode double pulse",
                TabStop = false,
                Text = "Mode double pulse"
            };

            this.Gb_Principal.Controls.Add(this.Bt_Aide);
            this.Gb_Principal.Controls.Add(this.Cmb_Choix_DP_Transistor);
            this.Gb_Principal.Controls.Add(this.L_choix_dp_transistor);
            this.Gb_Principal.Controls.Add(this.Tb_bras_DP);
            this.Gb_Principal.Controls.Add(this.Tb_db_T_Pulse3);
            this.Gb_Principal.Controls.Add(this.Tb_db_T_Pulse1);
            this.Gb_Principal.Controls.Add(this.L_Pulse_t_1);
            this.Gb_Principal.Controls.Add(this.L_chx_dp);
            this.Gb_Principal.Controls.Add(this.L_Pulse_t_3);
            this.Gb_Principal.Controls.Add(this.Tb_db_T_Pulse2);
            this.Gb_Principal.Controls.Add(this.L_Pulse_t_2);

        }

        private void Init_Textbox_et_label(int xpos, int ypos)
        {
            int lxpos = 50;
            int lypos = 3;

            this.Tb_db_T_Pulse1 = new TTextBox("Tb_db_T_Pulse1", xpos, ypos, "4,5", 0.1, 102.3, 0.1);
            this.L_Pulse_t_1 = new LLabel("L_Pulse_t_1", lxpos, ypos + lypos, "µs T1 : Durée de la première impulsion");
            ypos += 21;
            this.Tb_db_T_Pulse2 = new TTextBox("Tb_db_T_Pulse2", xpos, ypos, "7,5", 0.1, 102.3, 0.1);
            this.L_Pulse_t_2 = new LLabel("L_Pulse_t_2", lxpos, ypos + lypos, "µs T2 : Durée du temps de repos");
            ypos += 21;
            this.Tb_db_T_Pulse3 = new TTextBox("Tb_db_T_Pulse3", xpos, ypos, "0,9", 0.1, 102.3, 0.1);
            this.L_Pulse_t_3 = new LLabel("L_Pulse_t_3", lxpos, ypos + lypos, "µs T3 : Durée de la seconde impulsion");
            ypos += 21;
            this.Cmb_Choix_DP_Transistor = new CComboBox("Cmb_Choix_DP_Transistor", xpos, ypos, 116, new List<string>() { "Bas", "Haut" }, 0);
            this.L_choix_dp_transistor = new LLabel("L_choix_dp_transistor", lxpos + 76, ypos + lypos , "Choix du transistor affecté");
            ypos += 21;
            this.Tb_bras_DP = new TTextBox("Tb_bras_DP", xpos, ypos, "1", 1, 4, 1);
            this.L_chx_dp = new LLabel("L_chx_dp", lxpos, ypos + lypos, "Choix du bras testé");

        }

        private void Init_Lier_méthodes_aux_objets()
        {
            this.Tb_db_T_Pulse1.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_db_T_Pulse2.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_db_T_Pulse3.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_bras_DP.TextChanged += new EventHandler(Nouvelle_donné);
            this.Cmb_Choix_DP_Transistor.SelectedIndexChanged += new EventHandler(Nouvelle_donné);

            this.Gb_Principal.VisibleChanged += new EventHandler(this.Nouvelle_donné);
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

        #endregion

        #region extérieur


        /// <summary>
        /// Change la visibilité du groupbox
        /// </summary>
        /// <param name="visible">Nouvelle état de visibilité</param>
        public void Change_Visibilité(bool visible)
        {
            this.Gb_Principal.Visible = visible;
            this.EstVisible = visible;
        }

        /// <summary>
        /// Renvoi les donnée d'une manière compréhensible pour le datagrid
        /// </summary>
        /// <returns>liste de données</returns>
        public List<String> Récup_donné()
        {
            List<string> li_info = new List<string>();
            //tempo dp
            li_info.Add(this.Tb_db_T_Pulse1.Text);
            li_info.Add(this.Tb_db_T_Pulse2.Text);
            li_info.Add(this.Tb_db_T_Pulse3.Text);
            string str;
            if (EstVisible)
            {
                str = this.Cmb_Choix_DP_Transistor.Convertie_cmbindex_vers_stringbinaire();
                switch (this.Tb_bras_DP.Text)
                {
                    case "1":
                        str += "1000";
                        break;
                    case "2":
                        str += "0100";
                        break;
                    case "3":
                        str += "0010";
                        break;
                    case "4":
                        str += "0001";
                        break;
                    default:
                        break;
                }

                str += "00000";
            }
            else
            {
                str = "0000000000";
            }
            li_info.Add(str);
            this.A_changé = false;
            return li_info;
        }


        public void Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA)
        {
            data_pour_FPGA.Add_Li_Datafpga("T1 : Durée de la première impulsion");
            data_pour_FPGA.Add_Li_Datafpga("T2 : Durée du temps de repos");
            data_pour_FPGA.Add_Li_Datafpga("µs T3 : Durée de la seconde impulsion");
            data_pour_FPGA.Add_Li_Datafpga("Paramètre pour double pulse");
        }

        public int MAJ_Datafpga(List<UneDataFPGA> data, int index)
        {
            if (this.A_changé)//si il y a eu du changement sur ce bras
            {
                List<String> li_data = this.Récup_donné();
                data[index].Valeur = li_data[0];//pulse T1
                data[index + 1].Valeur = li_data[1];//T2
                data[index + 2].Valeur = li_data[2];//T3
                data[index + 3].Valeur = li_data[3];// param
            }
            return index + 4;
        }

            #endregion
        }
}
