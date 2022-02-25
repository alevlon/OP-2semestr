using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Lab_2
{
    class ThirdWindow
    {

        public Window wn = new Window();
        public Grid myGrid = new Grid();

        public ThirdWindow()
        {
            initControls();
        }
        private void initControls()
        {
            wn.Title = "ThirdWindow";
            wn.ResizeMode = ResizeMode.NoResize;

            wn.Width = 350;
            wn.Height = 380;
            myGrid.Background = new SolidColorBrush(Color.FromRgb(232, 232, 232));

            myGrid.Children.Add(ExitBtn());



            wn.Content = myGrid;
            wn.Show();
        }



        private Button ExitBtn()
        {
            Button btn = new Button();
            btn.Content = "Головна";
            btn.FontFamily = new FontFamily("Calibri");
            btn.FontSize = 16;
            btn.Height = 55;
            btn.Width = 80;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(10, 280, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 108, 97));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += GoToMainWin_Click;
            btn.BorderThickness = new Thickness(2);


            return btn;
        }
        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            wn.Close();
        }
    }
}
