using Gestion_Connection_Carte_FPGA;
using Gestion_Objet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeCoSwell
{
    interface IGB_Spéciaux
    {
        bool EstVisible { get; }
        bool A_changé { get; }
        int Index_de_départ_du_DGV { get; }
        int Nombre_dadresse { get; }

        void Nouvelle_donné(object sender, EventArgs e);

        List<String> Récup_donné();

        int Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA, int index_de_départ);
        void Lié_li_data(List<UneDataFPGA> data);
        void MAJ_Datafpga();
    }
}
