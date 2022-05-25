using System;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Collections.Generic;
using System.Globalization;

namespace KPI_DELIVERY
{
    /// <summary>
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        private Operator _operator;
        private SQLHandler _handler;
        public OperatorWindow(Operator @operator)
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            _handler = new SQLHandler(connection);

            this._operator = @operator;
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e) //Кнопка Вихід
        {
            Close();
        }
        private void Hide_btn_Click(object sender, RoutedEventArgs e) //Кнопка Згорнути
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Top_Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Переміщення вікна
        {
            DragMove();
        }
        private void LogOutBtn_Click(object sender, RoutedEventArgs e) //Кнопка вийти з кабінету оператора
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void MenuNewPublication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Відкрити Нова публікація
        {
            HideAllWindow();
            NewPublication.Margin = new Thickness(282, 72, 18, 234);
        }
        private void NewPublicationSubmit_Click(object sender, RoutedEventArgs e) //Зберегти нову публікацію
        {
            NewPublicationMistake.Content = "";
            string temp_for_price = NewPublicationPrice.Text;
            float Price = -1;

            foreach (TextBox t in MainNewPublication.Children.OfType<TextBox>())
            {
                if (t.Text == "")
                {
                    NewPublicationMistake.Content += "* Заповніть усі поля\n";
                    break;
                }
            }
            try
            {
                Price = float.Parse(temp_for_price);
            }
            catch (Exception)
            {
                NewPublicationMistake.Content += "* Поле 'Ціна' має \nмістити лише числа\n";
            }
            if (NewPublicationType.SelectedIndex == -1) NewPublicationMistake.Content += "* Введіть справжню адресу\n";

            if (NewPublicationMistake.Content.ToString().Length == 0) 
            {
                string Title = NewPublicationTitle.Text;
                bool Type = NewPublicationType.SelectedIndex == 1;
                temp_for_price = Price.ToString(CultureInfo.InvariantCulture);

                try
                {
                    if (_handler.CreateNewPublication(Title, temp_for_price, Type)) 
                    {
                        NewPublicationTitle.Text = "";
                        NewPublicationPrice.Text = "";
                        NewPublicationType.SelectedIndex = -1;

                        MessageBox.Show("Публікацію успішно додано");
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MenuSubscriptionCatalog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Відкрити Усі підписки
        {
            HideAllWindow();
            SubscriptionsCatalog.Margin = new Thickness(282, 72, 18, 0);
        }

        private void HideAllWindow() //Згорнути усі вікна
        {
            foreach (Grid g in this.VisibleWindow.Children.OfType<Grid>())
            {
                if (g.Name != "Menu" &&
                    g.Name != "Top_Menu" &&
                    g.Name != "EmptyField")
                {
                    g.Margin = new Thickness(1100, 365, -800, -251);
                }
            }
        }
    }
}
