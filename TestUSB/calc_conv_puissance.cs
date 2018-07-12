using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeCoSwell
{
    class Calc_conv_puissance
    {
        //------------------------------------------------------------------------------------
        //Fonction qui génére les valeurs pour une commande multi_niveau
        //
        //val liste des textbox avec qui on travail
        //Sens list des combobox avec qui on travail 0 => montant, 1 => descendant
        //incrément nombre de pas dans notre triangle
        //type 0 => dent de scie, 1=> vrai signal triangle
        //------------------------------------------------------------------------------------
        public static void Multiniv(List<TextBox> val,List<ComboBox> sens,int increment,Boolean type)
        {
            int nombre = val.Count();
            int max = increment;
            if (type)
            {
                increment = increment * 2;
            }
            for (int i = 0; i < val.Count(); i++)
            {
                if (increment * i / val.Count() > max - 1)
                {
                    val[i].Text = (2 * max - increment * i / val.Count()).ToString();
                    sens[i].SelectedIndex = 1;
                }
                else
                {
                    val[i].Text = (increment * i / val.Count()).ToString();
                    sens[i].SelectedIndex = 0;
                } 
            }
        }

        //----------------------------------------------------------------------
        //Calcul les max et min de chaque bras dans le mode multiniveaux
        //----------------------------------------------------------------------
        public static void Calc_multiniv(List<TextBox> maxl, List<TextBox> minl, TextBox maxtriangle)
        {
            if (maxl.Count != 0)
            {
                int n = 1;
                int pas = int.Parse(maxtriangle.Text) / maxl.Count;
                while (n <= maxl.Count)
                {
                    minl[maxl.Count - n].Text = (pas * (n - 1)).ToString();
                    minl[maxl.Count - n].Select();
                    maxl[maxl.Count - n].Text = (pas * (n)).ToString();
                    n++;
                }
                maxtriangle.Select();
            }
        }
    }
}
