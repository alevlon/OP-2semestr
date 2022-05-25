using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace KPI_DELIVERY
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private SQLHandler handler;

        private User user;
        private Publication publication;

        public UserWindow(User _user)
        {
            InitializeComponent();
            user = _user;

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            handler = new SQLHandler(connection);

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
            SettingsnGetStreets();

            HideAllWindow();
            Settings.Margin = new Thickness(280, 72, 20, 42);
        }
        private void SettingsCatalog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Відкрити каталог
        {
            string sqlR = "SELECT * FROM dbo.Publications";
            CatalogFillingContent(sqlR);

            HideAllWindow();
            Catalog.Margin = new Thickness(280, 72, 20, 42);
        }
        private void SettingsSubscribtion_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Відкрити підписки
        {
            HideAllWindow();

            string strQ = "SELECT dbo.Users.Login, dbo.Subscriptions.SubscribeID, dbo.Publications.Title, dbo.Subscriptions.DateBegin, dbo.Subscriptions.DateEnd ";
            strQ += "FROM dbo.Users INNER JOIN ";
            strQ += "dbo.Subscriptions ON dbo.Users.Login = dbo.Subscriptions.UserLogin INNER JOIN ";
            strQ += "dbo.Publications ON dbo.Subscriptions.PublicationID = dbo.Publications.PublicationID ";
            strQ += $"WHERE(dbo.Users.Login = '{user.login}')";

            SubscribtionFillingContent(strQ);
            Subscribtion.Margin = new Thickness(280, 72, 20, 42);

        }


        //Налаштування
        private void SettingsnGetStreets() //Отримання списку з вулицями
        {
            SettingsStreets.Items.Clear();

            foreach (Street street in handler.GetAllStreets())
            {
                SettingsStreets.Items.Add(street.ToComboBoxItem());
            }
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
                    if (handler.UpdateOldAccount(user.login, Password, Surname, Firstname, Middlename, StreetID, House)) 
                    {
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
                    else throw new Exception();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
            CatalogHandler catalogHandler = new CatalogHandler(user);

            catalogHandler.CreateCatalogPublications(strQ, CatalogPanel, CatalogAmount);
            foreach (Grid grid in CatalogPanel.Children) 
            {
                ((Button)grid.Children[3]).Click += CatalogOrderBtn_Click;
            }

        }
        private void CatalogOrderBtn_Click(object sender, RoutedEventArgs e) //Замовити видання з каталогу
        {
            Button btn = sender as Button;
            Grid g = btn.Parent as Grid;

            string strQ = $"SELECT * FROM dbo.Publications WHERE PublicationID = '{g.Uid}';";
            DataTable dt = handler.ConstructData(strQ);

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

                CheckoutOpen();
            }
        }


        //Оформлення підписки
        private void CheckoutBackBtn_Click(object sender, RoutedEventArgs e) //Повернутися
        {
            HideAllWindow();
            Catalog.Margin = new Thickness(280, 72, 20, 42);
        }
        private void CheckoutSubmit_Click(object sender, RoutedEventArgs e) //Підтвердити підписку
        {
            string login = user.login;
            int publicationId = publication.ID;
            int period = Convert.ToInt32((CheckoutPeriod.SelectedItem as ComboBoxItem).Content);

            if (handler.CreateRequest(login, publicationId, period)) MessageBox.Show("Ваша заявка прийнята, після підтвердження вона з'явиться у Вашому каталозі підписок");
            HideAllWindow();
        }
        private void CheckoutOpen() 
        {
            HideAllWindow();
            Checkout.Margin = new Thickness(280, 72, 20, 42);
        }


        //Підписки
        private void SubscribtionFillingContent(string strQ) //Заповнити підписки
        {
            CatalogHandler catalogHandler = new CatalogHandler(user);

            catalogHandler.CreateCatalogSubscriptions(strQ, SubscribtionPanel, SubscribtionAmount);
            foreach (Grid grid in SubscribtionPanel.Children) 
            {
                ((Button)grid.Children[3]).Click += SubscribtionOrderBtn_Click;
            }
            
        }
        private void SubscribtionSort(object sender, MouseButtonEventArgs e) //Сортування підписок
        {
            Label label = sender as Label;

            string strQ = "SELECT dbo.Users.Login, dbo.Subscriptions.SubscribeID, dbo.Publications.Title, dbo.Subscriptions.DateBegin, dbo.Subscriptions.DateEnd ";
            strQ += "FROM dbo.Users INNER JOIN ";
            strQ += "dbo.Subscriptions ON dbo.Users.Login = dbo.Subscriptions.UserLogin INNER JOIN ";
            strQ += "dbo.Publications ON dbo.Subscriptions.PublicationID = dbo.Publications.PublicationID ";
            strQ += $"WHERE(dbo.Users.Login = '{user.login}') ";

            string searchExpression = SubscribtionSearch.Text;

            if (label.Name == "SubscribtionSortByName")
            {
                if (searchExpression.Length == 0) strQ += "ORDER BY dbo.Publications.Title;";
                else strQ += $"AND (dbo.Publications.Title LIKE '%{searchExpression}%') ORDER BY dbo.Publications.Title;";
            }
            else if (label.Name == "SubscribtionSortByStartDate") 
            {
                if (searchExpression.Length == 0) strQ += "ORDER BY dbo.Subscriptions.DateBegin;";
                else strQ += $"AND (dbo.Publications.Title LIKE '%{searchExpression}%') ORDER BY dbo.Subscriptions.DateBegin;";
            }
            else if (label.Name == "SubscribtionSortByEndDate")
            {
                if (searchExpression.Length == 0) strQ += "ORDER BY dbo.Subscriptions.DateEnd;";
                else strQ += $"AND (dbo.Publications.Title LIKE '%{searchExpression}%') ORDER BY dbo.Subscriptions.DateEnd;";
            }

            SubscribtionFillingContent(strQ);
        }
        private void SubscribtionSearch_TextChanged(object sender, TextChangedEventArgs e) //Пошук за підписками
        {
            string searchExpression = (sender as TextBox).Text;

            string strQ = "SELECT dbo.Users.Login, dbo.Subscriptions.SubscribeID, dbo.Publications.Title, dbo.Subscriptions.DateBegin, dbo.Subscriptions.DateEnd ";
            strQ += "FROM dbo.Users INNER JOIN ";
            strQ += "dbo.Subscriptions ON dbo.Users.Login = dbo.Subscriptions.UserLogin INNER JOIN ";
            strQ += "dbo.Publications ON dbo.Subscriptions.PublicationID = dbo.Publications.PublicationID ";
            strQ += $"WHERE (dbo.Users.Login = '{user.login}') AND (dbo.Publications.Title LIKE '%{searchExpression}%')";

            SubscribtionFillingContent(strQ);
        }
        private void SubscribtionOrderBtn_Click(object sender, RoutedEventArgs e) 
        {
            Button btn = sender as Button;
            Grid g = btn.Parent as Grid;

            string strQ = "SELECT dbo.Publications.Title, dbo.Publications.Price, dbo.Users.Surname, dbo.Users.Firstname, dbo.Users.Middlename, ";
            strQ += "dbo.Subscriptions.DateBegin, dbo.Subscriptions.DateEnd, dbo.Streets.NameStreet, dbo.Location.HouseNumber, dbo.Subscriptions.SubscribeID ";
            strQ += "FROM dbo.Subscriptions INNER JOIN ";
            strQ += "dbo.Users ON dbo.Subscriptions.UserLogin = dbo.Users.Login INNER JOIN ";
            strQ += "dbo.Location ON dbo.Users.Login = dbo.Location.UserLogin INNER JOIN ";
            strQ += "dbo.Streets ON dbo.Location.StreetId = dbo.Streets.StreetID INNER JOIN ";
            strQ += "dbo.Publications ON dbo.Subscriptions.PublicationID = dbo.Publications.PublicationID ";
            strQ += $"WHERE(dbo.Subscriptions.SubscribeID = {g.Uid})";

            DataTable dt = handler.ConstructData(strQ);

            if (dt.Rows.Count == 1)
            {
                string Title = dt.Rows[0][0].ToString();
                double Price = Convert.ToDouble(dt.Rows[0][1]);
                string Surname = dt.Rows[0][2].ToString();
                string Firstname = dt.Rows[0][3].ToString();
                string Middlename = dt.Rows[0][4].ToString();
                string DateBegin = ((DateTime)dt.Rows[0][5]).Date.ToString("d");
                string DateEnd = ((DateTime)dt.Rows[0][6]).Date.ToString("d");
                string StreetName = dt.Rows[0][7].ToString();
                int HouseNumber =   Convert.ToInt32(dt.Rows[0][8].ToString());
                string Address = $"вул. {StreetName}, буд. {HouseNumber}";

                TicketTitle.Content = '\u0022' + Title + '\u0022';
                TicketPrice.Content = "$" + Price;
                TicketSurname.Content = Surname;
                TicketFirstname.Content = Firstname;
                TicketMiddlename.Content = Middlename;
                TicketDateBegin.Content = DateBegin;
                TicketDateEnd.Content = DateEnd;
                TicketAddress.Content = Address;

                TicketOpen();
            }
        }


        //Чек
        private void TicketBackBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAllWindow();
            Subscribtion.Margin = new Thickness(280, 72, 20, 42);
        }
        private void TicketOpen()
        {
            HideAllWindow();
            Ticket.Margin = new Thickness(280, 72, 20, 42);
        }
        private void TicketDownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            string Title = TicketTitle.Content.ToString();
            string Price = TicketPrice.Content.ToString();
            string Surname = TicketSurname.Content.ToString();
            string Firstname = TicketFirstname.Content.ToString();
            string Middlename = TicketMiddlename.Content.ToString();
            string DateBegin = TicketDateBegin.Content.ToString();
            string DateEnd = TicketDateEnd.Content.ToString();
            string Address = TicketAddress.Content.ToString();

            string StrForTicket = $"Назва публікації - {Title}\nЦіна - {Price}\nВаше прізвище - {Surname}\nІм'я - {Firstname}\nПо батькові - {Middlename}\n";
            StrForTicket += $"Дата початку підписки - {DateBegin}\nДата закінчення підписки - {DateEnd}\nАдреса - {Address}";

            DownloadTxtFile downloadTicket = new DownloadTxtFile(StrForTicket);
            downloadTicket.SaveFile();

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
