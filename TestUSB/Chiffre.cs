using System;

namespace GeCoSwell
{
    class Chiffre
    {
        // Formater le nombre pour qu'il ne possède que n chiffres significatifs
        // nombre : le nombre à formater
        // n : le nombre de chiffres significatifs voulus
        public static String Significatif(String nombre, int n)
        {
            String nb;              // nombre possédant n chiffres significatifs
            int zeroAjoute;         // nombre de zeros à ajouter au nombre
            int positionVirgule;    // indice de la virgule dans la chain

            if (nombre.Contains(","))
            {
                positionVirgule = nombre.IndexOf(",");

                if (positionVirgule > n - 1)
                { // la virgule est placée après les n premiers caractères
                    nb = nombre.Substring(0, n);
                    zeroAjoute = positionVirgule - n; // zéros nécessaire pour atteindre le chiffre des unités
                    nb = String.Concat(nb, new String('0', zeroAjoute));
                }
                else
                {// la virgule est dans les n premiers caractères
                    if (nombre.Length > n)
                    {
                        nb = nombre.Substring(0, n + 1);
                    }
                    else
                    {
                        nb = nombre.Substring(0, n);
                        nb += "0";
                    }

                }
            }
            else
            {
                nb = nombre;
                zeroAjoute = Math.Abs(nombre.Length - n);

                if (nombre.Length < n)
                {
                    nb += ",";
                    nb = String.Concat(nb, new String('0', zeroAjoute));
                }
                else
                {
                    nb = nombre.Substring(0, n);
                }

            }

            if (nb.Equals(new String('0', n)) || nb.Equals(new String('0', n).Insert(n, ",")))
            {
                nb = nb.Insert(1, ",");
            }

            return nb;
        }

        // Ajouter la puissance de dix nombre_zeros aux nombres
        private static String Zeros(String nombre, int nombre_zeros)
        {
            return String.Concat(nombre, "e" + nombre_zeros.ToString());
        }

        // Ecrire un nombre avec les notations scientifiques
        public static String Scientifique(String nombre, int chiffre)
        {
            String valeur = double.Parse(nombre).ToString("E");
            String[] temp = valeur.Split('E');
            temp[0] = Chiffre.Significatif(temp[0], chiffre);

            return String.Concat(temp[0], "e" + temp[1]);

        }

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
