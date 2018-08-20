using Gestion_Connection_Carte_FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeCoSwell;
using Gestion_Objet;

namespace Gestion_Connection_Carte_FPGA
{
    static class MAJ_DATA_pour_Envoi
    {

        static private Etat_de_connection Etat_co;
        static private List<IGB_Spéciaux> Li_Gb_Spéciaux;
        static private List<UneDataFPGA> Li_data;

        public static void Lier_à_état_co(Etat_de_connection etat_co)
        {
            Etat_co = etat_co;
        }
        public static void Lier_à_li_Gb_Spéciaux(List<IGB_Spéciaux> li_Gb_Spéciaux)
        {
            Li_Gb_Spéciaux = li_Gb_Spéciaux;
        }
        public static void Lier_à_li_data(List<UneDataFPGA> data)
        {
            Li_data = data;
        }

        

        public static void MAJ_Data_puis_envoi(Gestionaire_denvoi gestion_envoi)
        {
            Etat_co.Etat_de_connection_actuel = 20;//envoi en cour
            foreach (IGB_Spéciaux gb in Li_Gb_Spéciaux)
            {
                if (gb.A_changé)
                {
                    gb.MAJ_Datafpga();
                    try
                    {
                        gestion_envoi.Envoi_data_adresse(gb.Index_de_départ_du_DGV, gb.Nombre_dadresse);
                    }
                    catch (Exception e)
                    {
                        Etat_co.Etat_de_connection_actuel = 14;
                        GestionLog.Log_Write_Time(e.ToString());
                    }
                }
            }
            Etat_co.Etat_de_connection_actuel = 12; // état de l'envoi terminé
        }
    }
}
