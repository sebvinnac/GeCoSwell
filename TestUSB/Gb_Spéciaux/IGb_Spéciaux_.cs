using DataFPGA;
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

        void Nouvelle_donné(object sender, EventArgs e);

        List<String> Récup_donné();

        void Init_Datafpga(DataGridView_pour_FPGA data_pour_FPGA);
        int MAJ_Datafpga(List<UneDataFPGA> data, int index);
    }
}
