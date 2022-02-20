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

namespace Prac_1
{
    /// <summary>
    /// Логика взаимодействия для StudyWindow.xaml
    /// </summary>
    public partial class StudyWindow : Window
    {
        public StudyWindow()
        {
            InitializeComponent();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        public string writePath = @"..\..\data.txt";
        public int MainCount;
        public string phrase = "длагнитор";
        private void CountAttempts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountAttempts.IsEnabled = false;
            InputField.IsEnabled = true;
            try
            {
                StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);

                ComboBox comboBox = (ComboBox)sender;
                ListBoxItem selectedItem = (ListBoxItem)comboBox.SelectedItem;
                MainCount = Convert.ToInt32(selectedItem.Content.ToString());
                sw.WriteLine("Кількість спроб - " + MainCount.ToString());

                sw.Close();
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.Message);
            }
        }


        int Count = 0;
        List<List<double>> arr = new List<List<double>>();
        List<double> row = new List<double>();

        DateTime Start;
        DateTime End;
        bool firstClick = true;
        private void InputField_KeyUp(object sender, KeyEventArgs e)
        {
            
            string a = "";
            bool check = true;
            try
            {
                a = InputField.Text;
                if (a[a.Length - 1] != phrase[a.Length - 1])
                {
                    check = false;
                    a = a.Remove(a.Length - 1, 1);
                    InputField.Text = a;
                    InputField.Select(InputField.Text.Length, 0);
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
                            for (int j = 0; j < arr[i].Count; j++) 
                            {
                                List<double> try_i = new List<double>();
                                try_i = arr[i];

                                try_i.RemoveAt(j);

                                double M = 0;
                                for (int k = 0; k < try_i.Count; k++) M += try_i[k];
                                M = M / try_i.Count;

                                double S = 0;
                                for (int k = 0; k < try_i.Count; k++) S += Math.Pow((try_i[k] - M), 2);
                                S = S / try_i.Count;

                                double S_i = Math.Sqrt(S);

                                double t_p = Math.Abs((arr[i][j] - M) / (S_i / Math.Sqrt(try_i.Count)));

                                double[] ratioStudent = { 12.706, 4.3027, 3.1825, 2.7764, 2.5706, 2.4469, 2.3646, 2.3060, 2.2622, 2.2281, 2.2010, 2.1788, 2.1604, 2.1448, 2.1315, 2.1199, 2.1098 };
                                if (t_p > ratioStudent[try_i.Count]) 
                                {
                                    int abc = 123;
                                }
                            }
                        }



                        //StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default);
                        //for (int i = 0; i < arr.Count; i++)
                        //{
                        //    for (int j = 0; j < arr[i].Count; j++)
                        //    {
                        //        sw.Write(arr[i][j].ToString() + " ");
                        //    }
                        //    sw.WriteLine();
                        //}
                        //sw.Close();
                    }
                }

                CountSymbols.Content = InputField.Text.Length;
            }
        }
    }
}
