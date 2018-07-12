using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GeCoSwell
{
    class GestionFichier
    {
        private string filtre;
        private string titre;
        private string chemin;
        private bool append;//si on continue dans le fichier
        private StreamWriter sw;

        //Filter = "Programation des paramètres|*.save",
        //Title = "Sauvegarder tout les paramètres actuel",
        //chemin
        //append true on continue dans le fichier existant
        public GestionFichier(string filtre, string titre,string chemin = "",bool append = false)
        {
            this.filtre = filtre;
            this.titre = titre;
            this.chemin = chemin;
            this.append = append;
        }


        //memorise l'emplacement et créer le streamwriter correspondant
        //si chemin vide on va ouvrir l'explorateur pour chosir un fichier, sinon on ouvre celui mis en chemin
        public void ChoixEmplacement()
        {
            if (this.chemin != "")
            {
                sw = new StreamWriter(this.chemin, this.append);
            }
            else
            {
                // Douvre le OpenFileDialog pour que l'utilisateur choissise le fichier à ouvrir
                SaveFileDialog saveFileProg = new SaveFileDialog()
                {
                    Filter = filtre,
                    Title = titre,
                    RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
                };
                saveFileProg.ShowDialog();

                if (saveFileProg.FileName != "") // si le nom du fichier n'est pas vide
                {
                    sw = new StreamWriter(saveFileProg.OpenFile());

                }
            }
        }

        //écrit les listes de text dans le fichier
        public void Ecrire(List<String> l_test)
        {
            try
            {
                sw.WriteLine("#Fichier générer par GeCoSwell de vinnac seb");
                // pour chaque "line" dans le résultat de l'encodage on l'écrit dans le fichier
                foreach (String line in l_test)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
            catch
            {
                MessageBox.Show("Sauvegarde échoué", "Sauvegarde",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        //gestion automatique de la classe gestionfichier pour les points des courbes
        public static void Save_point(List<Series> lseries)
        {
            GestionFichier gf = new GestionFichier("Fichier texte|*.txt", "Sauvegarde des points pour Excel");
            gf.ChoixEmplacement();
            if (gf.sw != null)
            {
                List<string> lstring = new List<string>();
                lstring = Mise_en_forme_point(lseries);
                if (lstring.Any())
                {
                    gf.Ecrire(lstring);
                }
            }

            //Filter = "Programation des paramètres|*.save",
            //Title = "Sauvegarder tout les paramètres actuel",

        }

        private static List<string> Mise_en_forme_point(List<Series> lseries)
        {
            List<String> lstring = new List<string>();

            if (lseries.Count != 0)
            {
                lstring.Add("");//élément 0 pour les noms des series
                lstring.Add("");//éléments 1 pour les x y

                //init de la série pour contenir tout les éléments
                for (int i = 0; i < lseries[0].Points.Count; i++)
                {
                    lstring.Add("");
                }

                //on remplit les liste
                foreach (Series series in lseries)
                {
                    lstring[0] += series.Name + ";;";//initialisation des noms
                    lstring[1] += "X;Y;";//initialisation des tableau de colonnes

                    for (int i = 0; i < series.Points.Count; i++)
                    {
                        lstring[i + 2] += series.Points[i].XValue.ToString() + ";"
                            + series.Points[i].YValues[0].ToString() + ";";
                    }
                }
                

            }
            return lstring;
        }

        //----------------------------------------------------------------------
        //Fonction qui écrit dans le log
        //
        //text le texte à écrire dans le log
        //----------------------------------------------------------------------
        public static void Log_Write_Time(string text)
        {
            try
            {
                FileInfo fichier = new FileInfo("log.txt");
                StreamWriter sw = new StreamWriter("log.txt", true, System.Text.Encoding.ASCII);
                sw.WriteLine(DateTime.Now + " : " + text + Environment.NewLine);
                sw.Close();
            }
            catch (Exception e)
            {
                GestionFichier.Log_Write_Time(e.ToString());
            }
        }

        //----------------------------------------------------------------------
        //Fonction pour ouvrir le fichier log
        //----------------------------------------------------------------------
        public static void Log_Open()
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                // Nom du fichier dont l'extension est connue du shell à ouvrir
                proc.StartInfo.FileName = "log.txt";
                proc.Start();
                proc.Close();
            }
            catch
            {
                Log_Write_Time("Log inexistant");
            }
        }

        

        //----------------------------------------------------------------------
        //Fonction pour sauvegarder
        //
        //list_control list de tout les controls
        //----------------------------------------------------------------------
        private static void Save_valeur(List<Control> list_control, string fichier)
        {
            FileInfo file;// = new FileInfo(nom_fichier);
            StreamWriter sw;
            if (fichier != "-1")
            {
                file = new FileInfo(fichier);
                sw = new StreamWriter(fichier, false, System.Text.Encoding.ASCII);
            }
            else
            {
                // Douvre le OpenFileDialog pour que l'utilisateur choissise le fichier à ouvrir
                SaveFileDialog saveFileProg = new SaveFileDialog()
                {
                Filter = "Programation des paramètres|*.save",
                Title = "Sauvegarder tout les paramètres actuel",
                RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
                };

                saveFileProg.ShowDialog();

                if (saveFileProg.FileName != "") // si le nom du fichier n'est pas vide
                {
                    sw = new StreamWriter(saveFileProg.OpenFile(), System.Text.Encoding.ASCII);

                }
                else
                {
                    return;
                }
            }
            try
            {
                sw.WriteLine("#Sauvegarde pour le logiciel de control FPGA de vinnac seb");
                // pour chaque "line" dans le résultat de l'encodage on l'écrit dans le fichier
                foreach (String line in EncodeControl(list_control))
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
            catch
            {
                MessageBox.Show("Sauvegarde échoué, attention certains paramètres sont peut être à corriger", "Sauvegarde",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }

        }


        //--------------------------------------------------------------------
        //Encode les control pour être compatible avec les futur chargement
        //
        //Listcon = la list de tous les controls
        //
        //Renvoi ltext le texte formaté avec le formatage voulu pour être chargé plus tard
        //--------------------------------------------------------------------
        private static List<String> EncodeControl(List<Control> listcon)
        {
            List<String> lText = new List<String>();
            int i = 0;
            foreach (Control c in listcon)
            {
                if (c is TextBox)
                {
                    lText.Add("[" + c.GetType().ToString() + "][" + c.Name + "][" + c.Text + "]");
                }
                else if (c is CheckBox)
                {

                    lText.Add("[" + c.GetType().ToString() + "][" + c.Name + "][" + ((CheckBox)c).Checked + "]");
                }
                else if (c is ComboBox)
                {
                    lText.Add("[" + c.GetType().ToString() + "][" + c.Name + "][" + ((ComboBox)c).SelectedIndex + "]");
                }
                i++;
            }
            return lText;
        }

        //--------------------------------------------------------------
        //Fonction qui gére le chargmement de fichier, renvoi une list de string
        //
        //fichier -1 aucun fichier spécifier, sinon écrire le fichier genre save.txt
        //nblignenul 0 ne change rien sinon ne prend pas en compte les x première ligne
        //(note le fichier save ne dois pas prendre en compte la première ligne)
        //--------------------------------------------------------------
        private static List<string> Chargement_valeur(string fichier)
        {
            System.IO.StreamReader sr;
            String lActuel;
            List<string> chargement_valeur = new List<string>();//stock le fichier dans la variable sr
            if (fichier == "-1") // si aucun fichier spécifier
            {
                // Douvre le OpenFileDialog pour que l'utilisateur choissise le fichier à ouvrir
                OpenFileDialog openFileProg = new OpenFileDialog()
                {
                    Filter = "Programation des paramètres|*.save",
                    Title = "Sauvegarder tout les paramètres actuel",
                    Multiselect = false,//Empéche de choisir plusieur fichier
                    RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
                };

                if (openFileProg.ShowDialog() == System.Windows.Forms.DialogResult.OK)  //vérifie si un fichier a été sélectionné
                {
                    sr = new System.IO.StreamReader(openFileProg.FileName); //stock le fichier dans la variable sr
                }
                else // si la boite de dialogue n'a pas enregistré de valeur pertinente
                {
                    chargement_valeur.Clear();
                    chargement_valeur.Add("-1");
                    return chargement_valeur;
                }
            }
            else // si un fichier est spécifié
            {
                sr = new System.IO.StreamReader(fichier); //stock le fichier dans la variable sr
            }
            try
            {
                lActuel = sr.ReadLine(); // lit la ligne suivante dispo


                while (lActuel != "" && lActuel != null) //tant que lactuel n'est pas vide
                {
                    if (lActuel.Substring(0, 1) != "#") //test si le premier caractère n'est pas un #
                    {
                        chargement_valeur.Add(lActuel);  // rajoute la valeur qui ne commence pas par un #
                    }

                    lActuel = sr.ReadLine(); // lit la ligne suivante dispo

                }
                sr.Close();
                return chargement_valeur;
            }
            catch
            {
                chargement_valeur.Clear();
                chargement_valeur.Add("-1");
                return chargement_valeur;
            }
        }



        //----------------------------------------------------------------------
        //Fonction qui récupére tous les controls appel le chargement de fichier et met à jour les controls
        //
        //parent (normalement this) contien les controls
        //fichier, nom du fichier qui va contenir ses valeurs.
        //----------------------------------------------------------------------
        public static void Chargement(Control parent, string fichier)
        {
            List<String> chargement = new List<string>();
            List<Control> control_T = new List<Control>();
            string type;
            string nom;
            string etat;

            //Création de faux élément pour être utiliser dans l'appel.
            TextBox tb = new TextBox();
            CheckBox ch_b = new CheckBox();
            ComboBox co_b = new ComboBox();

            //Fonction qui stock dans une liste tout les éléments qui correspondent au type en deuxième paramètre
            control_T = (GValeurForm.FindAllControlForOneType(parent, tb.GetType(), control_T));
            control_T = (GValeurForm.FindAllControlForOneType(parent, ch_b.GetType(), control_T));
            control_T = (GValeurForm.FindAllControlForOneType(parent, co_b.GetType(), control_T));

            try
            {
                chargement = (Chargement_valeur(fichier));


                if (chargement[0] != "-1")
                {
                    for (int i = 0; i < chargement.Count; i++)
                    {
                        //découpage de chargement[i] pour mettre en forme le type, le nom de l'objet et la valeur qu'il doit prendre
                        type = chargement[i].Substring(1, chargement[i].IndexOf("]") - 1); // fonction qui prend qu'une partie du texte en cour, qui commence par [ et qui fini par ]
                        chargement[i] = chargement[i].Substring(chargement[i].IndexOf("][") + 1, chargement[i].Length - chargement[i].IndexOf("][") - 1); //fonction qui efface tout ce qu'il y a avant ][
                        nom = chargement[i].Substring(1, chargement[i].IndexOf("]") - 1); // fonction qui prend qu'une partie du texte en cour, qui commence par [ et qui fini par ]
                        chargement[i] = chargement[i].Substring(chargement[i].IndexOf("][") + 1, chargement[i].Length - chargement[i].IndexOf("][") - 1); //fonction qui efface tout ce qu'il y a avant ][
                        etat = chargement[i].Substring(1, chargement[i].IndexOf("]") - 1); // fonction qui prend qu'une partie du texte en cour, qui commence par [ et qui fini par ]

                        //détecte le type en question et change les paramètres suivant le fichier de sauvegarde.
                        if (type == "System.Windows.Forms.TextBox")
                        {
                            foreach (Control ct in control_T)
                            {
                                if (ct.Name == nom)
                                {
                                    ct.Text = etat;
                                    ((TextBox)ct).Select();
                                }
                            }
                        }
                        else if (type == "System.Windows.Forms.ComboBox")
                        {
                            foreach (Control ct in control_T)
                            {
                                if (ct.Name == nom)
                                {
                                    ((ComboBox)ct).SelectedIndex = int.Parse(etat);
                                }
                            }
                        }
                        else if (type == "System.Windows.Forms.CheckBox")
                        {
                            foreach (Control ct in control_T)
                            {
                                if (ct.Name == nom)
                                {
                                    ((CheckBox)ct).Checked = bool.Parse(etat);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Chargement échoué, attention certains paramètres sont peut être à corriger", "Chargement",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        public static void Sauvegarde(Control parent, string fichier)
        {
            List<Control> control_T = new List<Control>();

            //Création de faux élément pour être utiliser dans l'appel.
            TextBox tb = new TextBox();
            CheckBox ch_b = new CheckBox();
            ComboBox co_b = new ComboBox();

            //Fonction qui stock dans une liste tout les éléments qui correspondent au type en deuxième paramètre
            control_T = (GValeurForm.FindAllControlForOneType(parent, tb.GetType(), control_T));
            control_T = (GValeurForm.FindAllControlForOneType(parent, ch_b.GetType(), control_T));
            control_T = (GValeurForm.FindAllControlForOneType(parent, co_b.GetType(), control_T));

            Save_valeur(control_T, fichier);
        }
    }
}
