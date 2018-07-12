using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphiqueOsci
{
    class Graphique : Chart
    {
        private Legend legend = new Legend();
        private List<Block_de_Graphique> l_bd_graph = new List<Block_de_Graphique>();
        private Title titre = new Title();
        private Point location;
        private Size taille;
        private static int nombre_de_graph = 0;
        private List<Series> lseries;
        private int maxdepoint = 1000;

        public Graphique(int nombre_courbe,Point location,Size taille)
        {

            this.Gestion_legend(legend);


            this.location = location;
            this.Position(location);
            this.Taille(taille);

            this.Name = "chart1";
            this.Text = "chart1";


            //series ou courbe
            lseries = new List<Series>();



            //création du premier chartarea
            ChartArea zoneg = new ChartArea();
            Addgraph(zoneg);

            this.Gestion_titre();
            this.Gestion_border();
        }

        #region Affichage
        private void Gestion_titre()
        {
            //Titre
            this.titre.Name = "titre";
            this.titre.Text = "Mesures en cours";
            this.titre.Font = new Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titre.ForeColor = Color.Red;
            this.Titles.Add(titre);//rajout du titre sur le graphique
        }
        private void Gestion_legend(Legend leg)
        {

            leg.Title = "légende";
            leg.Name = "legend";
            leg.BackColor = Color.WhiteSmoke;
            leg.BackSecondaryColor = Color.White;
            leg.BackGradientStyle = GradientStyle.DiagonalLeft;
            leg.BorderColor = Color.Black;
            leg.BorderWidth = 2;
            leg.TitleSeparator = LegendSeparatorStyle.Line;
            leg.BorderDashStyle = ChartDashStyle.Solid;
            leg.ShadowOffset = 2;
            leg.Alignment = StringAlignment.Center;
            leg.InsideChartArea = "";
            leg.Docking = Docking.Bottom;

            // Add header separator of type line
            leg.HeaderSeparator = LegendSeparatorStyle.Line;
            leg.HeaderSeparatorColor = Color.Gray;

            // Add Color column
            LegendCellColumn firstColumn = new LegendCellColumn()
            {
                ColumnType = LegendCellColumnType.SeriesSymbol,
                HeaderText = "Color",
                HeaderBackColor = Color.WhiteSmoke
            };
            leg.CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn()
            {
                ColumnType = LegendCellColumnType.Text,
                HeaderText = "Name",
                Text = "#LEGENDTEXT",
                HeaderBackColor = Color.WhiteSmoke
            };
            leg.CellColumns.Add(secondColumn);

            // Add AVG cell column
            LegendCellColumn avgColumn = new LegendCellColumn()
            {
                Text = "#AVG{N2}",
                HeaderText = "Avg",
                Name = "AvgColumn",
                HeaderBackColor = Color.WhiteSmoke
            };
            leg.CellColumns.Add(avgColumn);

            // Add Max cell column
            LegendCellColumn maxColumn = new LegendCellColumn()
            {
                Text = "#MAX{N1}",
                HeaderText = "Max",
                Name = "Maxcolumn",
                HeaderBackColor = Color.WhiteSmoke

            };
            leg.CellColumns.Add(maxColumn);

            // Set Min cell column attributes
            LegendCellColumn minColumn = new LegendCellColumn()
            {
                Text = "#MIN{N1}",
                HeaderText = "Min",
                Name = "MinColumn",
                HeaderBackColor = Color.WhiteSmoke
            };
            leg.CellColumns.Add(minColumn);

            this.Legends.Add(leg);
        }

        

        private void Gestion_border()
        {
            this.BorderSkin.SkinStyle = BorderSkinStyle.Raised;
            this.BorderSkin.BorderDashStyle = ChartDashStyle.Solid;
            this.BorderSkin.BackColor = Color.MediumBlue;
            this.BorderSkin.BorderWidth = 1;
        }
        


        #endregion

        //move the graph
        public void Position(Point location)
        {
            this.location = location;
            this.Location = this.location;
        }

        //change the size of graph
        public  void Taille(Size taille)
        {
            this.taille = taille;
            this.Size = this.taille;
        }

        //Add a graph
        public void Addgraph(ChartArea zone)
        {
            nombre_de_graph++;
            Block_de_Graphique zonet = new Block_de_Graphique();
            this.ChartAreas.Add(zone);
            l_bd_graph.Add(zonet);
        }

        //return le nombre de graph
        public int Nombre_de_graph()
        {
            return nombre_de_graph;
        }

        //pour mettre à jour l'affichage
        public void Miseajouraffichage()
        {
            this.Invalidate();
        }

        #region Gestion des séries avec leurs point


        //rajoute des points sur une series
        public void Point_rajout(int num_de_serie, Point point)
        {
            if (num_de_serie < lseries.Count)
            {
                lseries[num_de_serie].Points.AddXY(point.X, point.Y);
                Limite_nb_point(num_de_serie);
            }
        }

        //rajoute des points sur une series
        public void Point_rajout(int num_de_serie, double x, double y)
        {
            if (num_de_serie < lseries.Count)
            {
                lseries[num_de_serie].Points.AddXY(x, y);
                Limite_nb_point(num_de_serie);
            }
        }

        //rajoute des points sur une series
        public void List_Point_rajout(int num_de_serie, List<Point> point)
        {
            if (num_de_serie < lseries.Count)
            {
                foreach (Point p in point)
                {
                    lseries[num_de_serie].Points.AddXY(p.X, p.Y);
                    Limite_nb_point(num_de_serie);
                }
            }
        }

        //retire les premiers point pour respecter le nombre max de point
        private void Limite_nb_point(int num_de_serie)
        {
            while (lseries[num_de_serie].Points.Count > maxdepoint)
            {
                lseries[num_de_serie].Points.RemoveAt(0);
            }
        }

        public void ChangeMaxPoint(int nb_de_point)
        {
            maxdepoint = nb_de_point;
        }

        //add series
        public void Serie_rajout(Series series)
        {
            //détecte si cette série existe déjà
            if (Find_serie(series) == -1)
            {
                series.ChartType = SeriesChartType.FastLine;
                this.Series.Add(series);
                this.lseries.Add(series);
                this.Series[lseries.Count - 1].Legend = "legend";
                //this.legend.CustomItems
            }
        }

        //retire toute les series
        public void Serie_removeAll()
        {
            this.Series.Clear();
            lseries.RemoveRange(0,lseries.Count);
            for(int i = 0; i < l_bd_graph.Count; i++)
            {
                Init_Axe(i);
            }
        }

        public List<Series> Recup_serie()
        {
            return lseries;
        }

        //donne le nombres de series
        public int Serie_nombre()
        {
            return lseries.Count;
        }

        //met à jour l'axe X
        public void MAJ_axeX(int num_de_serie,int numchartarea)
        {
            if (num_de_serie < this.lseries.Count && numchartarea < this.l_bd_graph.Count)
            {
                this.l_bd_graph[numchartarea].MAJ_axeX(this.lseries[num_de_serie]);
            }
        }

        public void Init_Axe(int numChartarea)
        {
            if (numChartarea < this.l_bd_graph.Count)
            {
                this.l_bd_graph[numChartarea].Init_axe();
            }
        }

        public void Nom_axe(string nomx, string nomy,int num_de_zone_de_graphique)
        {
            if (num_de_zone_de_graphique < l_bd_graph.Count)
            {
                l_bd_graph[num_de_zone_de_graphique].Nom_axe(nomx, nomy);
            }
        }


        //----------------------------------------------------------------------
        private int Find_serie(Series series)
        {
            for (int i = 0; i < lseries.Count; i++)
            {
                if (lseries[i].Name == series.Name)
                    return i;

            }
            return -1; // si rien trouvé   
        }

        #endregion
    }
}
