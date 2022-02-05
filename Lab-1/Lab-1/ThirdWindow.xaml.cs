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

namespace Lab_1
{
    /// <summary>
    /// Логика взаимодействия для ThirdWindow.xaml
    /// </summary>
    public partial class ThirdWindow : Window
    {
        public ThirdWindow()
        {
            InitializeComponent();
        }

        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        string znak = "";
        double first_number;
        private void button_click(object sender, RoutedEventArgs e)
        {


            string str = (string)((Button)e.OriginalSource).Content;
            if (str == "C") 
            {
                MainText.Content = "";
                SecondText.Content = "";
                znak = "";
            } 
            else if (str == "⌫")
            {
                string a = MainText.Content.ToString() ;
                if (a.Length > 0)
                {
                    a = a.Remove(a.Length - 1);
                    MainText.Content = a;
                }
            }
            else if (str == "+" ||
                str == "-" ||
                str == "/" ||
                str == "*")
            {
                if (MainText.Content.ToString().Length != 0 && znak.Length == 0)
                {
                    znak = str;
                    first_number = Convert.ToDouble(MainText.Content.ToString());
                    SecondText.Content = first_number.ToString() + znak;
                    MainText.Content = "";
                }
                else if (SecondText.Content.ToString().Length != 0) 
                {
                    znak = str;
                    string a = SecondText.Content.ToString();
                    a = a.Remove(a.Length - 1);
                    a += str;
                    SecondText.Content = a;
                }
            }
            else if (str == "=")
            {
                if (znak.Length != 0 && MainText.Content.ToString().Length != 0)
                {
                    switch (znak)
                    {
                        case "+":
                            MainText.Content = first_number + Convert.ToDouble(MainText.Content.ToString());
                            break;
                        case "-":
                            MainText.Content = first_number - Convert.ToDouble(MainText.Content.ToString());
                            break;
                        case "*":
                            MainText.Content = first_number * Convert.ToDouble(MainText.Content.ToString());
                            break;
                        case "/":
                            MainText.Content = first_number / Convert.ToDouble(MainText.Content.ToString());
                            break;
                    }
                    znak = "";
                    SecondText.Content = "";
                }
            }

            else MainText.Content += str;
        }
    }
}
