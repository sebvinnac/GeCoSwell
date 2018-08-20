using GeCoSwell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFPGA
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
/*
            // Définition du nombre de colonnes
            this.dataGridView1.ColumnCount = 4;
            // On nomme les colonnes (si on veut)
            dataGridView1.Columns[0].Name = "Col1";
            dataGridView1.Columns[1].Name = "Col2";
            dataGridView1.Columns[2].Name = "Col3";

            
            //On peut même insérer à la ligne que l'on souhaite
            // 0 étant la première ligne.
            this.dataGridView1.Rows.Insert(0, "un", "deux", "trois");


            // Edition d'une cellule => dataGridView1[ligne,colonne]
            // Ligne et colonne commencent à 0
            this.dataGridView1[2, 1].Value = "Nouvelles valeur";
            */
        }

        private void Init_Data()
        {
            this.Numéro_carte = this.Add_Li_Datafpga("Numéro_de_carte", typeasend: false);
            //Bras
            foreach (IGB_Spéciaux gb in Li_Gb_Spéciaux)
            {
                gb.Init_Datafpga(this);
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
            //((System.ComponentModel.ISupportInitialize)(this)).BeginInit();


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
            int i = 1;
            foreach (IGB_Spéciaux gb in Li_Gb_Spéciaux)
            {
                i = gb.MAJ_Datafpga(Li_Datafpga, i);
            }
        }

        #endregion
    }
}
