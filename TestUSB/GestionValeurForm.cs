using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeCoSwell
{
    class GValeurForm
    {
        public void TestValeur(object sender, List<Control> control_t)
        {

        }

        //----------------------------------------------------------------------
        //Donne la valeur d'index d'une combobox sous le bon format
        //
        //type = 1 deux choix choix 0 renvoie 0 sinon renvoie 1
        //type = 2 4 choix 0 donne 00 1 donne 01 2 donne 10 et 3 donne 11
        //sens = 1 combobox => string sinon string vers combobox
        //sender = optionel la combobox
        //val = optionel la valeur a mettredans la combobox
        //----------------------------------------------------------------------
        public static string Conv_cmb_string(int type, int sens, object sender = null,String val = "")
        {
            if(sens == 1)
            {
                if (type == 1)//cas à 2 choix
                {
                    return ((ComboBox)sender).SelectedIndex.ToString();
                }
                if(type == 2)//cas à 3 ou 4 choix
                {
                    switch(((ComboBox)sender).SelectedIndex)
                    {
                        case 0:
                            return "00";
                        case 1:
                            return "01";
                        case 2:
                            return "10";
                        case 3:
                            return "11";

                    }
                }
            }
            else
            {
                ((ComboBox)sender).SelectedIndex = Convert.ToInt32(val, 2);
            }
            return "verifcode";
        }

        //----------------------------------------------------------------------
        //renvoie une valeur suivant l'état d'un checkbox
        //
        //type = 1 deux choix choix 0 renvoie 0 sinon renvoie 1
        //type = ?
        //sens = 1 chb => string sinon string vers chb
        //sender = optionel la combobox
        //val = optionel la valeur a mettredans la  chx si 1 true sinon false
        //----------------------------------------------------------------------
        public static string Conv_chb_string(int type, int sens, object sender = null, String val = "")
        {
            if (sens == 1)
            {
                if (type == 1)
                {
                    if (((CheckBox)sender).Checked)//si la checkbox et check
                        return "1";//renvoie 1
                    else
                        return "0";//sinon 0
                }
            }
            else
            {
                if (type == 1)
                {
                    if (val == "1")//si val = 1
                        ((CheckBox)sender).Checked = true; // la checkbox sender deviens check
                    else
                        ((CheckBox)sender).Checked = false; // sinon on la met un check
                }
                return "fait";
            }
            return "verifcode";
        }

        //----------------------------------------------------------------------
        //Fonction qui créer une liste d'objet provenant de parent
        //demande textbox combobox checkbox
        //
        //parent = control qui va être lister
        //
        //renvoi
        //listctrl = list qui va contenir la liste complête des objets demandé
        //----------------------------------------------------------------------
        public static List<Control> Crealist(Control parent)
        {
            List<Control> control_T = new List<Control>();
            //Création de faux élément pour être utiliser dans l'appel.

            //Fonction qui stock dans une liste tout les éléments qui correspondent au type en deuxième paramètre
            control_T = (GValeurForm.FindAllControlForOneType(parent, typeof(TextBox), control_T));
            control_T = (GValeurForm.FindAllControlForOneType(parent, typeof(CheckBox), control_T));
            return (GValeurForm.FindAllControlForOneType(parent, typeof(ComboBox), control_T));
        }

        //----------------------------------------------------------------------
        //Fonction qui créer une liste d'objet provenant de parent
        //demande textbox combobox checkbox
        //
        //parent = control qui va être lister
        //
        //renvoi
        //listctrl = list qui va contenir la liste complête des objets demandé
        //----------------------------------------------------------------------
        public static List<Control> Crealist(Control parent,Type type)
        {
            List<Control> control_T = new List<Control>();
            //Création de faux élément pour être utiliser dans l'appel.

            //Fonction qui stock dans une liste tout les éléments qui correspondent au type en deuxième paramètre
            return control_T = (GValeurForm.FindAllControlForOneType(parent, type, control_T));
        }

        //----------------------------------------------------------------------
        //Fonction récursive qui liste tout les controls de la feuille
        //
        //parent = control qui va être lister
        //type = type rechercher
        //listctrl = list qui va contenir la liste complête des objets demandé
        //----------------------------------------------------------------------
        public static List<Control> FindAllControlForOneType(Control parent, Type type, List<Control> listctrl)
        {
            //Pour gérer l'autochargement qui créer une exeption
            if (parent.GetType() == type)
            {
                listctrl.Add(parent);
                return listctrl;
            }
            // fin de l'exception

            foreach (Control child in parent.Controls)
            {
                if (child.GetType() == type)
                {
                    listctrl.Exists(x => x.Name != child.Name);
                    {
                        listctrl.Add(child);
                    }
                }

                else
                    listctrl = FindAllControlForOneType(child, type, listctrl);
            }
            return listctrl;
        }

        //----------------------------------------------------------------------
        //écrit une chaine de caractère au bon endroit
        //
        //txtboxachanger la textbox qui va changer
        //newval la valeur a mettre à la place
        //pos la position de la valeur
        //----------------------------------------------------------------------
        public static void String_replace(object txtboxachanger, string newval, int pos)
        {
            string partA = ((TextBox)txtboxachanger).Text.Substring(0, pos);
            int temp = ((TextBox)txtboxachanger).Text.Length -pos - newval.Length;
            string partB = ((TextBox)txtboxachanger).Text.Substring(pos + newval.Length, temp);
            ((TextBox)txtboxachanger).Text = partA + newval + partB;
        }
    }
}
