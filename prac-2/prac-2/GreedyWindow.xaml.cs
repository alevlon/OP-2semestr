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
    /// Логика взаимодействия для GreedyWindow.xaml
    /// </summary>
    public partial class GreedyWindow : Window
    {
        Random rnd = new Random();
        static DispatcherTimer dT;
        static int Radius = 20;
        static int PointCount = 5;
        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List<Ellipse>();
        static PointCollection pC = new PointCollection();
        static List<int> QueuePoint = new List<int>();
        static List<int> ListForCheck = new List<int>();
        static int currentPoint;
        static double minLength = double.MaxValue;
        static int closePoint = int.MaxValue;

        public GreedyWindow()
        {
            dT = new DispatcherTimer();
            currentPoint = rnd.Next(1, PointCount);

            InitializeComponent();
            InitPoints();
            InitPolygon();

            dT = new DispatcherTimer();
            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            EnterThePoints(ListForCheck, currentPoint);
            QueuePoint.Clear();
            QueuePoint.Add(currentPoint);

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

                p.X = rnd.Next(Radius, (int)(0.75 * GreedyAlgWindow.Width) - 3 * Radius);
                p.Y = rnd.Next(Radius, (int)(0.90 * GreedyAlgWindow.Height - 3 * Radius));
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
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius / 2.0);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius / 2.0);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }


        private void PlotWay(int[] BestWayIndex)
        {
            PointCollection Points = new PointCollection();

            for (int i = 0; i < BestWayIndex.Length; i++)
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
            currentPoint = rnd.Next(1, PointCount);

            ListForCheck.Clear();
            EnterThePoints(ListForCheck, currentPoint);
            QueuePoint.Clear();
            QueuePoint.Add(currentPoint);
            minLength = double.MaxValue;
            closePoint = int.MaxValue;

            InitPoints();
            InitPolygon();
        }

        private void OneStep(object sender, EventArgs e)
        {
            if (QueuePoint.Count >= PointCount)
            {
                MessageBox.Show("Найкоротший шлях знайдено");
                dT.Stop();
            }
            else 
            {
                MyCanvas.Children.Clear();
                //InitPoints();
                PlotPoints();
                PlotWay(GetBestWay());
            }
        }

        private int[] GetBestWay()
        {
            int[] way = new int[QueuePoint.Count + 1];
            for (int i = 0; i < QueuePoint.Count; i++) way[i] = QueuePoint[i];

            if (ListForCheck.Count == 0)
            {
                way[way.Length - 1] = closePoint;
                QueuePoint.Add(closePoint);
                minLength = double.MaxValue;
                closePoint = int.MaxValue;
                EnterThePoints(ListForCheck, QueuePoint);
            }
            else 
            {
                way[way.Length - 1] = ListForCheck[0];
                double road = Sqrt(Pow(pC[way[way.Length - 1]].X - pC[way[way.Length - 2]].X, 2) + Pow(pC[way[way.Length - 1]].Y - pC[way[way.Length - 2]].Y, 2));
                if (road < minLength)
                {
                    closePoint = way[way.Length - 1];
                    minLength = road;
                }
                ListForCheck.RemoveAt(0);
            }
            

            return way;
        }





        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            dT.Stop();
            ListForCheck.Clear();
            QueuePoint.Clear();

            MainWindow mwd = new MainWindow();
            mwd.Show();
            Close();
        }
        private void EnterThePoints(List<int> arr, int currentPoint) 
        {
            arr.Clear();
            for (int i = 0; i < PointCount; i++) 
            {
                if (currentPoint == i) continue;

                arr.Add(i);
            }
        }
        private void EnterThePoints(List<int> arr, List<int> mass)
        {
            arr.Clear();
            for (int i = 0; i < PointCount; i++)
            {
                if (mass.Contains(i)) continue;

                arr.Add(i);
            }
        }
    }
}
