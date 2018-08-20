using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphiqueOsci
{
    class Block_de_Graphique
    {
        private ChartArea chartArea_local = new ChartArea();

        public Block_de_Graphique(ChartArea zone)
        {
            chartArea_local = zone;
            chartArea_local.Name = "ChartArea1";
            this.Gestion_scrollbar();
            this.Gestion_axe();
        }

        #region init_Affichage
        private void Gestion_scrollbar()
        {
            chartArea_local.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea_local.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea_local.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea_local.AxisX.ScrollBar.BackColor = System.Drawing.Color.White;
            chartArea_local.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea_local.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black;
            chartArea_local.AxisX.ScrollBar.Size = 12;
            //ZoomX
            chartArea_local.CursorX.IsUserEnabled = true;
            chartArea_local.CursorX.IsUserSelectionEnabled = true;
            chartArea_local.AxisX.ScaleView.Zoomable = true;
            chartArea_local.AxisX.ScrollBar.IsPositionedInside = true;
            chartArea_local.CursorX.Interval = 1;//on peut mettre 0.1 ou 0.01 pour accepter des zoom plus précis
            //zoomy

            chartArea_local.CursorY.IsUserEnabled = true;
            chartArea_local.CursorY.IsUserSelectionEnabled = true;
            chartArea_local.AxisY.ScaleView.Zoomable = true;
            chartArea_local.AxisY.ScrollBar.IsPositionedInside = true;
            chartArea_local.AxisY.IsMarginVisible = false;
            chartArea_local.CursorY.Interval = 1;
        }

        private void Gestion_axe()
        {
            chartArea_local.AxisY.LineColor = Color.DarkMagenta;
            chartArea_local.AxisX.LineColor = Color.DarkMagenta;

            // Set Axis Line Style
            chartArea_local.AxisY.LineDashStyle = ChartDashStyle.Solid;
            chartArea_local.AxisX.LineDashStyle = ChartDashStyle.Solid;

            // Set Arrow Style
            chartArea_local.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea_local.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            // Set Line Width
            chartArea_local.AxisY.LineWidth = 1;
            chartArea_local.AxisX.LineWidth = 1;


        }
        #endregion

        public void Nom_axe(string nomx, string nomy)
        {
            chartArea_local.AxisX.Title = nomx;
            chartArea_local.AxisX.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chartArea_local.AxisX.TitleForeColor = Color.Red;
            chartArea_local.AxisX.TitleAlignment = StringAlignment.Near;


            chartArea_local.AxisY.Title = nomy;
            chartArea_local.AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chartArea_local.AxisY.TitleForeColor = Color.Red;
            chartArea_local.AxisY.TitleAlignment = StringAlignment.Near;
        }

        //met à jour les axe
        //series_a_test, série qui va tester si on dépasse les limites
        public void MAJ_axeX(Series series_a_test)
        {
            chartArea_local.AxisX.Minimum = Math.Max(chartArea_local.AxisX.Minimum, series_a_test.Points[0].XValue);
            chartArea_local.AxisX.Maximum = Math.Max(chartArea_local.AxisX.Maximum, series_a_test.Points[series_a_test.Points.Count - 1].XValue);
        }

        public void Init_axe()
        {
            chartArea_local.AxisX.Minimum = Double.NaN;
            chartArea_local.AxisX.Maximum = Double.NaN;
            chartArea_local.AxisY.Minimum = Double.NaN;
            chartArea_local.AxisY.Maximum = Double.NaN;
        }


    }
}
