using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gestion_Objet
{
    static class GestionObjet
    {

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
            return (GestionObjet.Trouver_controls_dun_type(parent, new List<Type>() { typeof(TextBox), typeof(CheckBox), typeof(ComboBox) }));
        }

        /// <summary>
        /// Trouve l'objet qui posséde le même nom et lui applique la nouvelle valeur
        /// </summary>
        /// <param name="nom">Nom de l'objet</param>
        /// <param name="etat">Etat à appliquer, soit text, soit true or false en string </param>
        /// <param name="control_T">list des controls à tester</param>
        public static void Trouver_et_Appliquer(string nom, string etat,List<Control> control_T)
        {
            foreach (Control ct in control_T)
            {
                if (ct.Name == nom)
                {
                    TextBox tb = ct as TextBox;//plus rapide que le is
                    if (tb != null)
                    {
                        tb.Text = etat;
                        tb.Select();
                    }
                    else
                    {
                        CheckBox cb = ct as CheckBox;//plus rapide que le is
                        if (cb != null)
                        {
                            cb.Checked = bool.Parse(etat);
                        }
                        else
                        {
                            ComboBox cmb = ct as ComboBox;//plus rapide que le is
                            if (cmb != null)
                            {
                                cmb.SelectedIndex = int.Parse(etat);
                            }
                        }
                    }
                }
            }
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
        public static List<Control> Trouver_controls_dun_type(Control parent, Type type)
        {
            //Fonction qui stock dans une liste tout les éléments qui correspondent au type en deuxième paramètre
            return GestionObjet.Trouver_controls_dun_type(parent, new List<Type>() { type });
        }

        /// <summary>
        /// liste tout les controls de la feuille (récursive)
        /// </summary>
        /// <param name="parent">controls dont les enfants vont être listé</param>
        /// <param name="l_type">Liste des type d'objet doit être listé</param>
        /// <returns>tout les controls du type choisis</returns>
        public static List<Control> Trouver_controls_dun_type(Control parent, List<Type> l_type)
        {
            List<Control> listctrl = new List<Control>();
            //Pour gérer l'autochargement qui créer une exeption
            if (parent.HasChildren)
            {
                foreach (Control child in parent.Controls)
                {
                    listctrl = Détect_si_objet_est_de_type(child, l_type, listctrl);

                    if (child.HasChildren)
                    {
                        listctrl.AddRange(Trouver_controls_dun_type(child, l_type));
                    }

                }
            }
            else
            {
                listctrl = Détect_si_objet_est_de_type(parent, l_type, listctrl);
            }
            return listctrl;
        }

        private static List<Control> Détect_si_objet_est_de_type(Control control,List<Type> ltype, List<Control> lcontrolfinal)
        {
            foreach (Type type in ltype)
            {
                if (control.GetType() == type)
                {
                    lcontrolfinal.Add(control);
                    return lcontrolfinal;//des qu'on trouve on sort
                }
            }
            return lcontrolfinal;
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
