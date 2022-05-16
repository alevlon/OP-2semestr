using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Math;

namespace prac_2
{
    /// <summary>
    /// Логика взаимодействия для GeneticWindow.xaml
    /// </summary>
    public partial class GeneticWindow : Window
    {
        static DispatcherTimer dT;
        static int Radius = 15;
        static int PointCount = 5;
        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List<Ellipse>();
        static PointCollection pC = new PointCollection();
        static double percentMut = 0.9;
        List<List<int>> Population = new List<List<int>>();
        List<double> Roads = new List<double>();
        static int sizePopulation = 10;

        public GeneticWindow()
        {
            dT = new DispatcherTimer();

            CreateRandomPopulation(Population);
           

            InitializeComponent();
            InitPoints();
            InitPolygon();

            CalculateRoads(Population);
            dT = new DispatcherTimer();
            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1000);
        }
        private void InitPoints()
        {
            Random rnd = new Random();
            pC.Clear();
            EllipseArray.Clear();
            myPolygon = new Polygon();

            for (int i = 0; i < PointCount; i++)
            {
                Point p = new Point();

                p.X = rnd.Next(Radius, (int)(0.75 * GeneticAlgWindow.Width) - 3 * Radius);
                p.Y = rnd.Next(Radius, (int)(0.90 * GeneticAlgWindow.Height - 3 * Radius));
                pC.Add(p);
            }

            for (int i = 0; i < PointCount; i++)
            {
                Ellipse el = new Ellipse();

                el.StrokeThickness = 3;
                el.Height = el.Width = Radius;
                el.Stroke = Brushes.Red;
                el.Fill = Brushes.Yellow;
                EllipseArray.Add(el);
            }
        }

        private void InitPolygon()
        {
            myPolygon.Stroke = System.Windows.Media.Brushes.WhiteSmoke;
            myPolygon.StrokeThickness = 2;
        }

        private void PlotPoints()
        {
            for (int i = 0; i < PointCount; i++)
            {
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius / 2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius / 2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }


        private void PlotWay(List<int> BestWayIndex)
        {
            PointCollection Points = new PointCollection();

            for (int i = 0; i < BestWayIndex.Count; i++)
                Points.Add(pC[BestWayIndex[i]]);

            myPolygon.Points = Points;
            MyCanvas.Children.Add(myPolygon);
        }

        private void VelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;

            dT.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(item.Content));
        }

        private void StopStart_Click(object sender, RoutedEventArgs e)
        {
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
            }
            else
            {
                NumElemCB.IsEnabled = false;
                dT.Start();
            }
        }

        private void NumElemCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;

            PointCount = Convert.ToInt32(item.Content);
            CreateRandomPopulation(Population);
            

            InitPoints();
            InitPolygon();

            CalculateRoads(Population);
        }

        private void OneStep(object sender, EventArgs e)
        {
            MyCanvas.Children.Clear();
            //InitPoints();
            PlotPoints();
            PlotWay(GetBestWay());
        }
        
        private List<int> GetBestWay()
        {
            Random rnd = new Random();
            List<int> way = new List<int>();

            for (int k = 0; k < PointCount; k++) 
            {
                int x = rnd.Next(0, sizePopulation);
                int y = rnd.Next(0, sizePopulation);
                while (x == y) y = rnd.Next(0, sizePopulation);

                List<int> firstChild = new List<int>();
                List<int> secondChild = new List<int>();

                //скрещивание
                int gap = rnd.Next(2, PointCount - 1);
                for (int i = 0; i < gap; i++)
                {
                    firstChild.Add(Population[x][i]);
                    secondChild.Add(Population[y][i]);
                }
                for (int i = gap; i < PointCount; i++)
                {
                    int k1 = Population[y][i];
                    if (!firstChild.Contains(k1)) firstChild.Add(k1);

                    int k2 = Population[x][i];
                    if (!secondChild.Contains(k2)) secondChild.Add(k2);
                }
                for (int i = gap; i < PointCount; i++)
                {
                    int k1 = Population[x][i];
                    if (!firstChild.Contains(k1)) firstChild.Add(k1);

                    int k2 = Population[y][i];
                    if (!secondChild.Contains(k2)) secondChild.Add(k2);
                }

                //мутации
                Mutation(firstChild);
                Mutation(secondChild);

                double firstChildRoad = CalculateRoad(firstChild);
                double secondChildRoad = CalculateRoad(secondChild);

                Population.Add(firstChild);
                Roads.Add(firstChildRoad);
                Population.Add(secondChild);
                Roads.Add(secondChildRoad);

            }

            var d = Roads.Count / 2;
            while (d >= 1)
            {
                for (var i = d; i < Roads.Count; i++)
                {
                    var j = i;
                    while ((j >= d) && (Roads[j - d] > Roads[j]))
                    {
                        double t = Roads[j];
                        Roads[j] = Roads[j - d];
                        Roads[j - d] = t;

                        List<int> f = Population[j];
                        Population[j] = Population[j - d];
                        Population[j - d] = f;

                        j = j - d;
                    }
                }

                d = d / 2;
            }

            for (int k = 0; k < 2 * PointCount; k++) 
            {
                Roads.RemoveAt(Roads.Count - 1);
                Population.RemoveAt(Population.Count - 1);
            }

            way = Population[0];
            return way;
        }

        static void Mutation(List<int> arr) 
        {
            Random rnd = new Random();
            int kef = rnd.Next(0, 100);
            if (percentMut * 100 >= kef) 
            {
                int k1 = rnd.Next(0, PointCount);
                int k2 = rnd.Next(0, PointCount);
                while (k1 == k2) k2 = rnd.Next(0, PointCount);

                int t = arr[k1];
                arr[k1] = arr[k2];
                arr[k2] = t;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            dT.Stop();

            MainWindow mwd = new MainWindow();
            mwd.Show();
            Close();
        }
        private void CreateRandomPopulation(List<List<int>> arr) 
        {
            arr.Clear();
            Random rnd = new Random();
            for (int i = 0; i < sizePopulation; i++) 
            {
                arr.Add(new List<int>());
                arr[i] = new List<int>();
                for (int j = 0; j < PointCount; j++) 
                {
                    int x = rnd.Next(0, PointCount);
                    while (CheckForUniq(arr[i], x) == false) x = rnd.Next(0, PointCount);
                    arr[i].Add(x);
                }
            }
        }
        private double CalculateRoad(List<int> arr)
        {
            double sum = 0;
            for (int i = 1; i < arr.Count; i++) sum += Sqrt(Pow(pC[arr[i]].X - pC[arr[i - 1]].X, 2) + Pow(pC[arr[i]].Y - pC[arr[i - 1]].Y, 2));

            sum += Sqrt(Pow(pC[arr[0]].X - pC[arr[arr.Count - 1]].X, 2) + Pow(pC[arr[0]].Y - pC[arr[arr.Count - 1]].Y, 2));
            return sum;
        }
        private void CalculateRoads(List<List<int>> arr)
        {
            Roads.Clear();
            for (int i = 0; i < arr.Count; i++) 
            {
                double sum = 0;
                for (int j = 1; j < arr[i].Count; j++) 
                {
                    sum += Sqrt(Pow(pC[arr[i][j]].X - pC[arr[i][j - 1]].X, 2) + Pow(pC[arr[i][j]].Y - pC[arr[i][j - 1]].Y, 2));
                }

                sum += Sqrt(Pow(pC[arr[i][0]].X - pC[arr[i][arr[i].Count - 1]].X, 2) + Pow(pC[arr[i][0]].Y - pC[arr[i][arr[i].Count - 1]].Y, 2));
                Roads.Add(sum);
            }

        }
        private bool CheckForUniq(List<int> arr, int x) 
        {
            bool check = true;

            for (int i = 0; i < arr.Count; i++) 
            {
                if (x == arr[i]) 
                {
                    check = false;
                    break;
                } 
            }
            return check;
        }
    }
}
