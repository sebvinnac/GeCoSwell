using System.Windows.Forms;

namespace Gestion_Objet
{
    public class LLabel : Label
    {
        /// <summary>
        /// Génére des Label générique
        /// </summary>
        /// <param name="name">nom</param>
        /// <param name="xpos">position en x</param>
        /// <param name="ypos">position en Y</param>
        /// <param name="text">Text de la textbox</param>
        /// <returns>le Label tout fait</returns>
        public LLabel(string name, int xpos, int ypos, string text)
        {
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = name;
            this.AutoSize = true;
            this.Text = text;
        }
    }
}
