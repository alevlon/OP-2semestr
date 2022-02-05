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
    /// Логика взаимодействия для SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {

        private ComboBox[,] Boxes = new ComboBox[5, 5];
        public SecondWindow()
        {
            InitializeComponent();

            int x = 0, y = 0;
            foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>()) 
            {
                Boxes[y,x] = b;
                x++;
                if (x >= 5) 
                {
                    x = 0;
                    y++;
                }
            }
        }

        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
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
