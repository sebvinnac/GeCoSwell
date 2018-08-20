using System;

namespace GeCoSwell
{
    class Chiffre
    {        
        // Deplacer la virgule d'une position vers l'indice 1
        private static String Virgule(String nombre, int position)
        {
            return nombre.Replace(',', '0').Insert(1, ",");
        }

        // Rajouter un caractère si le nombre est négatif
        private static String Negatif(String nombre, String nombreOriginel)
        {
            if (double.Parse(nombre) < 0)
            {
                nombre = (nombreOriginel.Length > 4) ? nombre + nombreOriginel.Substring(5, 1) : nombre + '0';
            }

            if (double.Parse(nombre) == 0)
            {
                return "0,000";
            }
            return nombre;
        }

        // Obtenir 4 chiffres significatifs du nombre "number"
        public static String Significant(String number)
        {
            String n = number;
            if (n.Length > 3 && !n.Contains(","))
            {// si plus grand que 3 et ne contient pas de virgule tel quel 
                return Negatif(n, number);
            }
            else if (!n.Contains(","))
            {// sinon  si pas de virgule rajouter virgule a droite
                n += ",";
            }

            if (n.Length > 5)
            {// si plus de 5 caract prendre les 5 premiers
                return Negatif(n.Substring(0, 5), number);
            }
            else if (n.Length <= 4)
            {// sinon si moins de 4 CARACT rajouter un 0
                return Negatif(String.Concat(n, new String('0', Math.Abs(5 - n.Length))), number);
            }
            return Negatif(n, number);
        }
    }
}
