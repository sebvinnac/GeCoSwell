using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUSB
{
    class Verif_textbox
    {
        //----------------------------------------------------------------------
        //Fonction qui vérifie une valeur entré dans une case
        //
        // renvoie un bool true si la valeur respecte les régles
        //----------------------------------------------------------------------
        public static string Casenum(string valtext, float valmin, float valmax, double pas, int type,string correction)
        {
            try
            {
                double val = double.Parse(valtext.Replace('.',','));
                if (type == 0)
                {
                    double test = val % (int)val;
                    if (val < valmin || val > valmax || (val % (int)val != 0 && val != 0) || val % pas != 0 )
                    {
                        return correction;
                    }
                }
                if (type == 1)
                {
                    double mult = 1 / pas;
                    if (val < valmin || val > valmax || ((val * mult) % (pas * mult) != 0f && val != 0)) //condition spécial car je n'arrive pas a faire marcher le % avec des chiffres à virgule.
                    { 
                        return correction;
                    }
                    return val.ToString();
                }
            }
            catch
            {
                return correction;
            }

            return valtext;
        }
    }
}
