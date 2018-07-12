using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeCoSwell
{
    class CourbePuiTriangle//objet qui détientra toutes les infos
    {
        public CourbePuiTriangle(string Param, string Sens, string Bras, Pen Pinceau)
        {
            this.Param = Param;//stock la position de départ de la courbe
            this.Sens = Sens;//stock le sens de départ de la courbe
            this.Bras = Bras;//stock le nom du bras
            this.Pinceau = Pinceau;//stock la forme du pinceau
        }
        
        public string Param { get; set; }
        public string Sens { get; set; }
        public string Bras { get; set; }
        public Pen Pinceau { get; set; }
    }

    class Dessin
    {
        //----------------------------------------------------------------------
        //Fonction qui dessinne la courbe double pulse
        //
        //longueur = longueur de la zone dessinable
        //hauteur = hauteur de la zone dessinable
        //param = liste des valeurs initiale du triangle
        //sens = sens initial du triangle
        //freq = fréquence de la génération des triangles
        //----------------------------------------------------------------------
        public static void Pbox_courbe_paint_boucleouverte(float longueur, float hauteur, List<CourbePuiTriangle> courbe_pui,string maxcount ,PaintEventArgs e)
        {
            Pbox_paint_rectangle(longueur, hauteur, e);

            float x = longueur / 10;
            float y = hauteur / 10;

            Fleche_d_axe(x, y, 0,8, e);//dessinne les deux fléches d'axe

            for (int i = 0; i < courbe_pui.Count; i++)
            {
                Font drawFont = new Font("Arial", 9);
                SolidBrush drawBrush = new SolidBrush(courbe_pui[i].Pinceau.Color);
                e.Graphics.DrawString(courbe_pui[i].Bras, drawFont, drawBrush, 5, (i + 1) * hauteur / (courbe_pui.Count + 2));
            }
            Detect_Multiligne(hauteur, longueur, courbe_pui, maxcount, e);
        }

        //----------------------------------------------------------------------
        //Fonction qui dessinne la courbe double pulse
        //
        //longueur = longueur de la zone dessinable
        //hauteur = hauteur de la zone dessinable
        //t1 = durée du premier pulse
        //t2 = durée de l'état inactif
        //t3 = durée du second pulse
        //trans = 
        //e = event pour générer le dessins
        //----------------------------------------------------------------------
        public static void Pbox_courbe_paint_2pulse(float longueur, float hauteur, float t1, float t2, float t3, int nivactive,int trans, float tpsmort, PaintEventArgs e)
        {
            float x = longueur / 10;
            float haut = hauteur / 20;
            float bas = haut * 9 - 1;
            haut = haut + 10;
            Pbox_paint_rectangle(longueur, hauteur, e);
            float y = hauteur / 20;
            tpsmort = tpsmort / 1000;

            Fleche_d_axe(x, y, 0f,0, e);
            Fleche_d_axe(x, y, hauteur/2,0, e);

            if (nivactive == 0)// si la sortie est active sur un 0
            {
                haut = bas;
                bas = hauteur / 20;
                bas = bas + 10;
            }
            if (trans ==1)
            {
                Pen couleur = new Pen(Color.Red, 4);// Création d'un crayons épais de 4 pixel et de couleurs rouge
                Pbox_courbe_paint(longueur / 10, hauteur / 2, 0, t1, t2, t3, haut, bas, tpsmort, couleur, "High_Side",1, e);

                couleur = new Pen(Color.Blue, 4);// Création d'un crayons épais de 4 pixel et de couleurs bleu
                Pbox_courbe_paint(longueur / 10, hauteur / 2, hauteur / 2, t1, t2, t3, bas, haut, tpsmort, couleur, "Low_Side",0, e);
            }
            else
            {
                Pen couleur = new Pen(Color.Blue, 4);// Création d'un crayons épais de 4 pixel et de couleurs rouge
                Pbox_courbe_paint(longueur / 10, hauteur / 2, 0, t1, t2, t3, bas, haut, tpsmort, couleur, "High_Side",0, e);

                couleur = new Pen(Color.Red, 4);// Création d'un crayons épais de 4 pixel et de couleurs bleu
                Pbox_courbe_paint(longueur / 10, hauteur / 2, hauteur / 2, t1, t2, t3, haut, bas, tpsmort, couleur, "Low_Side",1, e);
            }

        }

        //----------------------------------------------------------------------
        //Fonction qui dessinne le rectangle
        //
        //longueur = longueur du rectangle
        //hauteur = hauteur du rectangle
        //----------------------------------------------------------------------
        private static void Pbox_paint_rectangle(float longueur, float hauteur, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, 1, 1, longueur - 1, 1);
            e.Graphics.DrawLine(Pens.Black, longueur - 1, 1, longueur - 1, hauteur - 1);
            e.Graphics.DrawLine(Pens.Black, longueur - 1, hauteur - 1, 1, hauteur - 1);
            e.Graphics.DrawLine(Pens.Black, 1, hauteur - 1, 1, 1);
        }

        //----------------------------------------------------------------------
        //Fonction qui dessinne la fléche des axes
        //
        //x = point à gauche à à 10% du rectangle
        //y= point en haut à 10% du rectangle
        //dec_y = décalage du départ
        //ralonge = nombre pixel en plus des deux coté
        //----------------------------------------------------------------------
        private static void Fleche_d_axe(float x, float y,float dec_y,float ralonge, PaintEventArgs e)
        {
            Pen noir = new Pen(Color.Black, 3);
            //fléche a droite
            e.Graphics.DrawLine(noir, x, y * 9 + dec_y, x * 9 + ralonge, y * 9 + dec_y);
            e.Graphics.DrawLine(noir, x * 9 + ralonge, y * 9 + dec_y, x * 9 - 5 + ralonge, y * 9 - 5 + dec_y);
            e.Graphics.DrawLine(noir, x * 9 + ralonge, y * 9 + dec_y, x * 9 - 5 + ralonge, y * 9 + 5 + dec_y);
            //fleche vers le haut
            e.Graphics.DrawLine(noir, x, y * 9 + dec_y, x, y + dec_y - ralonge);
            e.Graphics.DrawLine(noir, x, y + dec_y - ralonge, x - 5, y + 5 + dec_y - ralonge);
            e.Graphics.DrawLine(noir, x, y + dec_y - ralonge, x + 5, y + 5 + dec_y - ralonge);
        }

        //----------------------------------------------------------------------
        //Fonction qui dessinne la courbe double pulse
        //
        //longueur = longueur de la zone dessinable
        //hauteur = hauteur de la zone dessinable
        //t1 = durée du premier pulse
        //t2 = durée de l'état inactif
        //t3 = durée du second pulse
        //haut = valeur du haut
        //bas = valeur du bas
        //tspmort = durée du temps mort
        //c = type si 1 c'est vers le haut le temps mort, sinon vers le bas
        //e = event pour générer le dessins
        //----------------------------------------------------------------------
        private static void Pbox_courbe_paint(float x, float hauteur, float dec_y, float t1, float t2, float t3, float haut, float bas,float tpsmort,Pen couleur, string text,int c, PaintEventArgs e)
        {

            float x2 = x;
            bas = bas + dec_y;
            haut = haut + dec_y;
                float ratiodureetotal = x * 6 / (t1 + t2 + t3);
            float ratiotpsmort = ratiodureetotal * tpsmort;

            e.Graphics.DrawLine(couleur, x, bas, (x = 2 * x) + (ratiotpsmort * c), bas);
            e.Graphics.DrawLine(couleur, x + (ratiotpsmort * c), bas, x + (ratiotpsmort * c), haut);
            e.Graphics.DrawLine(couleur, x2 = x + (ratiotpsmort * c), haut, (x = x + ratiodureetotal * t1) + (ratiotpsmort * (1 - c)), haut);
            e.Graphics.DrawLine(couleur, x + (ratiotpsmort * (1 - c)), haut, x + (ratiotpsmort * (1 - c)), bas);

            Pbox_doublefleche((t1 - c * tpsmort + (1 - c) * tpsmort).ToString() + " µs", "T1", x2, x + (ratiotpsmort * (1 - c)), (haut + bas) / 2, e);

            e.Graphics.DrawLine(couleur, x2 = x + (ratiotpsmort * (1 - c)), bas, (x = x + ratiodureetotal * t2) + (ratiotpsmort * c), bas);
            e.Graphics.DrawLine(couleur, x + (ratiotpsmort * c), bas, x + (ratiotpsmort * c), haut);

            Pbox_doublefleche((t2 + c * tpsmort - (1 - c) * tpsmort).ToString() + " µs", "T2", x2, x + (ratiotpsmort *c), (haut + bas) / 2, e);

            e.Graphics.DrawLine(couleur, x2 = x + (ratiotpsmort * c), haut, (x = x + ratiodureetotal * t3) + (ratiotpsmort * (1 - c)), haut);
            e.Graphics.DrawLine(couleur, x + (ratiotpsmort * (1 - c)), haut, x + (ratiotpsmort * (1 - c)), bas);

            Pbox_doublefleche((t3 - c * tpsmort + (1 - c) * tpsmort).ToString() + " µs", "T3", x2, x + (ratiotpsmort * (1 - c)), (haut + bas) / 2, e);

            e.Graphics.DrawLine(couleur, x + (ratiotpsmort * (1 - c)), bas, 1.1f *x, bas);

            Font drawFont = new Font("Arial", 9);
            SolidBrush drawBrush = new SolidBrush(couleur.Color);
            e.Graphics.DrawString(text, drawFont, drawBrush, 5, hauteur /2+dec_y);

        }

        //----------------------------------------------------------------------
        //Fonction qui dessine une cotation et place un texte à son millieux
        //
        //text1 = le texte que l'on veux voir sous la ligne
        //text 2 = le texte que l'on veux voir sur la ligne
        //x1 début de la fléche a gauche
        //x2 fin de la fléche à droite
        //y axe de la fléche
        //e event pour paint
        //----------------------------------------------------------------------
        private static void Pbox_doublefleche(string text1, string text2, float x1, float x2, float y, PaintEventArgs e)
        {
            Pen noir = new Pen(Color.Black, 3);
            e.Graphics.DrawLine(noir, x1 + 2, y, x2 - 2, y);
            e.Graphics.DrawLine(noir, x1 + 2, y, x1 + 7, y + 5);
            e.Graphics.DrawLine(noir, x1 + 2, y, x1 + 7, y - 5);
            e.Graphics.DrawLine(noir, x2 - 2, y, x2 - 7, y + 5);
            e.Graphics.DrawLine(noir, x2 - 2, y, x2 - 7, y - 5);


            Font drawFont = new Font("Arial", 9);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            e.Graphics.DrawString(text1, drawFont, drawBrush, x1 + (x2 - x1) / 3, y);
            e.Graphics.DrawString(text2, drawFont, drawBrush, x1 + (x2 - x1) / 3, y - 15);
        }

        //----------------------------------------------------------------------
        //Fonction qui dessine les triangles
        //
        //longueur = longueur de la zone dessinable
        //hauteur = hauteur de la zone dessinable
        //param = liste des valeurs initiale du triangle
        //sens = sens initial du triangle
        //freq = fréquence de la génération des triangles
        //----------------------------------------------------------------------
        private static void Pbox_triangle(float hauteur,float largeur,CourbePuiTriangle courbe_pui,List<Pen>couleur,string maxcount, PaintEventArgs e)
        {
            float x_min = largeur / 10;     //point gauche
            float x_max = x_min * 9;        //point droite
            float y_min = hauteur / 10;     //point haut
            float y_max = y_min * 9;        //point bas

            float maxcountf = float.Parse(maxcount);
            float x = x_min;
            float fparam = float.Parse(courbe_pui.Param);
            float y = y_max - (y_max - y_min) / maxcountf * fparam;
            float xnext =0;
            float ynext =0;
            while (xnext < x_max)
            {
                if (courbe_pui.Sens == "0")//monté
                {
                    ynext = y_min;
                    xnext = x + ((maxcountf - fparam) * (x_max - x_min) / 4 / maxcountf);
                    fparam = maxcountf;
                    courbe_pui.Sens = "1";
                    if (xnext > x_max)
                    {
                        ynext = y_max - ((y_max - y_min) * (x_max - x) / (xnext - x));
                        xnext = x_max;
                    }
                }
                else
                {
                    ynext = y_max;
                    xnext = x + (fparam * (x_max - x_min) / 4 / maxcountf);
                    fparam = 0;
                    courbe_pui.Sens = "0";
                    if (xnext > x_max)
                    {
                        ynext = y_min + ((y_max - y_min) * (x_max - x) / (xnext - x));
                        xnext = x_max;
                    }
                }
                Pbox_Multiligne(x, y, xnext, ynext,couleur, e);
                x = xnext;
                y = ynext;
                    
            }
        }

        //----------------------------------------------------------------------
        //Fonction qui détecte si plusieurs ligne on les même références et les assembles
        //puis appel la fonction qui dessine les triangles correspondant
        //
        //longueur = longueur de la zone dessinable
        //hauteur = hauteur de la zone dessinable
        //param = liste des valeurs initiale du triangle
        //sens = sens initial du triangle
        //freq = fréquence de la génération des triangles
        //----------------------------------------------------------------------
        private static void Detect_Multiligne(float hauteur, float largeur, List<CourbePuiTriangle> courbe_pui,string maxcount, PaintEventArgs e)
        {
            for (int i = 0; i < courbe_pui.Count; i++)
            {
                List<Pen> multicouleur = new List<Pen>() { courbe_pui[i].Pinceau };
                for (int y = i; y < courbe_pui.Count; y++)
                {
                    if (i != y)
                    {
                        if (courbe_pui[i].Param == courbe_pui[y].Param && courbe_pui[i].Sens == courbe_pui[y].Sens)
                        {
                            multicouleur.Add(courbe_pui[y].Pinceau);
                            courbe_pui.RemoveAt(y);
                            y--;
                        }
                    }

                }

                Pbox_triangle(hauteur, largeur, courbe_pui[i], multicouleur, maxcount, e);
            }
        }


        //----------------------------------------------------------------------
        //Fonction qui dessine une ligne avec la gestion des multicouleur
        //
        //x point de départ sur l'axe horizontale
        //y point de départ sur l'axe de la hauteur
        //xf point d'arrivé
        //yf point d'arrivé
        //y liste des couleurs à utilisé (peut etre juste une)
        //----------------------------------------------------------------------
        private static void Pbox_Multiligne(float x,float y,float xf,float yf, List<Pen>couleur, PaintEventArgs e)
        {
            float pasx = (xf - x) / couleur.Count / 2;
            float pasy = (yf - y) / couleur.Count / 2;
            for (int i = 0; i < couleur.Count; i++)
            {
                e.Graphics.DrawLine(couleur[i], x + pasx * i, y + pasy * i, x + pasx * (1 + i), y + pasy * (1 + i));
            }
            for (int i = 0; i < couleur.Count; i++)
            {
                e.Graphics.DrawLine(couleur[i], x + pasx * (i+ couleur.Count), y + pasy * (i + couleur.Count), x + pasx * (1 + (i + couleur.Count)), y + pasy * (1 + (i + couleur.Count)));
            }
        }
    }
}
