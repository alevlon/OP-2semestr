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
    class FourthWindow
    {
        public Window wn = new Window();
        public Grid myGrid = new Grid();
        public FourthWindow()
        {
            initControls();
        }
        private void initControls()
        {
            wn.Title = "FourthWindow";
            wn.ResizeMode = ResizeMode.NoResize;
            wn.Width = 800;
            wn.Height = 450;
            myGrid.Background = new SolidColorBrush(Color.FromRgb(228, 228, 228));

            Label lb = new Label();
            lb.HorizontalAlignment = HorizontalAlignment.Left;
            lb.VerticalAlignment = VerticalAlignment.Top;
            lb.FontFamily = new FontFamily("Segoe Print");
            lb.FontSize = 18;
            lb.Width = 400;
            lb.Height = 400;
            lb.Content = "Роботу винонав:\nСтудент 1 курсу, ФПМ\nГрупи КП-12\nОнасенко Олексій Володимирович";
            lb.Margin = new Thickness(219, 111, 0, 0);

            myGrid.Children.Add(ExitBtn());
            myGrid.Children.Add(lb);
            wn.Content = myGrid;
            wn.Show();
        }

        private Button ExitBtn()
        {
            Button btn = new Button();
            btn.Content = "Головна";
            btn.FontFamily = new FontFamily("Segoe Print");
            btn.FontSize = 16;
            btn.Height = 41;
            btn.Width = 107;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(656, 342, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 108, 97));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += GoToMainWin_Click;

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
