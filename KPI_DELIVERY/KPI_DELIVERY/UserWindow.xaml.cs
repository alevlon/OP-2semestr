using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KPI_DELIVERY
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        string connectionString = null;
        SqlConnection connection = null;
        SqlCommand command;

        public User user;
        public Publication publication;
        public UserWindow(string login, string password, string surname, string firstname, string middlename)
        {
            InitializeComponent();
            user = new User(login, password, surname, firstname, middlename);

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string sqlR = "SELECT * FROM dbo.Publications";
            CatalogFillingContent(sqlR);
            SettingsnGetStreets();
        }


        //Головне вікно
        private void Top_Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Переміщення вікна
        {
            DragMove();
        }
        private void Exit_btn_Click(object sender, RoutedEventArgs e) //Кнопка Вихід
        {
            Close();
        }
        private void Hide_btn_Click(object sender, RoutedEventArgs e) //Кнопка Згорнути
        {
            this.WindowState = WindowState.Minimized;
        }     
        private void LogOutBtn_Click(object sender, RoutedEventArgs e) //Кнопка вийти з кабінету користувача
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        private void SettingsMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Відкрити налаштування
        {
            HideAllWindow();
            Settings.Margin = new Thickness(280, 72, 20, 42);
        }
        private void SettingsCatalog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Відкрити каталог
        {
            HideAllWindow();
            Catalog.Margin = new Thickness(280, 72, 20, 42);
        }


        //Налаштування
        private void SettingsnGetStreets() //Отримання списку з вулицями
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string strQ = "SELECT * FROM dbo.Streets;";
                command = new SqlCommand(strQ, connection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string StreetId = reader.GetValue(0).ToString();
                    string NameStreet = reader.GetValue(1).ToString();

                    ComboBoxItem item = new ComboBoxItem();
                    item.Uid = StreetId;
                    item.Content = NameStreet;

                    SettingsStreets.Items.Add(item);
                }
                reader.Close();
            }
            connection.Close();
        }
        private void SettingsSubmit_Click(object sender, RoutedEventArgs e) //Підтвердження змін акаунту
        {
            SettingsMistake.Content = "";
            string temp_for_house = SettingsHouse.Text;

            foreach (TextBox t in MainSettings.Children.OfType<TextBox>())
            {
                if (t.Text == "")
                {
                    SettingsMistake.Content += "* Заповніть усі поля\n";
                    break;
                }
            }
            foreach (char c in temp_for_house)
            {
                if (!((int)c >= 48 && (int)c <= 57))
                {
                    SettingsMistake.Content += "* Поле 'Будинок' має \nмістити лише числа\n";
                    break;
                }
            }
            if (SettingsStreets.SelectedIndex == -1) SettingsMistake.Content += "* Введіть справжню адресу\n";
            if (SettingsNewPassword.Text != SettingsRepeatNewPassword.Text) SettingsMistake.Content += "* Паролі не співпадають\n";


            if (SettingsMistake.Content.ToString().Length == 0)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    string Password = SettingsNewPassword.Text;
                    string Surname = SettingsSurname.Text;
                    string Firstname = SettingsFirstname.Text;
                    string Middlename = SettingsMiddlename.Text;
                    string StreetID;
                    ComboBoxItem item = SettingsStreets.SelectedItem as ComboBoxItem;
                    StreetID = item.Uid;
                    string House = SettingsHouse.Text;

                    try
                    {
                        string strQ = $"UPDATE Users SET Password = '{Password}', Surname = '{Surname}', Firstname = '{Firstname}', Middlename = '{Middlename}' ";
                        strQ += $"WHERE Login = '{user.login}';";
                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        strQ = $"UPDATE Location SET StreetId = '{StreetID}', HouseNumber = '{House}' ";
                        strQ += $"WHERE UserLogin = '{user.login}';";
                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        user.password = Password;
                        user.surname = Surname;
                        user.firstname = Firstname;
                        user.middlename = Middlename;

                        SettingsMistake.Content = "";
                        SettingsNewPassword.Text = "";
                        SettingsSurname.Text = "";
                        SettingsFirstname.Text = "";
                        SettingsMiddlename.Text = "";
                        SettingsNewPassword.Text = "";
                        SettingsStreets.SelectedIndex = -1;
                        SettingsRepeatNewPassword.Text = "";
                        SettingsHouse.Text = "";

                        MessageBox.Show("Дані акаунту успішно змінені");
                    }
                    catch (Exception)
                    {
                        SettingsMistake.Content += "Щось пішло не так!";
                    }
                }
            }
        }


        //Каталог
        private void CatalogSort_Click(object sender, MouseButtonEventArgs e) //Сортування каталогу
        {
            Label label = sender as Label;
            string strQ = "";

            string searchExpression = CatalogSearch.Text;

            if (label.Name == "CatalogSortByName")
            {
                if (searchExpression.Length == 0) strQ = "SELECT * FROM dbo.Publications ORDER BY Title";
                else strQ = $"SELECT * FROM dbo.Publications WHERE Title LIKE '%{searchExpression}%' ORDER BY Title;";
            }
            else if (label.Name == "CatalogSortByPrice")
            {
                if (searchExpression.Length == 0) strQ = "SELECT * FROM dbo.Publications ORDER BY Price";
                else strQ = $"SELECT * FROM dbo.Publications WHERE Title LIKE '%{searchExpression}%' ORDER BY Price;";
            }
            else if (label.Name == "CatalogSortByType") 
            {
                if (searchExpression.Length == 0) strQ = "SELECT * FROM dbo.Publications ORDER BY Type";
                else strQ = $"SELECT * FROM dbo.Publications WHERE Title LIKE '%{searchExpression}%' ORDER BY Type;";
            }

            CatalogFillingContent(strQ);
        }
        private void CatalogSearch_TextChanged(object sender, TextChangedEventArgs e) //Пошук за каталогом
        {
            string searchExpression = (sender as TextBox).Text;

            string strQ = $"SELECT * FROM dbo.Publications WHERE Title LIKE '%{searchExpression}%';";
            CatalogFillingContent(strQ);
        }
        private void CatalogFillingContent(string strQ) //Заповнити каталог
        {
            CatalogPanel.Children.Clear();

            connection = new SqlConnection(connectionString);
            command = new SqlCommand(strQ, connection);
            connection.Open();

            if (connection.State == System.Data.ConnectionState.Open) 
            {
                SqlDataReader reader = command.ExecuteReader();

                bool background = true;
                while (reader.Read()) 
                {
                    Grid myGrid = new Grid();
                    myGrid.Height = 35;
                    myGrid.Uid = reader.GetValue(0).ToString();

                    if (background)
                    {
                        background = false;
                        myGrid.Background = new SolidColorBrush(Color.FromRgb(247, 246, 254));
                    }
                    else background = true;


                    ColumnDefinition colDef1 = new ColumnDefinition();
                    colDef1.Width = new GridLength(260);
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    ColumnDefinition colDef3 = new ColumnDefinition();
                    ColumnDefinition colDef4 = new ColumnDefinition();

                    myGrid.ColumnDefinitions.Add(colDef1);
                    myGrid.ColumnDefinitions.Add(colDef2);
                    myGrid.ColumnDefinitions.Add(colDef3);
                    myGrid.ColumnDefinitions.Add(colDef4);


                    for (int i = 1; i <= 3; i++)
                    {
                        Label label = new Label();
                        label.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        label.FontSize = 15;
                        label.HorizontalAlignment = HorizontalAlignment.Center;
                        label.VerticalAlignment = VerticalAlignment.Center;

                        string a = "";
                        if (i == 1)
                        {
                            a = '\u0022' + reader.GetValue(i).ToString() + '\u0022';
                        }
                        else if (i == 2)
                        {
                            a = "$" + reader.GetValue(i).ToString();
                        }
                        else if (i == 3) 
                        {
                            a = reader.GetValue(i).ToString();
                            if (a == "True") a = "Газета";
                            else a = "Журнал";
                        }

                        label.Content = a;

                        Grid.SetColumn(label, i - 1);
                        myGrid.Children.Add(label);
                    }

                    Button btn = new Button();
                    Grid.SetColumn(btn, 3);
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.Style = (Style)Resources["Order_Button"];
                    btn.FontSize = 15;
                    btn.Content = "Замовити";
                    btn.FontFamily = new FontFamily("Arial");
                    btn.Width = 85;
                    btn.Height = 28;
                    btn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    btn.BorderBrush = new SolidColorBrush(Color.FromRgb(18, 200, 71));
                    btn.Foreground = new SolidColorBrush(Color.FromRgb(18, 200, 71));
                    btn.Click += CatalogOrderBtn_Click;

                    myGrid.Children.Add(btn);
                    CatalogPanel.Children.Add(myGrid);
                }
                reader.Close();
            }

            connection.Close();
        }
        private void CatalogOrderBtn_Click(object sender, RoutedEventArgs e) //Замовити видання з каталогу
        {
            Button btn = sender as Button;
            Grid g = btn.Parent as Grid;

            connection = new SqlConnection(connectionString);
            connection.Open();

            if (connection.State == System.Data.ConnectionState.Open) 
            {
                string strQ = $"SELECT * FROM dbo.Publications WHERE PublicationID = '{g.Uid}';";
                SqlDataAdapter adapter = new SqlDataAdapter(strQ, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count == 1) 
                {
                    int ID = Convert.ToInt32(dt.Rows[0][0]);
                    string Title = dt.Rows[0][1].ToString();
                    double Price = Convert.ToDouble(dt.Rows[0][2]);
                    bool Type = Convert.ToBoolean(dt.Rows[0][3]);

                    publication = new Publication(ID, Title, Price, Type);

                    CheckoutTitle.Content = '\u0022' + publication.title + '\u0022';
                    CheckoutPrice.Content = "$" + publication.price;
                    CheckoutSurname.Content = user.surname;
                    CheckoutFirstname.Content = user.firstname;
                    CheckoutMiddlename.Content = user.middlename;

                    HideAllWindow();
                    Checkout.Margin = new Thickness(280, 72, 20, 42);
                }
            }
            connection.Close();
        }

        //Оформлення підписки
        private void CheckoutBackBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAllWindow();
            Catalog.Margin = new Thickness(280, 72, 20, 42);
        }

        public struct User //дані користувача
        {
            public string login;
            public string password;
            public string surname;
            public string firstname;
            public string middlename;
            public User(string login, string password, string surname, string firstname, string middlename)
            {
                this.login = login;
                this.password = password;
                this.surname = surname;
                this.firstname = firstname;
                this.middlename = middlename;
            }
        }
        public struct Publication //дані видання
        {
            public int ID;
            public string title;
            public double price;
            public bool type;

            public Publication(int ID, string Title, double Price, bool Type) 
            {
                this.ID = ID;
                this.title = Title;
                this.price = Price; 
                this.type = Type;
            }
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
