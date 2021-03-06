﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Gestion_Connection_Carte_FPGA;
using Gestion_Objet;

namespace GeCoSwell
{
    class Sinus_FPGA : GroupBox, IGB_Spéciaux
    {
        private Label L_div_sinus;
        private TTextBox Tb_div_sinus;
        private TTextBox Tb_val_freq_sinus;
        private Label L_amplitude;
        private TTextBox Tb_amplitude;
        private TTextBox Tb_offset;
        private Label L_dephasage;
        private TTextBox Tb_dephasage;
        private TTextBox Tb_dephasage_en_pour;

        private string Index_consigne;

        public bool A_changé { get; private set; } = true;
        public bool EstVisible { get; private set; }
        public int Index_de_départ_du_DGV { get; private set; }
        public int Nombre_dadresse { get; private set; }
        public List<UneDataFPGA> Li_data_du_dgv { get; set; }

        #region Construteur

        /// <summary>
        /// Créer le groupBox et tout ses éléments
        /// </summary>
        /// <param name="xpos">position x du groupbox</param>
        /// <param name="ypos">position y du groupbox</param>
        public Sinus_FPGA(int xpos,int ypos, string index_consigne)
        {
            this.SuspendLayout();

            this.Index_consigne = index_consigne;

            this.Init_Label_et_Textbox(55, 13);
            this.Init_groupBox(xpos, ypos);

            this.Init_Lier_méthodes_aux_objets();

            this.ResumeLayout(false);
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Créer et place le groupbox
        /// </summary>
        /// <param name="xpos">position en X</param>
        /// <param name="ypos">position en Y</param>
        private void Init_groupBox(int xpos, int ypos)
        {
            this.Controls.Add(this.Tb_offset);
            this.Controls.Add(this.Tb_amplitude);
            this.Controls.Add(this.Tb_div_sinus);
            this.Controls.Add(this.Tb_val_freq_sinus);
            this.Controls.Add(this.Tb_dephasage);
            this.Controls.Add(this.Tb_dephasage_en_pour);
            this.Controls.Add(this.L_dephasage);
            this.Controls.Add(this.L_amplitude);
            this.Controls.Add(this.L_div_sinus);
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = "gb_sinus_fpga";
            this.Size = new System.Drawing.Size(221, 79);
            this.TabStop = false;
            this.Text = "Sinus FPGA";
            this.Visible = false;
            this.VisibleChanged += new EventHandler(Nouvelle_donné);
        }

        /// <summary>
        /// Place les Tabel et les textbox
        /// </summary>
        /// <param name="xpos">position en x du premier textbox</param>
        /// <param name="ypos">position en y du premier textbox</param>
        private void Init_Label_et_Textbox(int xpos, int ypos)
        {
            int lxpos = 10;
            int lypos = 3 ;
            int tb_xpos2 = xpos +105 ;

            this.L_div_sinus = new LLabel("l_div_sinus", lxpos, ypos + lypos, "Diviseur                 Fréquence                 Hz");
            this.Tb_div_sinus = new TTextBox("tb_div_sinus", xpos, ypos, "10", 1, 1023, 1);
            this.Tb_div_sinus.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_val_freq_sinus = new TTextBox("tb_val_freq_sinus", tb_xpos2, ypos, "4880",24, 24414,1);
            ypos += 21;
            lxpos = 6;
            this.L_amplitude = new LLabel("l_div_sinus", lxpos, ypos + lypos, "Amplitude                       OffSet");
            this.Tb_amplitude = new TTextBox("tb_amplitude", xpos, ypos, "1023",1,1023,1);
            this.Tb_amplitude.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_offset = new TTextBox("tb_offset", tb_xpos2, ypos, "512",1,1023,1);
            this.Tb_offset.TextChanged += new EventHandler(Nouvelle_donné);
            ypos += 21;
            lxpos = -2;
            this.L_dephasage = new LLabel("l_div_sinus", lxpos, ypos + lypos, "Déphasage                      En %");
            this.Tb_dephasage = new TTextBox("tb_div_sinus", xpos, ypos, "1023",0,1023,1);
            this.Tb_dephasage.TextChanged += new EventHandler(Nouvelle_donné);
            this.Tb_dephasage_en_pour = new TTextBox("tb_val_freq_sinus", tb_xpos2, ypos, "100",0,100,0.1);
        }

        /// <summary>
        /// Rajoute les méthodes au objets
        /// </summary>
        private void Init_Lier_méthodes_aux_objets()
        {
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_val_freq_sinus, this.Tb_div_sinus);
            TTextBox.Lier_ratio_entre_2textbox(this.Tb_dephasage, this.Tb_dephasage_en_pour);
        }

        #endregion

        #region Code appelé par ses propres objets

        #endregion

        /// <summary>
        /// Remet les valeur offset et amplitude au valeur d'origine
        /// </summary>
        public void Init_val_sinus()
        {
            this.Tb_offset.Text = "512";
            this.Tb_amplitude.Text = "1023";
        }
        
        #region Récupération des donné

        public void Change_Visibilité(bool visible)
        {
            this.Visible = visible;
            this.EstVisible = visible;
        }

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
            List<string> li_data = new List<string>()
            {
                this.Tb_div_sinus.Text,
                this.Tb_amplitude.Text,
                this.Calcul_minAmplitude(),
                this.Tb_dephasage.Text
            };

            this.A_changé = false;
            return li_data;
        }

        public int Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA, int index_de_départ)
        {
            this.Index_de_départ_du_DGV = index_de_départ;
            data_pour_FPGA.Add_Li_Datafpga("Diviseur de fréquence Consigne sinus FPGA " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("Amplitude " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("Valeur min du sinus " + this.Index_consigne);
            data_pour_FPGA.Add_Li_Datafpga("Position de départ Consigne sinus " + this.Index_consigne);
            this.Nombre_dadresse = 4;
            return index_de_départ + this.Nombre_dadresse;
        }

        public void Lié_li_data(List<UneDataFPGA> data)
        {
            this.Li_data_du_dgv = data;
        }

        public void MAJ_Datafpga()
        {
            if (this.A_changé)
            {
                MAJ_DataFPGA_boucle(this.Récup_donné());
            }

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

        private string Calcul_minAmplitude()
        {
            return (int.Parse(this.Tb_offset.Text) - ( int.Parse(this.Tb_amplitude.Text)/2)-1).ToString();
        }

        #endregion

    }
}