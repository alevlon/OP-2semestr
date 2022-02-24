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
    class FirstWindow
    {
        public FirstWindow() 
        {
            initControls();
        }

        Window wn = new Window();

        TextBox PIB = new TextBox();
        TextBox Number = new TextBox();
        TextBox StudentInfo = new TextBox();
        TextBox Number_Delete = new TextBox();
        private void initControls() 
        {
            
            wn.Title = "FirstWindow";
            wn.ResizeMode = ResizeMode.NoResize;

            wn.Width = 800;
            wn.Height = 450;

            Grid myGrid = new Grid();
            myGrid.Background = new SolidColorBrush(Color.FromRgb(170, 255, 217));
            myGrid.Width = 792;
            myGrid.Height = 419;

            //textbox create
            PIB = tbCreate(PIB);
            PIB.Margin = new Thickness(32, 96, 0, 0);
            Number = tbCreate(Number);
            Number.Margin = new Thickness(32, 169, 0, 0);
            StudentInfo = tbCreate(StudentInfo);
            StudentInfo.Margin = new Thickness(32, 246, 0, 0);
            Number_Delete = tbCreate(Number_Delete);
            Number_Delete.Margin = new Thickness(435, 96, 0, 0);

            //label create
            Label lb_PIB = new Label();
            lb_PIB = lbCreate(lb_PIB);
            lb_PIB.Content = "ПІБ";
            lb_PIB.Margin = new Thickness(45, 56, 0, 0);
            Label lb_Number = new Label();
            lb_Number = lbCreate(lb_Number);
            lb_Number.Content = "Номер залікової";
            lb_Number.Margin = new Thickness(45, 128, 0, 0);
            Label lb_StudentInfo = new Label();
            lb_StudentInfo = lbCreate(lb_StudentInfo);
            lb_StudentInfo.Content = "Особисті дані";
            lb_StudentInfo.Margin = new Thickness(45, 204, 0, 0);
            Label lb_Number_Delete = new Label();
            lb_Number_Delete = lbCreate(lb_Number_Delete);
            lb_Number_Delete.Content = "Видалити за номером залікової";
            lb_Number_Delete.Margin = new Thickness(423, 54, 0, 0);



            myGrid.Children.Add(PIB);
            myGrid.Children.Add(Number);
            myGrid.Children.Add(StudentInfo);
            myGrid.Children.Add(Number_Delete);
            myGrid.Children.Add(lb_PIB);
            myGrid.Children.Add(lb_Number);
            myGrid.Children.Add(lb_StudentInfo);
            myGrid.Children.Add(lb_Number_Delete);

            myGrid.Children.Add(ExitBtn());
            myGrid.Children.Add(SaveDataBtn());
            myGrid.Children.Add(Delete_DataBtn());

            wn.Content = myGrid;
            wn.Show();
        }



        private Label lbCreate(Label lb) 
        {
            lb.HorizontalAlignment = HorizontalAlignment.Left;
            lb.VerticalAlignment = VerticalAlignment.Top;
            lb.FontFamily = new FontFamily("Segoe Print");
            lb.FontSize = 20;
            lb.Width = 360;
            lb.Height = 42; 

            return lb;
        }
        private TextBox tbCreate(TextBox tb) 
        {
            tb.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            tb.Height = 32;
            tb.Width = 313;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontFamily = new FontFamily("Segoe Print");
            tb.FontSize = 18;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.Opacity = 0.8;
            return tb;
        }
        private Button ExitBtn() 
        {
            Button btn = new Button();
            btn.Content = "Головна";
            btn.FontFamily = new FontFamily("Segoe Print");
            btn.FontSize = 16;
            btn.Height = 48;
            btn.Width = 117;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(655, 351, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 108, 97));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += GoToMainWin_Click;

            return btn;
        }
        private Button SaveDataBtn() 
        {
            Button btn = new Button();
            btn.Content = "Зчитати";
            btn.FontFamily = new FontFamily("Segoe Print");
            btn.FontSize = 16;
            btn.Height = 48;
            btn.Width = 137;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(110, 317, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(112, 255, 97));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;


            btn.Click += SaveData_Click;
            return btn;
        }
        private Button Delete_DataBtn()
        {
            Button btn = new Button();
            btn.Content = "Видалити";
            btn.FontFamily = new FontFamily("Segoe Print");
            btn.FontSize = 16;
            btn.Height = 48;
            btn.Width = 137;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(524, 153, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 182, 97));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;


            btn.Click += Delete_Data_Click;
            return btn;
        }
        private void SaveData_Click(object sender, RoutedEventArgs e)
        {

            string PIB_text = PIB.Text;
            string Number_text = Number.Text;
            string StudentInfo_text = StudentInfo.Text;

            string writePath = @"..\..\data.txt";
            string text = PIB_text + " " + Number_text + " " + StudentInfo_text;

            if (PIB_text.Length > 0 &&
                Number_text.Length > 0 &&
                StudentInfo_text.Length > 0)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default);
                    (PIB as TextBox).Text = "";
                    (Number as TextBox).Text = "";
                    (StudentInfo as TextBox).Text = "";
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
            string path = @"..\..\data.txt";
            string writePath = @"..\..\NewData.txt";
            try
            {
                StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0) break;
                    if (!line.Contains(Number_Delete.Text.ToString())) sw.WriteLine(line);

                    //string[] str = line.Split(' ');
                    //if (str[3] != Number_Delete.Text) 
                    //{
                    //    sw.WriteLine(line);
                    //}
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
        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            wn.Close();
        }
    }
}
