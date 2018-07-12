using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeCoSwell
{
    class CalculFeuille
    {
        //----------------------------------------------------------------------
        //Fonction qui transforme une valeur en % ou un % en valeur brut
        //
        //val valeur soit du pourcentage, soit la valeur actuel brut
        //max valeur maximum de la valeur brut
        //nb_virgule nombre de chiffre après la virgule de notre résultat
        //type si 0 valeur => % et si 1 % => valeur
        //----------------------------------------------------------------------
        public static string Conv_10bitpour(string val, double max, double nb_virgule, Boolean type)
        {
            double val_int;
            if (type)
            {
                val_int = max / 100 * double.Parse(val);
            }
            else
            {
                val_int = double.Parse(val) * 100 / max;
            }
            // retourne l'arrondie de val (int * 10 puissance nb_virgule) le tout diviser par 10 puissance nb_virgule
            return (Math.Round(val_int * Math.Pow(10, nb_virgule)) / Math.Pow(10, nb_virgule)).ToString();
        }

        //----------------------------------------------------------------------
        //Fonction qui vérifie une valeur entré dans une case
        //
        // renvoie un bool true si la valeur respecte les régles
        //----------------------------------------------------------------------
        public static string Casenum(string valtext, double valmin, double valmax, double pas, int type, string correction)
        {
            try
            {
                double val = double.Parse(valtext.Replace('.', ','));
                if (type == 0)
                {
                    double test = val % (int)val;
                    if (val < valmin || val > valmax || (val % (int)val != 0 && val != 0) || val % pas != 0)
                    {
                        return correction;
                    }
                }
                if (type == 1)
                {
                    double mult = 1 / pas;
                    // ((val * mult) % 1 test si la partie decimale est nulle 
                    if (val < valmin || val > valmax || ((val * mult) % 1 != 0f && val != 0)) //condition spécial car je n'arrive pas a faire marcher le % avec des chiffres à virgule.
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
