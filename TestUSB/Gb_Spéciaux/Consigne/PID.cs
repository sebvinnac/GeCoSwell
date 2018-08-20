using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataFPGA;
using Gestion_Objet;

namespace GeCoSwell
{
    class PID : GroupBox, IGB_Spéciaux
    {
        //Listes des objets
        private TTextBox Tb_kp;
        private TTextBox Tb_ki;
        private TTextBox Tb_kd;
        private List<TTextBox> L_Tb_k = new List<TTextBox>();
        private TTextBox Tb_PID_cible;
        private Label L_PID_kp = new Label();
        private Label L_PID_ki = new Label();
        private Label L_PID_kd = new Label();
        private Label L_PID_cible = new Label();
        private List<Label> L_PID = new List<Label>();
        private ComboBox Cmb_PID_type = new ComboBox();

        private static bool Gb_visible = false;
        private static List<PID> Liste_pid = new List<PID>();

        public bool A_changé { get; private set; } = true;
        public bool EstVisible { get; private set; }

        private string Index_consigne;

        #region constructeur
        public PID(int xpos,int ypos,string Index_consigne)
        {
            this.SuspendLayout();

            this.Index_consigne = Index_consigne;

            this.Init_combobox_et_environement(63, 15);
            this.Init_G_TextBox_et_Label(73,42);
            this.Init_Groupbox(xpos,ypos);
            this.Init_Lier_méthodes_aux_objets();
            this.Init_list();

            Liste_pid.Add(this);

            this.ResumeLayout(false);
        }
        #endregion

        #region initialisation
        private void Init_list()
        {
            this.L_Tb_k.Add(this.Tb_kp);
            this.L_Tb_k.Add(this.Tb_ki);
            this.L_Tb_k.Add(this.Tb_kd);
            this.L_Tb_k.Add(this.Tb_PID_cible);

            this.L_PID.Add(this.L_PID_kp);
            this.L_PID.Add(this.L_PID_ki);
            this.L_PID.Add(this.L_PID_kd);
            this.L_PID.Add(this.L_PID_cible);
        }

        /// <summary>
        /// Créer et place le groupbox
        /// </summary>
        /// <param name="xpos">position en X</param>
        /// <param name="ypos">position en Y</param>
        private void Init_Groupbox(int xpos, int ypos)
        {
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Tb_kp);
            this.Controls.Add(this.Tb_PID_cible);
            this.Controls.Add(this.Tb_kd);
            this.Controls.Add(this.Tb_ki);
            this.Controls.Add(this.L_PID_cible);
            this.Controls.Add(this.L_PID_kd);
            this.Controls.Add(this.Cmb_PID_type);
            this.Controls.Add(this.L_PID_ki);
            this.Controls.Add(this.L_PID_kp);
            this.Visible = Gb_visible;
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = "gb_PID";
            this.TabStop = false;
            this.Text = "PID";
        }

        /// <summary>
        /// Créer et place les label et G_TextBox du PID
        /// </summary>
        /// <param name="xpos">position en X de la cmb</param>
        /// <param name="ypos">position en Y de la cmb</param>
        private void Init_G_TextBox_et_Label(int xpos,int ypos)
        {
            int lypos = 3;
            int lxpos = xpos-26;
            this.Tb_kp = new TTextBox("Tb_kp", xpos, ypos, "512", 1,1023,1);
            this.L_PID_kp = new LLabel("l_PID_kp", lxpos, ypos + lypos, "kp");

            ypos += 21;
            this.Tb_ki = new TTextBox("Tb_ki", xpos, ypos, "972",1,1023,1);
            this.L_PID_ki = new LLabel("l_PID_ki", lxpos, ypos + lypos, "ki");

            ypos += 21;
            this.Tb_kd = new TTextBox("Tb_kd", xpos, ypos, "51",1,1023,1);
            this.L_PID_kd = new LLabel("l_PID_kd", lxpos, ypos + lypos, "kd");

            ypos += 21;
            lxpos -= 40;
            this.Tb_PID_cible = new TTextBox("Tb_PID_cible", xpos, ypos, "512",1,1023,1);
            this.L_PID_cible = new LLabel("L_PID_cible", lxpos, ypos + lypos, "Valeur cible :");
        }


        private void Init_Lier_méthodes_aux_objets()
        {
            this.VisibleChanged += new EventHandler(Nouvelle_donné);
            this.Tb_kp.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_ki.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_kd.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_PID_cible.TextChanged += new EventHandler(Nouvelle_donné);
            this.Cmb_PID_type.SelectedIndexChanged += new EventHandler(Nouvelle_donné);

            this.Cmb_PID_type.SelectedIndexChanged += new EventHandler(this.Cmb_PID_changed);
        }

        /// <summary>
        /// Créer et place la combobox et son label
        /// </summary>
        /// <param name="xpos">position en X de la cmb</param>
        /// <param name="ypos">position en Y de la cmb</param>
        private void Init_combobox_et_environement(int xpos, int ypos)
        {
            int lypos = +3;
            int lxpos = xpos - 57;
            this.L_PID_cible = new LLabel("l_PID_type", lxpos, lypos, "type de PID");
            this.Cmb_PID_type = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FormattingEnabled = true,
                Location = new System.Drawing.Point(xpos, ypos),
                Name = "cmb_PID_type",
                Size = new System.Drawing.Size(50, 21),
            };
            this.Cmb_PID_type.Items.AddRange(new object[] {
            "Rien",
            "P",
            "PI",
            "PID"});
            this.Cmb_PID_type.SelectedIndex = 3;
        }

        #endregion

        #region Code appelé par ses propres objets

        /// <summary>
        /// Affiche les labels et G_TextBox suivant le mode sélectionné
        /// </summary>
        private void Cmb_PID_changed(object sender, EventArgs e)
        {
            int nbaffiche = this.Cmb_PID_type.SelectedIndex;

            for (int i = 1; i < this.Cmb_PID_type.Items.Count; i++)
            {
                this.L_PID[i - 1].Visible = nbaffiche >= i;
                this.L_Tb_k[i - 1].Visible = nbaffiche >= i;
            }
        }
        #endregion

        #region Gestion Extérieur
        public void Change_visibilité(bool visible)
        {
            Change_visibilité_all(visible);
        }

        public static void Change_visibilité_all(bool visible)
        {
            foreach (PID pid in Liste_pid)
            {
                pid.Visible = visible;
            }
        }

        #endregion

        #region Récupération des donné

        /// <summary>
        /// Indique qu'il y a eu un changement dans un élément
        /// </summary>
        public void Nouvelle_donné(object sender, EventArgs e)
        {
            this.A_changé = true;
        }

        /// <summary>
        /// Renvoi les donnée d'une manière compréhensible pour le datagrid
        /// </summary>
        /// <returns>liste de données</returns>
        public List<string> Récup_donné()
        {
            List<string> li_data = new List<string>();
            int choix = this.Cmb_PID_type.SelectedIndex;
            if (this.CePID_inutile(choix))
            {
                li_data.Add("0");
                li_data.Add("0");
                li_data.Add("0");
                li_data.Add("0");
            }
            else
            {
                li_data.Add(this.Valeur_PID(choix, 0));
                li_data.Add(this.Valeur_PID(choix, 1));
                li_data.Add(this.Valeur_PID(choix, 2));
                li_data.Add(this.Tb_PID_cible.Text);
            }

            this.A_changé = false;
            return li_data;
        }

        private bool CePID_inutile(int choix)
        {
            return choix == 0 || this.EstVisible == false;
        }

        private string Valeur_PID(int choix,int index)
        {
            return choix > index ? L_Tb_k[index].Text : "0";
        }


        public int MAJ_Datafpga(List<UneDataFPGA> data, int index)
        {
            if (this.A_changé)
            {
                List<String> li_data = this.Récup_donné();
                data[index].Valeur = li_data[0];//kp
                data[index + 1].Valeur = li_data[1];//ki
                data[index + 2].Valeur = li_data[2];//kp
                data[index + 3].Valeur = li_data[3];//valeur cible
            }
            index += 4;

            return index;

        }

        public void Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA)
        {
            data_pour_FPGA.Add_Li_Datafpga("r0PID venant de kp " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("r1PID venant de ki " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("r2PID venant de kd " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("Valeur cible PID " + this.Index_consigne);
        }

        #endregion
    }
}
