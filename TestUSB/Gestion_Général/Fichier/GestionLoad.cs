using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gestion_Objet
{
    class GestionLoad : GestionFichier, IDisposable
    {
        private StreamReader Sr;

        #region Constructeur
        /// <summary>
        /// Pour choisir et charger un fichier
        /// </summary>
        /// <param name="filtre">Permet de definir un text et un titre ex "Programation des paramètres|*.txt"</param>
        /// <param name="titre">Permet de donner un titre à la fenetre ouverture exemple "Chargement des paramètres"</param>
        public GestionLoad(String filtre,String titre)
        {
            this.Filtre = filtre;
            this.Titre = titre;
        }

        /// <summary>
        /// Chargement d'un fichier, plus pour l'automatique
        /// </summary>
        public GestionLoad()
        {

        }

        #endregion

        #region Chargement

        /// <summary>
        /// récupére tous les controls appel le chargement de fichier et met à jour les controls
        /// </summary>
        /// <param name="parent">contien les controls</param>
        /// <param name="fichier">fichier qui va contenir ses valeurs. sinon va ouvrir la fenetre de recherche de fichier</param>
        public void Chargement(Control parent, string fichier = "")
        {
            List<String> chargement = new List<string>();

            //Fonction qui stock dans une liste tout les éléments qui correspondent au type en deuxième paramètre
            List<Control> control_T = GestionObjet.Trouver_controls_dun_type(parent, new List<Type>() { typeof(TextBox), typeof(CheckBox), typeof(ComboBox) });

            try
            {
                chargement = (Chargement_valeur(fichier));


                if (chargement[0] != "-1")
                {
                    for (int i = 0; i < chargement.Count; i++)
                    {
                        this.Traitement_des_donnés(chargement[i], control_T);
                    }
                }
                this.Dispose();
            }
            catch
            {
                MessageBox.Show("Chargement échoué, attention certains paramètres sont peut être à corriger", "Chargement",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// Permet de définir l'emplacement du fichier à ouvrir
        /// </summary>
        private void ChoixEmplacement(string fichier)
        {
            if (fichier == "") // si aucun fichier spécifier
            {
                OpenFileDialog openFileProg = new OpenFileDialog()
                {
                    Filter = this.Filtre,
                    Title = this.Titre,
                    Multiselect = false,//Empéche de choisir plusieur fichier
                    RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
                };

                if (openFileProg.ShowDialog() == System.Windows.Forms.DialogResult.OK)  //vérifie si un fichier a été sélectionné
                {
                    Sr = new System.IO.StreamReader(openFileProg.FileName); //stock le fichier dans la variable sr
                }
            }

            else // si un fichier est spécifié
            {
                Sr = new System.IO.StreamReader(fichier); //stock le fichier dans la variable sr
            }
        }

        /// <summary>
        /// Gére le chargement du fichier, et renvoi une liste de string
        /// </summary>
        /// <param name="fichier">si "" ouvre la fenetre d'ouverture, sinon contient le chemin du fichier</param>
        /// <returns>List de string chaque string correspond à un objet avec type nom valeur</returns>
        private List<string> Chargement_valeur(string fichier)
        {
            List<string> chargement_valeur = new List<string>();//stock le fichier dans la variable sr
            this.ChoixEmplacement(fichier);
            try
            {
                String lActuel = Sr.ReadLine(); // lit la ligne suivante dispo
                while (lActuel != "" && lActuel != null) //tant que lactuel n'est pas vide
                {
                    if (lActuel.Substring(0, 1) != "#") //test si le premier caractère n'est pas un #
                    {
                        chargement_valeur.Add(lActuel);  // rajoute la valeur qui ne commence pas par un #
                    }

                    lActuel = Sr.ReadLine(); // lit la ligne suivante dispo

                }
                Sr.Close();
                return chargement_valeur;
            }
            catch
            {
                chargement_valeur.Clear();
                chargement_valeur.Add("-1");
                return chargement_valeur;
            }
        }

        /// <summary>
        /// Utilise une ligne mis en forme d'enregistrement et met à jour l'objet correspondant
        /// </summary>
        /// <param name="chargement">Text qui viens d'une ligne d'un fichier sauvegarder</param>
        /// <param name="control_T">Liste des controls</param>
        private void Traitement_des_donnés(String chargement, List<Control> control_T)
        {
            //découpage de chargement pour mettre en forme le type, le nom de l'objet et la valeur qu'il doit prendre
            string type = chargement.Substring(1, chargement.IndexOf("]") - 1); // fonction qui prend qu'une partie du texte en cour, qui commence par [ et qui fini par ]
            chargement = chargement.Substring(chargement.IndexOf("][") + 1, chargement.Length - chargement.IndexOf("][") - 1); //fonction qui efface tout ce qu'il y a avant ][
            string nom = chargement.Substring(1, chargement.IndexOf("]") - 1); // fonction qui prend qu'une partie du texte en cour, qui commence par [ et qui fini par ]
            chargement = chargement.Substring(chargement.IndexOf("][") + 1, chargement.Length - chargement.IndexOf("][") - 1); //fonction qui efface tout ce qu'il y a avant ][
            string etat = chargement.Substring(1, chargement.IndexOf("]") - 1); // fonction qui prend qu'une partie du texte en cour, qui commence par [ et qui fini par ]

            //détecte le type en question et change les paramètres suivant le fichier de sauvegarde.
            GestionObjet.Trouver_et_Appliquer(nom, etat, control_T);
        }
        #endregion

        #region Gestion_du_disposable


        bool disposed = false; // pour savoir si la classe est libérée


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.Sr.Dispose();
                    this.Sr = null;
                }
                disposed = true;
            }
        }

        #endregion

    }
}
