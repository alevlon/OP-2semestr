using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prac_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count_for_enter = 3;
        string connectionString = null;
        SqlConnection connection = null;
        SqlCommand command;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            AdminPanel_CreateUsers();
        }
        

        
        private void RegistrationOpen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HideAllWindow();
            Registration.Margin = new Thickness(0, 0, 0, 0);
        }
        private void LoginOpen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HideAllWindow();
            Login.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Login_Btn_Click(object sender, RoutedEventArgs e)
        {
            string Login = Login_FieldForLogin.Text;
            string password = Login_FieldForPassword.Password;
            connection = new SqlConnection(connectionString);
            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open) 
            {
                string strQ = "SELECT * FROM Users WHERE Login='" + Login + "';";
                SqlDataAdapter adapter = new SqlDataAdapter(strQ, connection);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0) Login_Mistakes.Content = "Введіть справжній логін";
                else 
                {
                    bool Status = Convert.ToBoolean(dt.Rows[0][4]);
                    if (!Status) Login_Mistakes.Content = "Користувач заблокований";

                    else 
                    {
                        if (dt.Rows[0][3].ToString() == password)
                        {
                            if (Login == "Admin")
                            {
                                Login_Mistakes.Content = "";
                                count_for_enter = 3;
                                HideAllWindow();
                                AdminPanel.Margin = new Thickness(0, 0, 0, 0);
                            }
                            else
                            {
                                Login_Mistakes.Content = "";
                                count_for_enter = 3;
                                HideAllWindow();
                                UserData.Margin = new Thickness(0, 0, 0, 0);
                            }
                        }
                        else 
                        {
                            count_for_enter--;
                            if (count_for_enter <= 0) Close();

                            Login_Mistakes.Content = "Невірна спроба входу, залишилось " + count_for_enter.ToString() + " спроби";
                        }
                    }
                }
            }
            connection.Close();
        }
        private void Login_AboutRef_Btn_Click(object sender, RoutedEventArgs e)
        {
            HideAllWindow();
            About.Margin = new Thickness(0, 0, 0, 0);
        }

        private void About_Btn_Click(object sender, RoutedEventArgs e)
        {
            HideAllWindow();
            Login.Margin = new Thickness(0, 0, 0, 0);
        }

        private void UserData_SignUp_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_FieldForLogin.Text;

            if (CheckCorrectPassword(UserData_newPassword, UserData_newPasswordAgain, UserData_Mistakes)) 
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open) 
                {
                    string strQ = "UPDATE Users SET Surname ='" + UserData_Surname.Text + "', ";
                    strQ += "Name = '" + UserData_Name.Text + "', ";
                    strQ += "Password = '" + UserData_newPassword.Text + "' ";
                    strQ += "WHERE Login = '" + login + "';";

                    try
                    {
                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        UserData_Surname.Text = "";
                        UserData_Name.Text = "";
                        UserData_newPassword.Text = "";
                        UserData_newPasswordAgain.Text = "";
                        Login_FieldForPassword.Password = "";

                        HideAllWindow();
                        Login.Margin = new Thickness(0, 0, 0, 0);
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                connection.Close();
                AdminPanel_CreateUsers();
            }
        }
        private void UserData_LogOut_Click(object sender, RoutedEventArgs e)
        {
            UserData_Surname.Text = "";
            UserData_Name.Text = "";
            UserData_newPassword.Text = "";
            UserData_newPasswordAgain.Text = "";
            Login_FieldForPassword.Password = "";

            HideAllWindow();
            Login.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Registration_CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckCorrectPassword(Registration_NewPassword, Registration_NewPasswordAgain, Registration_Mistakes))
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    string strQ = "INSERT INTO Users (Login, Name, Surname, Password, Status)";
                    strQ += " values('" + Registration_LoginForm.Text + "', '" + Registration_Name.Text + "', '" + Registration_Surname.Text + "', '";
                    strQ += Registration_NewPassword.Text + "', 1);";

                    try
                    {
                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        Registration_LoginForm.Text = "";
                        Registration_Name.Text = "";
                        Registration_Surname.Text = "";
                        Registration_NewPassword.Text = "";
                        Registration_NewPasswordAgain.Text = "";

                        HideAllWindow();
                        Login.Margin = new Thickness(0, 0, 0, 0);
                    }
                    catch (Exception)
                    {
                        Registration_Mistakes.Content = "Такий логін вже існує!";
                    }

                    
                }
                connection.Close();
                AdminPanel_CreateUsers();
            }
        }

        private void AdminPanel_ChangePassword_Btn_Click(object sender, RoutedEventArgs e)
        {
            string Login = "Admin";
            string password = AdminPanel_ChangePassword_OldField.Password;

            connection = new SqlConnection(connectionString);
            connection.Open();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                string strQ = "SELECT * FROM Users WHERE Login='" + Login + "';";
                SqlDataAdapter adapter = new SqlDataAdapter(strQ, connection);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows[0][3].ToString() == password)
                {
                    count_for_enter = 3;

                    if (CheckCorrectPassword(AdminPanel_ChangePassword_NewField, AdminPanel_ChangePassword_Mistakes)) 
                    {
                        strQ = "UPDATE Users SET Password ='" + AdminPanel_ChangePassword_NewField.Text + "' ";
                        strQ += "WHERE Login = '" + Login + "';";

                        try
                        {
                            command = new SqlCommand(strQ, connection);
                            command.ExecuteNonQuery();

                            AdminPanel_ChangePassword_OldField.Password = "";
                            AdminPanel_ChangePassword_NewField.Text = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    count_for_enter--;
                    if (count_for_enter <= 0) Close();

                    AdminPanel_ChangePassword_Mistakes.Content = "Невірна спроба входу, залишилось " + count_for_enter.ToString() + " спроби";
                }
            }

            connection.Close();
            AdminPanel_CreateUsers();
        }
        private void AdminPanel_AddUser_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPanel_AddUser_NewLogin.Text.ToString().Length > 0) 
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    string strQ = "INSERT INTO Users (Login, Status)";
                    strQ += " values('" + AdminPanel_AddUser_NewLogin.Text + "', 1);";

                    try
                    {
                        command = new SqlCommand(strQ, connection);
                        command.ExecuteNonQuery();

                        AdminPanel_AddUser_Mistakes.Content = "";
                        AdminPanel_AddUser_NewLogin.Text = "";
                    }
                    catch (Exception)
                    {
                        AdminPanel_AddUser_Mistakes.Content = "Такий логін вже існує!";
                    }


                }
                connection.Close();
                AdminPanel_CreateUsers();
            }
        }
        private void AdminPanel_CreateUsers() 
        {
            AdminPanel_UsersList_Data.Children.Clear();

            connection = new SqlConnection(connectionString);
            string strQ = "SELECT * FROM Users;";
            command = new SqlCommand(strQ, connection);

            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    Grid myGrid = new Grid();
                    myGrid.Height = 50;

                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    ColumnDefinition colDef3 = new ColumnDefinition();
                    ColumnDefinition colDef4 = new ColumnDefinition();
                    ColumnDefinition colDef5 = new ColumnDefinition();
                    myGrid.ColumnDefinitions.Add(colDef1);
                    myGrid.ColumnDefinitions.Add(colDef2);
                    myGrid.ColumnDefinitions.Add(colDef3);
                    myGrid.ColumnDefinitions.Add(colDef4);
                    myGrid.ColumnDefinitions.Add(colDef5);

                    for (int i = 0; i < 4; i++)
                    {
                        Label label = new Label();
                        label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        label.FontSize = 18;
                        label.HorizontalAlignment = HorizontalAlignment.Center;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.Content = reader.GetValue(i);
                        if (label.Content.ToString().Length == 0) label.Content = "NULL";
                        Grid.SetColumn(label, i);

                        myGrid.Children.Add(label);
                    }

                    CheckBox checkBox = new CheckBox();
                    Grid.SetColumn(checkBox, 4);
                    checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                    checkBox.VerticalAlignment = VerticalAlignment.Center;
                    checkBox.Click += CheckBox_Click;
                    bool check = (bool)reader.GetValue(4);
                    checkBox.IsChecked = check;

                    myGrid.Children.Add(checkBox);
                    AdminPanel_UsersList_Data.Children.Add(myGrid);
                }

                reader.Close();
            }

            connection.Close();
        }
        private void AdminPanel_Btn_Click(object sender, RoutedEventArgs e)
        {
            Login_FieldForPassword.Password = "";
            

            HideAllWindow();
            Login.Margin = new Thickness(0, 0, 0, 0);
        }

        private bool CheckCorrectPassword(TextBox passwordBox, Label Mistakes)
        {
            Mistakes.Content = "";

            string a = passwordBox.Text;
            if (a.Length < 6) Mistakes.Content += "Пароль має містити > 5 символів\n";
            bool check_number = false;
            bool check_upper = false;
            bool check_lower = false;
            bool check_spec = false;
            foreach (char x in a)
            {
                if ((int)x >= 65 && (int)x <= 90) check_upper = true;
                else if ((int)x >= 97 && (int)x <= 122) check_lower = true;
                else if (x == '+' || x == '-' || x == '*' || x == '/') check_spec = true;
                else if ((int)x >= 48 && (int)x <= 57) check_number = true;
            }

            if (!check_number) Mistakes.Content += "Пароль має містити цифри\n";
            if (!check_upper) Mistakes.Content += "Пароль має містити літери верхнього регістру\n";
            if (!check_lower) Mistakes.Content += "Пароль має містити літери нижнього регістру\n";
            if (!check_spec) Mistakes.Content += "Пароль має містити один із символів(+, -, *, /)\n";


            if (Mistakes.Content.ToString().Length > 0) return false;
            else return true;
        }
        private bool CheckCorrectPassword(TextBox passwordBox1, TextBox passwordBox2, Label Mistakes)
        {
            Mistakes.Content = "";

            string a = passwordBox1.Text;
            string b = passwordBox2.Text;

            if (a.Length < 6) Mistakes.Content += "Пароль має містити > 5 символів\n";
            bool check_number = false;
            bool check_upper = false;
            bool check_lower = false;
            bool check_spec = false;
            foreach (char x in a)
            {
                if ((int)x >= 65 && (int)x <= 90) check_upper = true;
                else if ((int)x >= 97 && (int)x <= 122) check_lower = true;
                else if (x == '+' || x == '-' || x == '*' || x == '/') check_spec = true;
                else if ((int)x >= 48 && (int)x <= 57) check_number = true;
            }

            if (a != b) Mistakes.Content += "Паролі в полях не збігаються\n";
            if (!check_number) Mistakes.Content += "Пароль має містити цифри\n";
            if (!check_upper) Mistakes.Content += "Пароль має містити літери верхнього регістру\n";
            if (!check_lower) Mistakes.Content += "Пароль має містити літери нижнього регістру\n";
            if (!check_spec) Mistakes.Content += "Пароль має містити один із символів(+, -, *, /)\n";


            if (Mistakes.Content.ToString().Length > 0) return false;
            else return true;
        }
        private void Top_Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void HideAllWindow()
        {
            foreach (Grid g in this.VisibleWindow.Children.OfType<Grid>())
            {
                g.Margin = new Thickness(1055, 0, -1055, 0);
            }
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Grid g = cb.Parent as Grid;

            Label lb = g.Children[0] as Label;

            bool check = (bool)cb.IsChecked;

            connection = new SqlConnection(connectionString);
            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string strQ = "UPDATE Users SET Status ='" + check + "' ";
                strQ += "WHERE Login = '" + lb.Content + "';";
                
                try
                {
                    command = new SqlCommand(strQ, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.Close();
        }

        private void Exit_Btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
