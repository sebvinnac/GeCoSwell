using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Objet
{
    class GGroupBox : GroupBox
    {
        public const int EcartX = 6;
        public const int EcartY = 6;

        protected List<GGroupBox> Li_Gb_du_mêmetype;
        protected List<GGroupBox> Li_Gb_dessous; 
        
        #region méthode extérieur
        /// <summary>
        /// déplace les groupbox consigne suivant
        /// </summary>
        public static void Déplacement_gb_consigne(List<GGroupBox> l_gb,int index)
        {
            for (int i = index; i < l_gb.Count; i++)
            {
                l_gb[i].Location = new System.Drawing.Point(Position_X_droite(l_gb[i - 1]) + EcartX, l_gb[i].Location.Y);
            }
        }

        /// <summary>
        /// Donne la position x à droite de la groupbox
        /// </summary>
        public static int Position_X_droite(GroupBox gb)
        {
            
            return gb.Location.X + gb.Size.Width + EcartX;
        }

        /// <summary>
        /// Donne le point y en bas du groupbox consigne
        /// </summary>
        /// <returns>Position en y</returns>
        private static int Position_Y_bas(GroupBox gb)
        {
            return gb.Location.Y + gb.Size.Height+ EcartY;
        }

        /// <summary>
        /// Change la position Y
        /// </summary>
        public void Change_Position_Pos_Y(int ypos)
        {
            this.Location = new System.Drawing.Point(this.Location.X, ypos);
        }

        /// <summary>
        /// donne la position base du plus bas élément
        /// </summary>
        /// <returns>retourne la position en y</returns>
        public static int Position_Y_bas_max(List<GGroupBox> l_gb)
        {
            int ypos = 0;
            foreach (GGroupBox gb in l_gb)
            {
                ypos = Math.Max(ypos, Position_Y_bas(gb));
            }
            return ypos;
        }

        public void Lié_position_y_deux_gb(List<GGroupBox> l_gb1,List<GGroupBox> l_gb2)
        {
            foreach (GGroupBox gb in l_gb1)
            {
                gb.Li_Gb_du_mêmetype = l_gb1;
                gb.Li_Gb_dessous = l_gb2;
                gb.SizeChanged += new EventHandler(Déplacer_élément_dessous);
            }
        }

        private void Déplacer_élément_dessous(object sender, EventArgs e)
        {
            Move_gb2_quand_gb1_sizechange(Li_Gb_du_mêmetype, Li_Gb_dessous);
        }

        private void Move_gb2_quand_gb1_sizechange(List<GGroupBox> l_gb1,List<GGroupBox> l_gb2)
        {
            int ypos = Position_Y_bas_max(l_gb1);
            foreach (GGroupBox gb in l_gb2)
            {
                gb.Change_Position_Pos_Y(ypos);
            }
        }


        #endregion
    }
}
