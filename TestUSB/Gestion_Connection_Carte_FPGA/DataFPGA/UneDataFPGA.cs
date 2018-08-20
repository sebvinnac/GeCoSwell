using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Connection_Carte_FPGA
{
    class UneDataFPGA
    {
        public string Adresse { get; set; }
        public string Valeur { get; set; }
        public string Utilité_de_cette_valeur { get; set; }
        public bool Type_que_lon_peux_send { get; set; }

        public UneDataFPGA(string adresse, string explication,bool typeasend = true)
        {
            this.Adresse = adresse;
            this.Utilité_de_cette_valeur = explication;
            this.Type_que_lon_peux_send = typeasend;
        }


    }
}
