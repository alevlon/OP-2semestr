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
        public TextBox MainText = new TextBox();
        public TextBox SecondText = new TextBox();

        public string znak = "";
        public double first_number;

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

            MainText = MainTextCreate(MainText);
            SecondText = SecondTextCreate(SecondText);



            myGrid.Children.Add(ExitBtn());
            myGrid.Children.Add(MainText);
            myGrid.Children.Add(SecondText);
            myGrid.Children.Add(BtnZeroCreate());
            myGrid.Children.Add(EqualCreate());
            int x = 1;
            int co_x = 10;
            int co_y = 225;
            for (int i = 1; i <= 3; i++) 
            {
                for (int j = 1; j <= 4; j++) 
                {
                    Button current = ButtonCreate();
                    current.Margin = new Thickness(co_x, co_y, 0, 0);

                    if (j % 4 != 0)
                    {
                        current.Content = x;
                        x++;
                    }
                    else 
                    {
                        if (i == 1) current.Content = "+";
                        else if (i == 2) current.Content = "-";
                        else if (i == 3) current.Content = "*";
                    }

                    myGrid.Children.Add(current);
                    co_x += 80;
                }
                co_x = 10;
                co_y -= 55;
            }

            co_x += 80;
            for (int i = 1; i <= 3; i++) 
            {
                Button current = ButtonCreate();
                current.Margin = new Thickness(co_x, co_y, 0, 0);
                if (i == 1) current.Content = "C";
                else if (i == 2) current.Content = "⌫";
                else if (i == 3) current.Content = "/";

                co_x += 80;
                myGrid.Children.Add(current);
            }

            wn.Content = myGrid;
            wn.Show();
        }

        private Button BtnZeroCreate()
        {
            Button btn = new Button();
            btn.Content = "0";
            btn.FontFamily = new FontFamily("Calibri");
            btn.FontSize = 16;
            btn.Height = 55;
            btn.Width = 80;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(90, 280, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += button_click;
            btn.BorderThickness = new Thickness(2);


            return btn;
        }
        private Button EqualCreate() 
        {
            Button btn = new Button();
            btn.Content = "=";
            btn.FontFamily = new FontFamily("Calibri");
            btn.FontSize = 16;
            btn.Height = 55;
            btn.Width = 160;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(170, 280, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(234, 190, 56));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += button_click;
            btn.BorderThickness = new Thickness(2);

            return btn;
        }
        private Button ButtonCreate()
        {
            Button btn = new Button();
            btn.FontFamily = new FontFamily("Calibri");
            btn.FontSize = 16;
            btn.Height = 55;
            btn.Width = 80;
            btn.IsEnabled = true;
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += button_click;
            btn.BorderThickness = new Thickness(2);


            return btn;
        }
        private void button_click(object sender, RoutedEventArgs e)
        {

            string str = (((Button)e.OriginalSource).Content).ToString();
            if (str == "C")
            {
                MainText.Text = "";
                SecondText.Text = "";
                znak = "";
            }
            else if (str == "⌫")
            {
                string a = MainText.Text.ToString();
                if (a.Length > 0)
                {
                    a = a.Remove(a.Length - 1);
                    MainText.Text = a;
                }
            }
            else if (str == "+" ||
                str == "-" ||
                str == "/" ||
                str == "*")
            {
                if (MainText.Text.ToString().Length != 0 && znak.Length == 0)
                {
                    znak = str;
                    first_number = Convert.ToDouble(MainText.Text.ToString());
                    SecondText.Text = first_number.ToString() + znak;
                    MainText.Text = "";
                }
                else if (SecondText.Text.ToString().Length != 0)
                {
                    znak = str;
                    string a = SecondText.Text.ToString();
                    a = a.Remove(a.Length - 1);
                    a += str;
                    SecondText.Text = a;
                }
            }
            else if (str == "=")
            {
                if (znak.Length != 0 && MainText.Text.ToString().Length != 0)
                {
                    switch (znak)
                    {
                        case "+":
                            MainText.Text = (first_number + Convert.ToDouble(MainText.Text.ToString())).ToString();
                            break;
                        case "-":
                            MainText.Text = (first_number - Convert.ToDouble(MainText.Text.ToString())).ToString();
                            break;
                        case "*":
                            MainText.Text = (first_number * Convert.ToDouble(MainText.Text.ToString())).ToString();
                            break;
                        case "/":
                            MainText.Text = (first_number / Convert.ToDouble(MainText.Text.ToString())).ToString();
                            break;
                    }
                    znak = "";
                    SecondText.Text = "";
                }
            }

            else MainText.Text += str;
        }
        private TextBox MainTextCreate(TextBox tb)
        {
            tb.Text = "";   
            tb.Height = 37;
            tb.Width = 320;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontFamily = new FontFamily("Calibri");
            tb.FontSize = 18;
            tb.HorizontalContentAlignment = HorizontalAlignment.Right;
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            tb.Background = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            tb.Margin = new Thickness(10, 23, 0, 0);
            tb.IsEnabled = false;
            return tb;
        }
        private TextBox SecondTextCreate(TextBox tb)
        {
            tb.Background = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            tb.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102));
            tb.Text = "";
            tb.Height = 23;
            tb.Width = 320;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontFamily = new FontFamily("Calibri");
            tb.FontSize = 13;
            tb.HorizontalContentAlignment = HorizontalAlignment.Right;
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            tb.Margin = new Thickness(15, 0, 0, 0);
            tb.IsEnabled = false;

            return tb;
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
