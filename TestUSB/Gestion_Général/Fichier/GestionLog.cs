using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gestion_Objet
{
    static class GestionLog
    {
        #region Gestion log

        /// <summary>
        /// Écrit dans le log.
        /// </summary>
        /// <param name="text">le texte à écrire dans le log.</param>
        public static void Log_Write_Time(string text)
        {
            try
            {
                FileInfo fichier = new FileInfo("log.txt");
                StreamWriter sw = new StreamWriter("log.txt", true, System.Text.Encoding.ASCII);
                sw.WriteLine(DateTime.Now + " : " + text + Environment.NewLine);
                sw.Close();
            }
            catch (Exception e)
            {
                Log_Write_Time(e.ToString());
            }
        }


        /// <summary>
        /// Ouvre le fichier log si existant
        /// </summary>
        public static void Log_Open()
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                // Nom du fichier dont l'extension est connue du shell à ouvrir
                proc.StartInfo.FileName = "log.txt";
                proc.Start();
                proc.Close();
            }
            catch
            {
                Log_Write_Time("Log inexistant");
            }
        }
        #endregion
    }
}
