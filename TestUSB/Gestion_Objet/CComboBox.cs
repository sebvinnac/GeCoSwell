using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gestion_Objet
{
    class CComboBox : ComboBox
    {
        #region Constructeur
        public CComboBox(string name, int xpos, int ypos, int xsize, List<String> items,int index,bool visible = true)
        {
            this.Name = name;
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Size = new System.Drawing.Size(xsize, 21);
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.FormattingEnabled = true;
            this.Visible = visible;
            foreach (string str in items)
            {
                this.Items.Add(str);
            }
            this.SelectedIndex = index;
        }
        #endregion

        #region Méthode extérieur

        /// <summary>
        /// Permet d'obtenir l'index du combobox en format binaire
        /// sur le format XXXX si 16 choix par exemple
        /// </summary>
        /// <returns>la valeur textuel en binaire</returns>
        public string Convertie_cmbindex_vers_stringbinaire()
        {
            int i = this.Items.Count;

            return Convertie_cmbindex_vers_stringbinaire(i);
        }

        /// <summary>
        /// Permet d'obtenir l'index du combobox en format binaire
        /// sur le format XXXX si 16 choix par exemple
        /// </summary>
        /// <param name="i">nombre de choix</param>
        /// <returns>la valeur textuel en binaire</returns>
        public string Convertie_cmbindex_vers_stringbinaire(int i)
        {
            string str = Convert.ToString(this.SelectedIndex, 2);
            int nbcar = 0;
            while (i > 0)//permet de calculer le nombre de caractère nécéssaire
            {
                nbcar++;
                i = i / 2;
            }
            while (str.Length < nbcar)//rajoute les 0 qui manque a gauche si il en manque
            {
                str = "0" + str;
            }

            return str;
        }


        /// <summary>
        /// Convertie une valeur binaire en index pour la combobox
        /// </summary>
        /// <param name="val">valeur binaire à convertir</param>
        public void Convertie_stringbinaire_vers_cmbindex(String val)
        {
            this.SelectedIndex = Convert.ToInt32(val, 2);
        }
        #endregion
    }
}
