using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gestion_Objet;

namespace GeCoSwell
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            this.Chargement_des_Options();
            this.tb_quartus_stpexe.Select();
            this.tb_TCL.Select();
            this.tb_quartus_stpexe.Select();
        }


        /// <summary>
        /// Charge les valeur du fichier Option.ini et met à jour l'interface en correspondance
        /// </summary>
        private void Chargement_des_Options()
        {
            GestionLoad chargement = new GestionLoad();
            chargement.Chargement(this.tb_quartus_stpexe, "Options.ini");
            chargement.Chargement(this.tb_TCL, "Options.ini");
            chargement.Chargement(this.chb_AutoLoad, "Options.ini");
            chargement.Chargement(this.chb_ModeDaltonien, "Options.ini");
            chargement.Chargement(this.chb_valid_expert, "Options.ini");
        }

        //----------------------------------------------------------------------
        //Fonction qui ouvre l'explorateur de fichier pour trouver quartus_stp.exe
        //----------------------------------------------------------------------
        private void B_Chercherquartus_stp_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofn = new OpenFileDialog()
            {
                Filter = "Quartus executable (Quartus_stp.exe)|Quartus_stp.exe",
                Title = "Indiquer l'emplacement de Quartus_stp.exe",
                Multiselect = false,//Empéche de choisir plusieur fichier
                RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
            };

            if (ofn.ShowDialog() == DialogResult.OK)// si un fichier est trouvé
            {
                this.tb_quartus_stpexe.Text = ofn.FileName;// écrire l'adresse du fichier au bon endroit
                this.l_info_quartus.Text = "Fichier trouvé";
            }
        }

        //----------------------------------------------------------------------
        //Fonction pour fermer la fenetre sans prendre en compte les changements
        //----------------------------------------------------------------------
        private void B_param_annuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //----------------------------------------------------------------------
        //Fonction qui ouvre l'explorateur de fichier pour trouver *.tcl
        //----------------------------------------------------------------------
        private void B_ChercherTCL_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofn = new OpenFileDialog()
            {
                Filter = "Script TCL(*.tcl)|*.tcl",
                Title = "Indiquer l'emplacement du script *.tcl",
                Multiselect = false,//Empéche de choisir plusieur fichier
                RestoreDirectory = true //conserve l'emplacement de la dernière ouverture du fichier
            };
            if (ofn.ShowDialog() == DialogResult.OK)// si un fichier est trouvé
            {
                this.tb_TCL.Text = ofn.FileName;// écrire l'adresse du fichier au bon endroit
                this.l_info_TCL.Text = "Fichier trouvé";
            }
        }

        //----------------------------------------------------------------------
        //Fonction qui enregistres les options
        //----------------------------------------------------------------------
        private void B_param_ok_Click(object sender, EventArgs e)
        {
            GestionSave.SauvegardeAuto(this, "Options.ini");

            this.Close();
        }

        //----------------------------------------------------------------------
        //Fonction qui détecte si le chemin spécifié dans la texte box pointe sur un fichier
        //----------------------------------------------------------------------
        private void Tb_fileexist_Validating(object sender, CancelEventArgs e)
        {
            if (System.IO.File.Exists(((TextBox)sender).Text))
            {
                if (((TextBox)sender) == this.tb_quartus_stpexe)
                    this.l_info_quartus.Text = "Fichier trouvé";
                else
                    this.l_info_TCL.Text = "Fichier trouvé";
            }
            else
            {
                if (((TextBox)sender) == this.tb_quartus_stpexe)
                    this.l_info_quartus.Text = "Fichier introuvable";
                else
                    this.l_info_TCL.Text = "Fichier introuvable";
            }
        }

    }
}
