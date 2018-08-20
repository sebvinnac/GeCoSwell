using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gestion_Objet
{
    class TTextBox : TextBox
    {
        public double Valeur_Min_Théorique { get; set; }
        public double Valeur_Max_Théorique { get; set; }
        public double Valeur_Pas { get; private set; }

        String Correction;
        
        private TTextBox Tb_Lié_pour_ratio;

        #region Constructeur

        /// <summary>
        /// Génére des textbox générique
        /// Qui vérifie toujours si leurs valeur numérique sont oki
        /// </summary>
        /// <param name="name">nom</param>
        /// <param name="xpos">position en x</param>
        /// <param name="ypos">position en Y</param>
        /// <param name="text">Texte de la textbox par défaut</param>
        /// <param name="min">Valeur min autorisé</param>
        /// <param name="max">Valeur max autorisé</param>
        /// <param name="pas">Pas minimum autorisé exemple 0.1</param>
        /// <param name="xsize">taille sur x</param>
        /// <param name="ysize">taille sur y</param>
        /// <returns>la textbox toute faite</returns>
        public TTextBox(string name, int xpos, int ypos, string text, double min,double max, double pas, int xsize = 40, int ysize = 20)
        {
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = name;
            this.Size = new System.Drawing.Size(xsize, ysize);
            this.Text = text;
            this.TextAlign = HorizontalAlignment.Center;

            this.Valeur_Min_Théorique = min;
            this.Valeur_Max_Théorique = max;
            this.Valeur_Pas = pas;
            this.Correction = text;
            this.Validating += new CancelEventHandler(this.Verif_si_valeur_ok);
        }

        /// <summary>
        /// Génére des textbox générique
        /// Qui vérifie toujours si leurs valeur numérique sont oki
        /// </summary>
        /// <param name="name">nom</param>
        /// <param name="xpos">position en x</param>
        /// <param name="ypos">position en Y</param>
        /// <param name="text">Texte de la textbox par défaut</param>
        /// <param name="xsize">taille sur x</param>
        /// <param name="ysize">taille sur y</param>
        /// <returns>la textbox toute faite</returns>
        public TTextBox(string name, int xpos, int ypos, string text, int xsize = 40, int ysize = 20)
        {
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = name;
            this.Size = new System.Drawing.Size(xsize, ysize);
            this.Text = text;
            this.TextAlign = HorizontalAlignment.Center;
        }

        #endregion

        #region Code appelé par ses propres objets

        /// <summary>
        /// Vérifie si la valeur respecte ses bornes, sinon remet la valeur par défaut
        /// </summary>
        private void Verif_si_valeur_ok(object sender, CancelEventArgs e)
        {
            try
            {
                double val = double.Parse(this.Text.Replace('.', ','));//remplace les point par des virgules pour respecté le Fr-fr

                if (ValeurEstOk(val))
                    this.Text = val.ToString();
                else
                    this.Text = this.Correction;
            }
            catch
            {
                this.Text = this.Correction;
            }
        }

        private bool ValeurEstOk(double val)
        {
            double mult = 1 / this.Valeur_Pas;
            return (val >= this.Valeur_Min_Théorique && val <= this.Valeur_Max_Théorique && (val == 0 || (val * mult) % 1 == 0));
        }
        #endregion

        #region Gestion extérieur
        
        /// <summary>
        /// Génère la string pour les tooltip qui prend en compte le min max et le pas
        /// </summary>
        /// <returns>la string à utiliser</returns>
        public string GeneText_ToolTip()
        {
            return "min " + this.Valeur_Min_Théorique + "; max " + this.Valeur_Max_Théorique + (this.Valeur_Pas == 1 ? "" : "; par pas de " + this.Valeur_Pas);
        }

        /// <summary>
        /// Permet de lier deux textbox pour les ratios
        /// </summary>
        /// <param name="tb1">la première textbox</param>
        /// <param name="tb2">la seconde textbox</param>
        public static void Lier_ratio_entre_2textbox(TTextBox tb1,TTextBox tb2)
        {
            tb1.Lier_ration_avec_autre_textbox(tb2);
            tb2.Lier_ration_avec_autre_textbox(tb1);
        }

        /// <summary>
        /// Permet de lier la textbox à une autre pour les ratio
        /// </summary>
        /// <param name="tb">la première textbox</param>
        public void Lier_ration_avec_autre_textbox(TTextBox tb)
        {
            this.Tb_Lié_pour_ratio = tb;
            this.Validated += new EventHandler(this.Tb_ratio_a_MAJ_dans_tb_lié);
        }

        /// <summary>
        /// Met à jour la TextBox lié en respectant les ratios
        /// </summary>
        private void Tb_ratio_a_MAJ_dans_tb_lié(object sender, EventArgs e)
        {
            this.Tb_Lié_pour_ratio.Text = (this.Valeur_Min_Théorique + Math.Round(Double.Parse(this.Text)* this.Tb_Lié_pour_ratio.Valeur_Max_Théorique / (this.Valeur_Max_Théorique - this.Valeur_Min_Théorique) /this.Tb_Lié_pour_ratio.Valeur_Pas) * this.Tb_Lié_pour_ratio.Valeur_Pas).ToString();
        }

        /// <summary>
        /// Met à jour la TextBox lié en respectant les ratios
        /// </summary>
        public void Tb_ratio_a_MAJ()
        {
            this.Text = (this.Valeur_Min_Théorique + Math.Floor((this.Valeur_Max_Théorique - this.Valeur_Min_Théorique) * double.Parse(this.Tb_Lié_pour_ratio.Text) / this.Tb_Lié_pour_ratio.Valeur_Max_Théorique)).ToString();
        }

        /// <summary>
        /// Met à jour la TextBox lié en respectant les ratios
        /// </summary>
        public void Tb_ratio_a_MAJ(int new_max, int new_min)
        {
            this.Text = (new_min + Math.Floor((new_max - new_min) * double.Parse(this.Tb_Lié_pour_ratio.Text) / this.Tb_Lié_pour_ratio.Valeur_Max_Théorique)).ToString();
        }
        #endregion
    }
}
