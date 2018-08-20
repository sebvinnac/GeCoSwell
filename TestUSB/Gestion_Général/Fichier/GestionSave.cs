using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gestion_Objet
{
    class GestionSave : GestionFichier
    {
        private StreamWriter Sw;

        #region Constructeur

        /// <summary>
        /// Objet pour la gestion de l'enregistrement
        /// </summary>
        /// <param name="filtre">Texte et filtre exemple "Programation des paramètres|*.txt"</param>
        /// <param name="titre">Titre de la fenetre exemple "Sauvegarder tout les paramètres actuel"</param>
        /// <param name="chemin">Emplacement si connu</param>
        /// <param name="append">true = on continu d'écrire dans le fichier existant</param>
        public GestionSave(string filtre, string titre, string chemin = "", bool append = false)
        {
            this.Filtre = filtre;
            this.Titre = titre;
            this.Chemin = chemin;
            this.Append = append;
        }

        /// <summary>
        /// Objet pour la gestion de l'enregistrement automatique
        /// </summary>
        /// <param name="chemin">Emplacement si connu</param>
        /// <param name="append">true = on continu d'écrire dans le fichier existant</param>
        public GestionSave(string chemin, bool append)
        {
            this.Filtre = "";
            this.Titre = "";
            this.Chemin = chemin;
            this.Append = append;
        }

        /// <summary>
        /// Objet pour la gestion de l'enregistrement
        /// </summary>
        public GestionSave()
        {
            this.Filtre = "";
            this.Titre = "";
            this.Chemin = "";
            this.Append = false;
        }
        #endregion

        #region Enregistrement

        /// <summary>
        /// Permet de choisir le chemin ou sera enregistré le fichier et créer le streamwriter correspondant
        /// </summary>
        private void ChoixEmplacement()
        {
            if (this.Chemin != "")
            {
                Sw = new StreamWriter(this.Chemin, this.Append);
            }
            else
            {
                // Douvre le OpenFileDialog pour que l'utilisateur choissise le fichier à ouvrir
                SaveFileDialog saveFileProg = new SaveFileDialog()
                {
                    Filter = Filtre,
                    Title = Titre,
                    RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
                };
                saveFileProg.ShowDialog();

                if (saveFileProg.FileName != "") // si le nom du fichier n'est pas vide
                {
                    Sw = new StreamWriter(saveFileProg.OpenFile());
                }
            }
        }

        /// <summary>
        /// Trouve tout les objets de type "les_types" inclut dans le controls et les enregistre
        /// </summary>
        /// <param name="les_types">list des types d'objet à enregistrer utiliser typeof(textbox)</param>
        /// <param name="parent">control qui contient les objets</param>
        public void Ecrire(Control parent, List<Type> les_types)
        {
            this.ChoixEmplacement();
            this.Ecrire(EncodeControl(GestionObjet.Trouver_controls_dun_type(parent, les_types)));
        }

        /// <summary>
        /// écrit les listes de text dans le fichier
        /// </summary>
        /// <param name="l_test">liste contenant les paramètres a stocker</param>
        public void Ecrire(List<String> l_test)
        {
            try
            {
                Sw.WriteLine("#Fichier généré par GeCoSwell de vinnac seb");
                // pour chaque "line" dans le résultat de l'encodage on l'écrit dans le fichier
                foreach (String line in l_test)
                {
                    Sw.WriteLine(line);
                }
                Sw.Close();
            }
            catch
            {
                MessageBox.Show("Sauvegarde échoué", "Sauvegarde",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            this.Dispose();

        }

        /// <summary>
        /// Enregistre les points des courbes
        /// </summary>
        /// <param name="lseries">liste des points</param>
        public void Ecrire(List<Series> lseries)
        {
            this.Filtre = "Fichier texte|*.txt";
            this.Titre = "Sauvegarde des points pour Excel";
            this.ChoixEmplacement();
            if (Sw != null)
            {
                List<string> lstring = new List<string>();
                lstring = Encode_series(lseries);
                if (lstring.Any())
                {
                    Ecrire(lstring);
                }
            }
            this.Dispose();
        }

        #endregion

        #region Méthode static
        /// <summary>
        /// Sauvegarde les objets trouvé dans parent, à l'emplacement fichier
        /// </summary>
        /// <param name="parent">le contenant ou l'on va chercher les textbox, combobox et checkbox</param>
        /// <param name="fichier">emplacement du fichier</param>
        public static void SauvegardeAuto(Control parent, string fichier)
        {
            List<Control> control_T = GestionObjet.Trouver_controls_dun_type(parent, new List<Type>() { typeof(TextBox), typeof(CheckBox), typeof(ComboBox) });

            GestionSave saveauto = new GestionSave(fichier, false);
            saveauto.ChoixEmplacement();
            saveauto.Ecrire(GestionFichier.EncodeControl(control_T));
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
                    this.Sw.Dispose();
                    this.Sw = null;
                }
                disposed = true;
            }
        }

        #endregion

    }
}
