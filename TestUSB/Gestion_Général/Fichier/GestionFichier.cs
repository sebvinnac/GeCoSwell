using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gestion_Objet
{
    class GestionFichier
    {
        protected string Filtre;
        protected string Titre;
        protected string Chemin;
        protected bool Append;//si on continue dans le fichier

        #region Outils

        /// <summary>
        /// Encode les controls pour pouvoir les enregistrer
        /// </summary>
        /// <param name="listcon">La liste des controls à enregistrer</param>
        /// <returns>Liste de string chaque ligne correspond à un control avec son identifiant</returns>
        protected static List<String> EncodeControl(List<Control> listcon)
        {
            List<String> lText = new List<String>();
            foreach (Control c in listcon)
            {
                if (c is TextBox)
                {
                    lText.Add("[" + c.GetType().ToString() + "][" + c.Name + "][" + c.Text + "]");
                }
                else if (c is CheckBox)
                {

                    lText.Add("[" + c.GetType().ToString() + "][" + c.Name + "][" + ((CheckBox)c).Checked + "]");
                }
                else if (c is ComboBox)
                {
                    lText.Add("[" + c.GetType().ToString() + "][" + c.Name + "][" + ((ComboBox)c).SelectedIndex + "]");
                }
            }
            return lText;
        }
        
        /// <summary>
        /// Transforme des listes de series en liste de string prête à être enregistré
        /// </summary>
        /// <param name="lseries">Liste de series à encoder</param>
        /// <returns>Une liste de string prête à être enregistré</returns>
        protected static List<string> Encode_series(List<Series> lseries)
        {
            List<String> lstring = new List<string>();

            if (lseries.Count != 0)
            {
                lstring.Add("");//élément 0 pour les noms des series
                lstring.Add("");//éléments 1 pour les x y

                //init de la série pour contenir tout les éléments
                for (int i = 0; i < lseries[0].Points.Count; i++)
                {
                    lstring.Add("");
                }

                //on remplit les liste
                foreach (Series series in lseries)
                {
                    lstring[0] += series.Name + ";;";//initialisation des noms
                    lstring[1] += "X;Y;";//initialisation des tableau de colonnes

                    for (int i = 0; i < series.Points.Count; i++)
                    {
                        lstring[i + 2] += series.Points[i].XValue.ToString() + ";"
                            + series.Points[i].YValues[0].ToString() + ";";
                    }
                }
            }
            return lstring;
        }

        #endregion
        
    }
}
