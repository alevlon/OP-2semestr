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
    class SecondWindow
    {

        private ComboBox[,] Boxes = new ComboBox[5, 5];

        public Window wn = new Window();
        public Grid myGrid = new Grid();
        public Label Winner = new Label();

        public SecondWindow()
        {
            initControls();
        }
        private void initControls()
        {
            wn.Title = "SecondWindow";
            wn.ResizeMode = ResizeMode.NoResize;

            wn.Width = 480;
            wn.Height = 620;

            myGrid.Background = new SolidColorBrush(Color.FromRgb(252, 214, 170));
            Winner = lbCreate(Winner);


            myGrid.Children.Add(ExitBtn());
            myGrid.Children.Add(NewGameBtn());
            myGrid.Children.Add(Winner);

            int x = 45;
            int y = 34;
            for (int i = 1; i <= 5; i++) 
            {
                for (int j = 1; j <= 5; j++) 
                {
                    ComboBox current = cbCreate();
                    current.Margin = new Thickness(x, y, 0, 0);
                    myGrid.Children.Add(current);
                    x += 75;
                }
                y += 75;
                x = 45;
            }

            x = 0;
            y = 0;
            foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>())
            {
                Boxes[y, x] = b;
                x++;
                if (x >= 5)
                {
                    x = 0;
                    y++;
                }
            }

            wn.Content = myGrid;
            wn.Show();
        }

        private ComboBox cbCreate() 
        {
            ComboBox cb = new ComboBox();
            cb.HorizontalAlignment = HorizontalAlignment.Left;
            cb.VerticalAlignment = VerticalAlignment.Top;
            cb.Width = 70;
            cb.Height = 70;
            cb.FontFamily = new FontFamily("Tahoma");
            cb.FontSize = 48;
            cb.Foreground = new SolidColorBrush(Color.FromRgb(255, 82, 12));
            cb.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            cb.Opacity = 0.65;
            cb.SelectionChanged += ComboBox_SelectionChanged;

            ListBoxItem x = new ListBoxItem();
            x.Content = "X";
            x.Foreground = new SolidColorBrush(Color.FromRgb(255, 82, 12));
            x.HorizontalAlignment = HorizontalAlignment.Center;
            x.VerticalAlignment = VerticalAlignment.Center;

            ListBoxItem o = new ListBoxItem();
            o.Content = "O";
            o.Foreground = new SolidColorBrush(Color.FromRgb(255, 82, 12));
            o.HorizontalAlignment = HorizontalAlignment.Center;
            o.VerticalAlignment = VerticalAlignment.Center;

            cb.Items.Add(x);
            cb.Items.Add(o);

            return cb;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Boxes[i, j] == sender && Boxes[i, j].SelectedIndex != -1)
                    {
                        int current_x = j, current_y = i;

                        bool check_win;
                        for (int jump = -3; jump <= 0; jump++)
                        {
                            check_win = true;
                            for (int k = 0; k <= 3; k++)
                            {
                                if (current_x + jump + k < 0 || current_x + jump + k > 4)
                                {
                                    check_win = false;
                                    continue;
                                }

                                if (Boxes[current_y, current_x].SelectedIndex != Boxes[current_y, current_x + jump + k].SelectedIndex)
                                {
                                    check_win = false;
                                }
                            }
                            if (check_win)
                            {
                                foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>()) b.IsEnabled = false;

                                if (Boxes[current_y, current_x].SelectedIndex == 0) Winner.Content = "Переможець: хрестики";
                                else Winner.Content = "Переможець: нулики";
                                return;
                            }
                        }

                        for (int jump = -3; jump <= 0; jump++)
                        {
                            check_win = true;
                            for (int k = 0; k <= 3; k++)
                            {
                                if (current_y + jump + k < 0 || current_y + jump + k > 4)
                                {
                                    check_win = false;
                                    continue;
                                }

                                if (Boxes[current_y, current_x].SelectedIndex != Boxes[current_y + jump + k, current_x].SelectedIndex)
                                {
                                    check_win = false;
                                }
                            }
                            if (check_win)
                            {
                                foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>()) b.IsEnabled = false;

                                if (Boxes[current_y, current_x].SelectedIndex == 0) Winner.Content = "Переможець: хрестики";
                                else Winner.Content = "Переможець: нулики";
                                return;
                            }
                        }

                        for (int jump = -3; jump <= 0; jump++)
                        {
                            check_win = true;
                            for (int k = 0; k <= 3; k++)
                            {
                                if (current_y + jump + k < 0 || current_y + jump + k > 4 || current_x + jump + k < 0 || current_x + jump + k > 4)
                                {
                                    check_win = false;
                                    continue;
                                }

                                if (Boxes[current_y, current_x].SelectedIndex != Boxes[current_y + jump + k, current_x + jump + k].SelectedIndex)
                                {
                                    check_win = false;
                                }
                            }
                            if (check_win)
                            {
                                foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>()) b.IsEnabled = false;

                                if (Boxes[current_y, current_x].SelectedIndex == 0) Winner.Content = "Переможець: хрестики";
                                else Winner.Content = "Переможець: нулики";
                                return;
                            }
                        }
                        for (int jump = 3; jump >= 0; jump--)
                        {
                            check_win = true;
                            for (int k = 0; k >= -3; k--)
                            {
                                if (current_x + jump + k < 0 || current_x + jump + k > 4)
                                {
                                    check_win = false;
                                    continue;
                                }

                                if (Boxes[current_y, current_x].SelectedIndex != Boxes[4 - (current_x + jump + k), current_x + jump + k].SelectedIndex)
                                {
                                    check_win = false;
                                }
                            }
                            if (check_win)
                            {
                                foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>()) b.IsEnabled = false;

                                if (Boxes[current_y, current_x].SelectedIndex == 0) Winner.Content = "Переможець: хрестики";
                                else Winner.Content = "Переможець: нулики";
                                return;
                            }
                        }
                    }
                }
            }
        }
        private Label lbCreate(Label lb)
        {
            lb.HorizontalAlignment = HorizontalAlignment.Left;
            lb.VerticalAlignment = VerticalAlignment.Top;
            lb.FontFamily = new FontFamily("Segoe Print");
            lb.FontSize = 22;
            lb.Opacity = 0.8;
            lb.Width = 360;
            lb.Height = 42;
            lb.Content = "Переможець: ";
            lb.Margin = new Thickness(74, 436, 0, 0);

            return lb;
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
            btn.Margin = new Thickness(345, 525, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 108, 97));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += GoToMainWin_Click;

            return btn;
        }
        private Button NewGameBtn()
        {
            Button btn = new Button();
            btn.Content = "Нова гра";
            btn.FontFamily = new FontFamily("Segoe Print");
            btn.FontSize = 16;
            btn.Height = 41;
            btn.Width = 107;
            btn.IsEnabled = true;
            btn.Margin = new Thickness(176, 525, 0, 0);
            btn.Background = new SolidColorBrush(Color.FromRgb(145, 234, 143));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Click += NewGame_Click;

            return btn;
        }
        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            wn.Close();
        }
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>())
            {
                b.SelectedIndex = -1;
                b.IsEnabled = true;
                Winner.Content = "Переможець:";
            }
        }
    }
}
