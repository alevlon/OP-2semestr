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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //Main Window 
        private void btn_Exit_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void FirstWindowButtonEnter(object sender, MouseEventArgs e)
        {
            FirstWindowText.Visibility = Visibility.Visible;
        }
        private void FirstWindowButtonLeave(object sender, MouseEventArgs e)
        {
            FirstWindowText.Visibility = Visibility.Hidden;
        }
        private void SecondWindowButtonEnter(object sender, MouseEventArgs e)
        {
            SecondWindowText.Visibility = Visibility.Visible;
        }
        private void SecondWindowButtonLeave(object sender, MouseEventArgs e)
        {
            SecondWindowText.Visibility = Visibility.Hidden;
        }
        private void ThirdWindowButtonEnter(object sender, MouseEventArgs e)
        {
            ThirdWindowText.Visibility = Visibility.Visible;
        }
        private void ThirdWindowButtonLeave(object sender, MouseEventArgs e)
        {
            ThirdWindowText.Visibility = Visibility.Hidden;
        }
        private void FourthWindowButtonEnter(object sender, MouseEventArgs e)
        {
            FourthWindowText.Visibility = Visibility.Visible;
        }
        private void FourthWindowButtonLeave(object sender, MouseEventArgs e)
        {
            FourthWindowText.Visibility = Visibility.Hidden;
        }


        //First Window
        private void ToWin1_Click(object sender, RoutedEventArgs e)
        {
            FirstWindow mw;
            mw = new FirstWindow();
            Hide();
            mw.Show();
        }

        private void ToWin4_Click(object sender, RoutedEventArgs e)
        {
            FourthWindow mw;
            mw = new FourthWindow();
            Hide();
            mw.Show();
        }

        private void ToWin3_Click(object sender, RoutedEventArgs e)
        {
            ThirdWindow mw;
            mw = new ThirdWindow();
            Hide();
            mw.Show();
        }

        private void ToWin2_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow mw;
            mw = new SecondWindow();
            Hide();
            mw.Show();
        }
    }
}
