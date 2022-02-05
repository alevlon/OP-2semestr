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

namespace Lab_1
{
    /// <summary>
    /// Логика взаимодействия для FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window
    {
        public FirstWindow()
        {
            InitializeComponent();
        }

        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            string PIB_text = PIB.Text;
            string Number_text = Number.Text;
            string StudentInfo_text = StudentInfo.Text;

            string writePath = @"D:\KPI\Programming\2 семестр\Lab-1\data.txt";
            string text = PIB_text + " " + Number_text + " " + StudentInfo_text;

            if (PIB_text.Length > 0 &&
                Number_text.Length > 0 &&
                StudentInfo_text.Length > 0) 
            {
                try
                {
                    StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default);
                    PIB.Text = "";
                    Number.Text = "";
                    StudentInfo.Text = "";
                    sw.WriteLine(text);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message);
                }
            }
        }

        private void Delete_Data_Click(object sender, RoutedEventArgs e)
        {
            string path = @"D:\KPI\Programming\2 семестр\Lab-1\data.txt";
            string writePath = @"D:\KPI\Programming\2 семестр\Lab-1\NewData.txt";
            try
            {
                StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0) break;

                    string[] str = line.Split(' ');
                    if (str[3] != Number_Delete.Text) 
                    {
                        sw.WriteLine(line);
                    }
                }
                
                sr.Close();
                sw.Close();
                StreamReader sr_1 = new StreamReader(writePath, System.Text.Encoding.Default);
                StreamWriter sw_1 = new StreamWriter(path, false, System.Text.Encoding.Default);
                sw_1.Write(sr_1.ReadToEnd());
                sr_1.Close();
                sw_1.Close();

                Number_Delete.Text = "";
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.Message);
            }
        }
    }
}
