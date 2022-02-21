using System;
using System.IO;
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
using static System.Math;

namespace Prac_1
{
    /// <summary>
    /// Логика взаимодействия для CheckWindow.xaml
    /// </summary>
    public partial class CheckWindow : Window
    {
        static string path = @"..\..\data.txt";
        static string phrase = "длагнитор";

        public int Count = 0;
        public int MainCount;
        public double unlucky = 0;
        public double lucky = 0;
        public double sum = 0;
        public List<List<double>> S_M_study = new List<List<double>>();
        public List<double> S_M_study_row = new List<double>();

        public List<List<double>> S_M_check = new List<List<double>>();
        public List<double> S_M_check_row = new List<double>();

        public List<List<double>> arr = new List<List<double>>();
        public List<double> row = new List<double>();


        public DateTime Start;
        public DateTime End;
        public bool firstClick = true;

        public CheckWindow()
        {
            InitializeComponent();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void CountAttempts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountAttempts.IsEnabled = false;
            InputField.IsEnabled = true;

            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("Кількість спроб:"))
                {
                    string[] x = line.Split(' ');
                    //MainCount = Convert.ToInt32(x[2]);
                    //CountAttempts.SelectedIndex = MainCount - 3;
                    //CountAttempts.IsEnabled = false;
                }
                else
                {
                    string[] x = line.Split(' ');
                    S_M_study_row.Add(Convert.ToDouble(x[1]));
                    S_M_study_row.Add(Convert.ToDouble(x[4]));
                    S_M_study.Add(S_M_study_row);
                    S_M_study_row = new List<double>();
                }
            }

            try
            {
                ComboBox comboBox = (ComboBox)sender;
                ListBoxItem selectedItem = (ListBoxItem)comboBox.SelectedItem;
                MainCount = Convert.ToInt32(selectedItem.Content.ToString());
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.Message);
            }
        }

        private void InputField_TextChanged(object sender, TextChangedEventArgs e)
        {
            string a = "";
            bool check = true;
            try
            {
                a = InputField.Text;
                if (a[a.Length - 1] != phrase[a.Length - 1])
                {
                    MessageBox.Show("You make a mistake");
                    //MainWindow mw = new MainWindow();
                    //Hide();
                    //mw.Show();
                    row = new List<double>();
                    firstClick = true;
                    check = false;
                    InputField.Text = "";
                }
            }
            catch (Exception Exc)
            {
                check = false;
            }


            if (check)
            {
                if (firstClick)
                {
                    Start = DateTime.Now;
                    firstClick = false;
                }
                else
                {
                    End = DateTime.Now;
                    long elapsedTicks = End.Ticks - Start.Ticks;
                    TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                    double x = elapsedSpan.TotalSeconds;
                    row.Add(x);

                    Start = End;
                }
                if (a.Length == phrase.Length)
                {
                    InputField.Text = "";
                    Count++;
                    arr.Add(row);
                    row = new List<double>();
                    firstClick = true;

                    if (Count >= MainCount)
                    {
                        InputField.IsEnabled = false;

                        for (int i = 0; i < MainCount; i++)
                        {
                            double M = 0;
                            for (int k = 0; k < arr[i].Count; k++) M += arr[i][k];
                            M = M / arr[i].Count;

                            double S = 0;
                            for (int k = 0; k < arr[i].Count; k++) S += Math.Pow((arr[i][k] - M), 2);
                            S = S / arr[i].Count;   

                            double S_i = Math.Sqrt(S);

                            S_M_check_row.Add(S_i);
                            S_M_check_row.Add(M);
                            S_M_check.Add(S_M_check_row);
                            S_M_check_row = new List<double>();
                        }

                        if (Fisher_Find()) Variance_Of_Samples.Content = "Однорідні";
                        else Variance_Of_Samples.Content = "Неоднорідні";

                        double P = Student_Find();
                        P_Identification.Content = P;

                        double P1 = 0;
                        double P2 = 0;

                        P1 = Math.Round(unlucky / sum, 2);
                        P2 = Math.Round(lucky / sum, 2);

                        if (Check_author.IsChecked == true) Mistake_1.Content = P1;
                        else Mistake_2.Content = P2;
                    }
                }
                CountSymbols.Content = InputField.Text.Length;
            }
        }


        private bool Fisher_Find() 
        {
            double[,] fisher_table = { { 161.45, 199.50, 215.71, 224.58, 230.16, 233.99, 236.77 },
                                         { 18.51, 19, 19.16, 19.25, 19.30, 19.33, 19.35 },
                                         { 10.13, 9.55, 9.28, 9.12, 9.01, 8.94, 8.89 },
                                         { 7.71, 6.94, 6.59, 6.39, 6.26, 6.16, 6.09 },
                                         { 6.61, 5.79, 5.41, 5.19, 5.05, 4.95, 4.88 },
                                         { 5.99, 5.14, 4.76, 4.53, 4.39, 4.28, 4.21 },
                                         { 5.59, 4.74, 4.35, 4.12, 3.97, 3.87, 3.79 }};

            int count = 0;
            for (int i = 0; i < S_M_study.Count; i++) 
            {
                for (int j = 0; j < S_M_check.Count; j++) 
                {
                    double S_max = Math.Max(Math.Pow(S_M_check[j][0], 2), Math.Pow(S_M_study[i][0], 2));
                    double S_min = Math.Min(Math.Pow(S_M_check[j][0], 2), Math.Pow(S_M_study[i][0], 2));

                    double Fisher_P = S_max / S_min;
                    if (Fisher_P > fisher_table[S_M_study.Count - 1, S_M_check.Count - 1]) count++;
                }
            }

            if (count > S_M_check.Count * S_M_study.Count / 2) return false;
            else 
                return true;
        }
        private double Student_Find() 
        {
            double[] Student_table = { 12.7, 4.3, 3.18, 2.78, 2.57, 2.45, 2.36, 2.31, 2.26 };
            double r = 0;
            for (int i = 0; i < S_M_study.Count; i++) 
            {
                double global_t = 0;
                for (int j = 0; j < S_M_check.Count; j++) 
                {
                    double S = Sqrt((Pow(S_M_study[i][0], 2) + Pow(S_M_check[j][0], 2)) * (arr[j].Count - 1) / (2.0 * arr[j].Count - 1));
                    double t_student = Abs(S_M_study[i][1] - S_M_check[j][1]) / (S * Sqrt(2.0 / arr[j].Count));

                    if (t_student > Student_table[arr[0].Count - 1]) unlucky++;
                    else lucky++;
                    sum++;

                    global_t += t_student;
                }
                global_t /= S_M_check.Count;

                if (global_t <= Student_table[arr[0].Count - 1]) r++;
                else unlucky++;
            }   

            double P = r / S_M_study.Count;
            P = Math.Round(P, 2);
            return P;
        }  
    }
}
