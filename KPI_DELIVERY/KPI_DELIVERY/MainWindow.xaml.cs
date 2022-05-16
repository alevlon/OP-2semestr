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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        string connectionString = null;
        SqlConnection connection = null;
        SqlCommand command;
        public int count_for_enter = 3;
        //Data Source=ALEVLON;Initial Catalog=Kpi_Subscribe;Integrated Security=True
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            RegistrationGetStreets();
        }
        

        //Головне вікно та меню
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
        private void Registration_btn_Click(object sender, RoutedEventArgs e) //Відкриття вікна вхід, з меню
        {
            HideAllWindow();

            Registration.Margin = new Thickness(282, 72, 18, 0);
        }
        private void Login_btn_Click(object sender, RoutedEventArgs e) //Відкриття вікна реєстрації, з меню
        {
            HideAllWindow();

            Login.Margin = new Thickness(282, 72, 18, 0);
        }


        //Регестрація та вхід
        private void LoginEnter_Click(object sender, RoutedEventArgs e) //вхід до акаунту
        {
            string login = LoginLoginTB.Text;
            string password = LoginPassword.Password;
            connection = new SqlConnection(connectionString);
            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string strQ = "SELECT * FROM Users WHERE Login='" + login + "';";
                SqlDataAdapter adapter = new SqlDataAdapter(strQ, connection);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0) LoginMistake.Content = "Введіть справжній логін";
                else
                {
                    
                    LoginMistake.Content = "";

                    if (dt.Rows[0][1].ToString() == password)
                    {
                        LoginMistake.Content = "";
                        count_for_enter = 3;

                        string surname = dt.Rows[0][2].ToString();
                        string firstname = dt.Rows[0][3].ToString();
                        string middlename = dt.Rows[0][4].ToString();

                        UserWindow userWindow = new UserWindow(login, password, surname, firstname, middlename);
                        userWindow.Show();
                        Close();
                    }
                    else
                    {
                        count_for_enter--;
                        LoginMistake.Content = "Невірна спроба входу, залишилось \n" + count_for_enter.ToString() + " спроби";
                        
                        if (count_for_enter <= 0) 
                        {
                            LoginLoginTB.Text = "";
                            LoginPassword.Password = "";
                            count_for_enter = 3;
                            
                            MessageBox.Show("Невдала спробу входу до акаунту");
                            LoginMistake.Content = "";
                        }
                    }
                }
            }
            connection.Close();
        }
        private void SubmitRegistration_Click(object sender, RoutedEventArgs e) //Створення нового акаунту
        {
            RegistrationMistake.Content = "";
            string temp_for_house = RegistrationHouse.Text;

            foreach (TextBox t in MainRegistration.Children.OfType<TextBox>())
            {
                if (t.Text == "") 
                {
                    RegistrationMistake.Content += "* Заповніть усі поля\n";
                    break;
                }
            }
            foreach (char c in temp_for_house)
            {
                if (!((int)c >= 48 && (int)c <= 57))
                {
                    RegistrationMistake.Content += "* Поле 'Будинок' має \nмістити лише числа\n";
                    break;
                }
            }
            if (RegistrationStreets.SelectedIndex == -1) RegistrationMistake.Content += "* Введіть справжню адресу\n";
            
            if (RegistrationMistake.Content.ToString().Length == 0) 
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    string Login = RegistrationLoginTB.Text;
                    string Password = RegistrationPassword.Text;
                    string Surname = RegistrationSurname.Text;
                    string Firstname = RegistrationFirstName.Text;
                    string Middlename = RegistrationMiddleName.Text;
                    string StreetID;
                    ComboBoxItem item = RegistrationStreets.SelectedItem as ComboBoxItem;
                    StreetID = item.Uid;
                    string House = RegistrationHouse.Text;

                    try
                    {
                        string strQ = "INSERT INTO dbo.Users";
                        strQ += $" values('{Login}', '{Password}', '{Surname}', '{Firstname}', '{Middlename}');";

                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        strQ = "INSERT INTO dbo.Location";
                        strQ += $" values('{Login}', '{StreetID}', '{House}');";
                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        RegistrationMistake.Content = "";
                        RegistrationLoginTB.Text = "";
                        RegistrationPassword.Text = "";
                        RegistrationSurname.Text = "";
                        RegistrationFirstName.Text = "";
                        RegistrationMiddleName.Text = "";
                        RegistrationStreets.SelectedIndex = -1;
                        RegistrationHouse.Text = "";

                        MessageBox.Show("Акаунт успішно створений");
                    }
                    catch (Exception) 
                    {
                        RegistrationMistake.Content += "Такий логін вже існує!";
                    }
                }
            }
        }
        private void LoginWindowOpen(object sender, MouseButtonEventArgs e) //Відкриття вікна входу
        {
            HideAllWindow();

            Login.Margin = new Thickness(282, 72, 18, 0);
        }
        private void RegistrationWindowOpen(object sender, MouseButtonEventArgs e)//Відкриття вікна реєстрації
        {
            HideAllWindow();

            Registration.Margin = new Thickness(282, 72, 18, 0);
        }
        private void RegistrationGetStreets() //Отримання списку з вулицями
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

                    RegistrationStreets.Items.Add(item);
                }
                reader.Close();
            }
            connection.Close();
        } 


        private void HideAllWindow() //Згорнути усі вікна
        {
            foreach (Grid g in this.VisibleWindow.Children.OfType<Grid>())
            {
                if (g.Name != "Menu" &&
                    g.Name != "Top_Menu" &&
                    g.Name != "EmptyField")
                {
                    g.Margin = new Thickness(1075, 72, -775, 0);
                }
            }
        }
    }
}
