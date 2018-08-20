using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GraphiqueOsci;
using Gestion_Objet;

namespace GeCoSwell
{
    public partial class MesureOsci : Form
    {
        Graphique graphique1;

        //real time
        private Thread addDataRunner;
        private Random rand = new Random();
        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;
        private bool mesureencour;

        public MesureOsci()
        {
            InitializeComponent();
            this.Init_graph();

            this.Closing += new CancelEventHandler(this.MainForm_Closing);//fonction quand la fenetre se ferme
        }

        private void Init_graph()
        {
            //créer le chart et lui donne tout les paramètres important
            graphique1 = new Graphique(2,new Point(0, 0), new Size(800, 600));

            this.Controls.Add(graphique1);//le rajout à la fenetre
            
        }


        //créer le délégate, init la fenetre
        private void MesureOsci_Load(object sender, EventArgs e)
        {            
            // create a delegate for adding data
            addDataDel += new AddDataDelegate(AddData);
        }


        //ferme le thread avant de fermer la fenetre
        private void MainForm_Closing(Object sender, CancelEventArgs e)
        {
            if (addDataRunner != null)
            if (addDataRunner.IsAlive == true)
            {
                addDataRunner.Abort();
            }
        }

        #region Realtime

        int index_tab_sinus = 1;
        double quantum_temps;
        int nb_serie;
        private void B_Mesure_Click(object sender, System.EventArgs e)
        {
            ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            addDataRunner = new Thread(addDataThreadStart);

            // Disable all controls on the form
            b_Mesure.Enabled = false;
            // and only Enable the Stop button
            b_stop.Enabled = true;
            b_save_point.Enabled = true;


            //donne les noms des axes
            graphique1.Nom_axe("Durée en µs", "Valeur en 10Bits", 0);

            //initialisation du tableau dans le fpga
            //initialise le tableau à la première valeur;
            index_tab_sinus = 1;
            //GCfpga.Send_Param("tb_val_63", index_tab_sinus.ToString(), 1, 1);
            quantum_temps = Calcul_duree_entre_point_micros();

            //retire toute les autre série en rajoute 1
            graphique1.Serie_removeAll();
            graphique1.Serie_rajout(new Series("CAN1"));
            nb_serie = graphique1.Serie_nombre();


            // start worker threads.
            if (addDataRunner.IsAlive == false)
            {
                addDataRunner.Start();
            }

        }

        //Donne la durée entre deux point en µs
        private double Calcul_duree_entre_point_micros()
        {
            double result;

            //if (div_freq == 1025)
            //    div_freq = 1;

            //msg_recu = GCfpga.Send_Param("tb_val_32", "0", 11, 1);
            //Double max_triangle = double.Parse(GCfpga.Conv_1023_vers_10bits(msg_recu.Substring(msg_recu.Length - 13, 10), false, 1));

            //return result = Math.Round(1 / (200/max_triangle/div_freq),2);//en µs 200M /100 000 pour avoir µs /2 car deux mesure par triangle
            return 2;
        }

        private void B_stop_click(object sender, System.EventArgs e)
        {
           
            mesureencour = false;

            // Enable all controls on the form
            b_Mesure.Enabled = true;
            // and only Disable the Stop button
            b_stop.Enabled = false;
        }

        // Gere l'invoke du chart, et l'arret
        private void AddDataThreadLoop()
        {
            mesureencour = true;
            while (mesureencour)
            {
                if (graphique1.IsDisposed || (!graphique1.IsHandleCreated && !graphique1.FindForm().IsHandleCreated))
                {
                    mesureencour = false;
                }
                else
                {
                    try
                    {
                        graphique1.Invoke(addDataDel);
                    }
                    catch
                    {
                        mesureencour = false;
                    }
                }
            }

            //si on sort de la boucle on doit stopper le thread
            if (addDataRunner.IsAlive == true)
            {
                addDataRunner.Abort();
            }

            // Enable all controls on the form
            b_Mesure.Enabled = true;
            // and only Disable the Stop button
            b_stop.Enabled = false;
        }

        
        public void AddData()
        {
            for (int i = 0; i < nb_serie;  i++)
            {
                AddNewPoint(i);
            }
            graphique1.Invalidate();
        }


        public void AddNewPoint(int num_de_serie_affecte)
        {
            /*
            //return les 10 dernier bits retourner par send et les convertie en décimal
            string msg_recu = GCfpga.Send_Param("tb_val_64", index_tab_sinus.ToString(), 11, 1);
            if (msg_recu != "-1")//si erreur dans le transfert
            {
                if (index_tab_sinus != 1)
                {
                    Double val = double.Parse(GCfpga.Conv_1023_vers_10bits(msg_recu.Substring(msg_recu.Length - 13, 10), false, 1));
                    //ptSeries.Points.AddXY(nbpoint, val);
                    graphique1.Point_rajout(num_de_serie_affecte, (index_tab_sinus - 2) * quantum_temps, val);
                    graphique1.MAJ_axeX(num_de_serie_affecte, 0);
                }
            }
            if (index_tab_sinus == 1000)
            {
                mesureencour = false;
            }
            index_tab_sinus++;
            */
        }
        #endregion

        private void B_save_point_Click(object sender, EventArgs e)
        {
            GestionSave save = new GestionSave("Fichier texte|*.txt", "Sauvegarde des points pour Excel");
            save.Ecrire(this.graphique1.Recup_serie());
            
        }
    }
}
