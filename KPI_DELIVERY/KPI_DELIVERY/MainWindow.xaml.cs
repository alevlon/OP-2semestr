using System;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Collections.Generic;


namespace KPI_DELIVERY
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private int count_for_enter = 3;
        private SQLHandler handler;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            handler = new SQLHandler(connection);

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
        private void Registration_btn_Click(object sender, RoutedEventArgs e) //Відкриття вікна реєстрації, з меню
        {
            RegistrationGetStreets();
            HideAllWindow();

            Registration.Margin = new Thickness(282, 72, 18, 0);
        }
        private void Login_btn_Click(object sender, RoutedEventArgs e) //Відкриття вікна входу, з меню
        {
            HideAllWindow();

            Login.Margin = new Thickness(282, 72, 18, 0);
        }


        //Регестрація та вхід користувача
        private void LoginEnter_Click(object sender, RoutedEventArgs e) //вхід до акаунту
        {
            string login = LoginLoginTB.Text;
            string password = LoginPassword.Password;
            User UserTryToEnter = new User(login, password);
            UserTryToEnter = handler.AuthorizationUser(UserTryToEnter);

            if (UserTryToEnter.exception.Length == 0)
            {
                UserWindow userWindow = new UserWindow(UserTryToEnter);
                userWindow.Show();
                Close();
            }
            else 
            {
                if (UserTryToEnter.exception == "Введіть справжній логін") LoginMistake.Content = UserTryToEnter.exception;
                else 
                {
                    count_for_enter--;
                    LoginMistake.Content = "Невірна спроба входу, залишилось \n" + count_for_enter.ToString() + " спроби";
                }
            }
                        
            if (count_for_enter <= 0) 
            {
                LoginLoginTB.Text = "";
                LoginPassword.Password = "";
                count_for_enter = 3;
                            
                MessageBox.Show("Невдала спробу входу до акаунту");
                LoginMistake.Content = "";
            }

        }
        private void LoginWindowOpen(object sender, MouseButtonEventArgs e) //Відкриття вікна входу
        {
            HideAllWindow();

            Login.Margin = new Thickness(282, 72, 18, 0);
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
                    if (handler.CreateNewAccount(Login, Password, Surname, Firstname, Middlename, StreetID, House))
                    {
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
                    else throw new Exception();
                }
                catch (Exception) 
                {
                    RegistrationMistake.Content += "Такий логін вже існує!";
                }
            }
        }
        private void RegistrationGetStreets() //Отримання списку з вулицями
        {
            RegistrationStreets.Items.Clear();
            foreach (Street street in handler.GetAllStreets()) 
            {
                RegistrationStreets.Items.Add(street.ToComboBoxItem());
            }
        }
        private void RegistrationWindowOpen(object sender, MouseButtonEventArgs e)//Відкриття вікна реєстрації
        {
            RegistrationGetStreets();
            HideAllWindow();

            Registration.Margin = new Thickness(282, 72, 18, 0);
        }


        private void LoginOperator_Click(object sender, RoutedEventArgs e)
        {
            HideAllWindow();
            Operator.Margin = new Thickness(282, 72, 18, 0);
        }
        private void OperatorEnter_Click(object sender, RoutedEventArgs e)
        {
            if (OperatorID_TB.Text.Length == 0)
            {
                OperatorMistake.Content = "Введіть справжній ідентифікатор";
                return;
            }
            else 
                OperatorMistake.Content = "";

            int ID = Convert.ToInt32(OperatorID_TB.Text);
            string password = OperatorPassword_PB.Password;

            Operator OperatorTryToEnter = new Operator(ID, password);
            OperatorTryToEnter = handler.AuthorizationOperator(OperatorTryToEnter);

            if (OperatorTryToEnter.exception.Length == 0)
            {
                OperatorWindow operatorWindow = new OperatorWindow(OperatorTryToEnter);
                operatorWindow.Show();
                Close();
            }
            else
            {
                if (OperatorTryToEnter.exception == "Введіть справжній ідентифікатор") OperatorMistake.Content = OperatorTryToEnter.exception;
                else
                {
                    count_for_enter--;
                    OperatorMistake.Content = "Невірна спроба входу, залишилось \n" + count_for_enter.ToString() + " спроби";
                }
            }

            if (count_for_enter <= 0)
            {
                OperatorID_TB.Text = "";
                OperatorPassword_PB.Password = "";
                count_for_enter = 3;

                MessageBox.Show("Невдала спробу входу");
                OperatorMistake.Content = "";
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
                    g.Margin = new Thickness(1075, 72, -775, 0);
                }
            }
        }
    }
}
