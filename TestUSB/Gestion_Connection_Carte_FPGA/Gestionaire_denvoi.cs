using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Connection_Carte_FPGA
{
    class Gestionaire_denvoi
    {
        private DataGridView_pour_FPGA DataGridView_FPGA;

        public Gestionaire_denvoi(DataGridView_pour_FPGA dgv)
        {
            this.DataGridView_FPGA = dgv;
        }

        public void Envoi_data_adresse(int adresse_de_départ,int nombre_dadresse)
        {
            int fin = adresse_de_départ + nombre_dadresse;
            for (int i = adresse_de_départ; i < fin; i++)
            {
                string adresse = DataGridView_FPGA.Rows[i].Cells[0].Value.ToString();
                string message = DataGridView_FPGA.Rows[i].Cells[1].Value.ToString();
                Gestion_Serveur.AsynchronousClient.Send_data(adresse + message);
            }
        }
    }
}
