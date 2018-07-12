using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphiqueOsci
{
    class Block_de_Graphique : ChartArea
    {

        public Block_de_Graphique()
        {
            this.Name = "ChartArea1";
            this.Gestion_scrollbar();
            this.Gestion_axe();

        }

        #region init_Affichage
        private void Gestion_scrollbar()
        {
            this.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            this.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AxisX.ScrollBar.BackColor = System.Drawing.Color.White;
            this.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black;
            this.AxisX.ScrollBar.Size = 12;
            this.CursorX.IsUserEnabled = true;
            this.CursorX.IsUserSelectionEnabled = true;
            this.AxisX.ScaleView.Zoomable = true;
        }

        private void Gestion_axe()
        {
            this.AxisY.LineColor = Color.DarkMagenta;
            this.AxisX.LineColor = Color.DarkMagenta;

            // Set Axis Line Style
            this.AxisY.LineDashStyle = ChartDashStyle.Solid;
            this.AxisX.LineDashStyle = ChartDashStyle.Solid;

            // Set Arrow Style
            this.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            this.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            // Set Line Width
            this.AxisY.LineWidth = 1;
            this.AxisX.LineWidth = 1;


        }
        #endregion

        public void Nom_axe(string nomx, string nomy)
        {
            this.AxisX.Title = nomx;
            this.AxisX.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            this.AxisX.TitleForeColor = Color.Red;


            this.AxisY.Title = nomy;
            this.AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            this.AxisY.TitleForeColor = Color.Red;
        }

        //met à jour les axe
        //series_a_test, série qui va tester si on dépasse les limites
        public void MAJ_axeX(Series series_a_test)
        {
                this.AxisX.Minimum = Math.Max(this.AxisX.Minimum, series_a_test.Points[0].XValue);
                this.AxisX.Maximum = Math.Max(this.AxisX.Maximum, series_a_test.Points[series_a_test.Points.Count - 1].XValue);
        }

        public void Init_axe()
        {
            this.AxisX.Minimum = Double.NaN;
            this.AxisX.Maximum = Double.NaN;
            this.AxisY.Minimum = Double.NaN;
            this.AxisY.Maximum = Double.NaN;
        }


    }
}
