using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GeCoSwell
{
    public partial class Tab_sinus : Form
    {
        public Tab_sinus()
        {
            InitializeComponent();
            cb_nb_val.SelectedIndex = 0;
        }

        //----------------------------------------------------------------------
        //Fonction qui apel la fonction générer
        //----------------------------------------------------------------------
        private void B_generer_Click(object sender, EventArgs e)
        {
            List<String> list_val = Generer_list(double.Parse((cb_nb_val.SelectedIndex).ToString()), double.Parse(tb_val_max.Text), double.Parse(tb_val_min.Text), double.Parse(tb_nb_bits.Text));
            Save_valeur(list_val);
        }

        //----------------------------------------------------------------------
        //Fonction qui génére la table
        //----------------------------------------------------------------------
        private List<String> Generer_list(double nbpoint, double max, double min,double nb_bit)

        {
            //Génération de l'entête du fichier
            nbpoint = Math.Pow(2, nbpoint + 5);
            List<String> list_val = new List<string>()
            {
            "DEPTH = " + nbpoint.ToString() +";",
            "WIDTH = " + nb_bit + ";",
            "",
            "ADDRESS_RADIX = DEC;",
            "DATA_RADIX = DEC;",
            "",
            "CONTENT",
            "    BEGIN"
            };
            //calcul des points
            for (int i = 0; i < nbpoint; i++)
            {
                double moyenne = (max - min) / 2;
                string r = Math.Round(moyenne + moyenne * Math.Sin(2 * Math.PI / nbpoint * i)).ToString();
                list_val.Add(i.ToString() + " : " + r + ";");
            }
            list_val.Add("	END ;");
            return list_val;
        }

        //----------------------------------------------------------------------
        //Fonction qui génére le fichier tab_sinus
        //
        //list_var la liste a enregistré chaque valeur étant une ligne
        //----------------------------------------------------------------------
        private static void Save_valeur(List<String> list_var)
        {
            try
            {
                StreamWriter sw;

                // Douvre le OpenFileDialog pour que l'utilisateur choissise le fichier à ouvrir
                SaveFileDialog saveFileProg = new SaveFileDialog();
                saveFileProg.Filter = "Table de sinus|*.mif";
                saveFileProg.Title = "Sauvegarder la table de sinus";
                saveFileProg.RestoreDirectory = true; //conserve l'emplacement de la dernière ouverture du fichier
                saveFileProg.ShowDialog();

                if (saveFileProg.FileName != "") // si le nom du fichier n'est pas vide
                {
                    sw = new StreamWriter(saveFileProg.OpenFile(), System.Text.Encoding.ASCII);
                    // pour chaque "line" dans le résultat de l'encodage on l'écrit dans le fichier
                    foreach (String line in list_var)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                }
                else
                {
                    MessageBox.Show("Sauvegarde échoué, attention certains paramètres sont peut être à corriger", "Sauvegarde",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }

            catch
            {
                MessageBox.Show("Sauvegarde échoué, attention certains paramètres sont peut être à corriger", "Sauvegarde",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }

        }

        //----------------------------------------------------------------------
        //Fonction qui vérifie que les valeurs sont correcte
        //----------------------------------------------------------------------
        private void Verif_textbox_TextChanged(object sender, EventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "tb_nb_val"://si c'est le textbox tb_nb_val
                    ((TextBox)sender).Text = Verif_textbox(((TextBox)sender).Text, 99999, 1, "8192"); //lance la vérification suivant les paramètres et retourne la même valeur si tout va bien, ou la valeur defaut
                    break;
                case "tb_nb_bits"://si c'est le textbox tb_nb_val
                    ((TextBox)sender).Text = Verif_textbox(((TextBox)sender).Text, 20, 0, "10"); //lance la vérification suivant les paramètres et retourne la même valeur si tout va bien, ou la valeur defaut
                    break;
                case "tb_val_max"://si c'est le textbox tb_nb_val
                    ((TextBox)sender).Text = Verif_textbox(((TextBox)sender).Text, int.Parse(Math.Pow(2,double.Parse(tb_nb_bits.Text)).ToString()), int.Parse(tb_val_min.Text), Math.Pow(2, double.Parse(tb_nb_bits.Text)).ToString()); //lance la vérification suivant les paramètres et retourne la même valeur si tout va bien, ou la valeur defaut
                    break;
                case "tb_val_min"://si c'est le textbox tb_nb_val
                    ((TextBox)sender).Text = Verif_textbox(((TextBox)sender).Text, int.Parse(tb_val_max.Text), 0, "0"); //lance la vérification suivant les paramètres et retourne la même valeur si tout va bien, ou la valeur defaut
                    break;
            }
        }

        //----------------------------------------------------------------------
        //Fonction qui vérifie que les valeurs suivant des paramètres
        //
        //val la valeur vérifié
        //max le max autorisé
        //min le min autorisé
        //defaut la valeur retourné si une erreur existe
        //----------------------------------------------------------------------
        private string Verif_textbox(string val, int max,int min,string defaut)
        {
            int ival;
            if (int.TryParse(val, out ival))    //si la valeur est numérique
            {
                if (ival >= min && ival <= max)     //et si la valeur n'est ni trop petite ni trop grande
                    return val;                     //retourne la valeur d'origine
            }
            return defaut;                          //sinon retourne la valeur par défaut
        }
       
    }
}
