using GeCoSwell;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gestion_Connection_Carte_FPGA
{
    class DataGridView_pour_FPGA : DataGridView
    {
        #region variable
        private DataGridViewTextBoxColumn Col_adresse;
        private DataGridViewTextBoxColumn Col_data;
        private DataGridViewTextBoxColumn Col_Explication;
        List<UneDataFPGA> Li_Datafpga = new List<UneDataFPGA>();
        #endregion

        #region Liste des data pour le fpga

        UneDataFPGA Numéro_carte;
        
        UneDataFPGA tb_val_63;//", Adresse = "0111111", Use = "MESURE_TAB_FPGA", Exp = "D Position de départ Consigne sinus 3"},
        UneDataFPGA tb_val_64;//", Adresse = "1000000", Use = "MESURE_TAB_FPGA", Exp = "D Position de départ Consigne sinus 3"},
        
        List<IGB_Spéciaux> Li_Gb_Spéciaux;
        #endregion

        #region Constructeur

        public DataGridView_pour_FPGA(int xpos, int ypos,List<IGB_Spéciaux> li_gb_spéciaux)
        {
            this.SuspendLayout();

            this.Li_Gb_Spéciaux = li_gb_spéciaux;
            this.Init_Collum();
            this.Init_DGV(xpos, ypos);
            this.Init_Data();
            this.MAJ_Data();

            this.Init_Row();

        }

        #endregion

        #region Initialisation

        private void Init_Row()
        {
            foreach (UneDataFPGA dt in this.Li_Datafpga)
            {

                this.Rows.Add(dt.Adresse, dt.Valeur, dt.Utilité_de_cette_valeur);

            }
        }

        private void Init_Data()
        {
            this.Numéro_carte = this.Add_Li_Datafpga("Numéro_de_carte", typeasend: false);

            int index = 1;
            foreach (IGB_Spéciaux gb in Li_Gb_Spéciaux)
            {
                index = gb.Init_Datafpga(this,index);
            }

            //--------------
            //mode multiconsigne
            //--------------
            this.tb_val_63 = this.Add_Li_Datafpga("");
            this.tb_val_64 = this.Add_Li_Datafpga("");
        }
        #endregion

        public UneDataFPGA Add_Li_Datafpga(string explication,bool typeasend = true)
        {
            UneDataFPGA data;
            string adresse = Convert.ToString(this.Li_Datafpga.Count, 2);
            while (adresse.Length < 8)//rajoute les 0 qui manque a gauche si il en manque
            {
                adresse = "0" + adresse;
            }

            Li_Datafpga.Add(data = new UneDataFPGA(adresse, explication, typeasend));
            return data;
        }

        #region Initialisation

        private void Init_Collum()
        {
            this.Col_adresse = new DataGridViewTextBoxColumn();
            this.Col_data = new DataGridViewTextBoxColumn();
            this.Col_Explication = new DataGridViewTextBoxColumn();


            this.Col_adresse.HeaderText = "Adresse";
            this.Col_adresse.Name = "Adresse";
            this.Col_adresse.ReadOnly = true;
            this.Col_data.HeaderText = "Valeur";
            this.Col_data.Name = "Valeur";
            this.Col_data.ReadOnly = false;
            this.Col_Explication.HeaderText = "Explication";
            this.Col_Explication.Name = "Explication";
            this.Col_Explication.ReadOnly = true;
        }

        private void Init_DGV(int xpos, int ypos)
        {
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Columns.AddRange(new DataGridViewColumn[] { this.Col_adresse,this.Col_data, this.Col_Explication});
            this.AutoGenerateColumns = true;
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = "dataGridView";
            this.Size = new System.Drawing.Size(400, 400);
            this.Dock = DockStyle.Fill;
            this.AllowUserToAddRows = false;

        }

        
        public void MAJ_Data()
        {
            foreach (IGB_Spéciaux gb in Li_Gb_Spéciaux)
            {
                gb.Lié_li_data(Li_Datafpga);
            }
        }


        #endregion
    }
}
