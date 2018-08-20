using System;

namespace Méthod_outil
{
    static class Outil_text
    {
        public static string StringDec_versStringBinaire(string str,int nbdecaractère)
        {
            str = Convert.ToString(int.Parse(str), 2);
            for (int i = str.Length; i < nbdecaractère; i++)
            {
                str = "0" + str;
            }
            return str;
        }
    }
}
