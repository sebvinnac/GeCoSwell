using System;
using System.Windows.Forms;
using Gestion_Objet;

namespace GeCoSwell
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Fenetre1());
            }
            catch(Exception e)
            {
                GestionLog.Log_Write_Time(e.ToString());
            }
        }
    }
}
