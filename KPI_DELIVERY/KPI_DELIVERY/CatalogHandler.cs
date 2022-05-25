using System;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows.Media;


namespace KPI_DELIVERY
{
    public class CatalogHandler
    {
        private SQLHandler handler;
        private SqlConnection _connection;
        private User user;
        public CatalogHandler(User user) 
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _connection = new SqlConnection(connectionString);

            handler = new SQLHandler(_connection);
            this.user = user;
        }

        public void CreateCatalogPublications(string strQ, StackPanel catalog, Label CatalogAmount) 
        {
            catalog.Children.Clear();
            
            string strCheckRequest = $"SELECT * FROM dbo.Requests WHERE UserLogin = '{user.login}';";
            DataTable Request = handler.ConstructData(strCheckRequest);

            string strCheckSubscribe = $"SELECT * FROM dbo.Subscriptions WHERE UserLogin = '{user.login}';";
            DataTable Subscribe = handler.ConstructData(strCheckSubscribe);

            _connection.Open();
            SqlCommand command = new SqlCommand(strQ, _connection);
            SqlDataReader reader = command.ExecuteReader();

            int count = 0;
            bool background = true;
            while (reader.Read())
            {
                Grid myGrid = new Grid();
                myGrid.Height = 35;
                myGrid.Uid = reader.GetValue(0).ToString();
                count++;

                if (background)
                {
                    background = false;
                    myGrid.Background = new SolidColorBrush(Color.FromRgb(247, 246, 254));
                }
                else background = true;


                ColumnDefinition colDef1 = new ColumnDefinition();
                colDef1.Width = new GridLength(216.6);
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
                btn.FontSize = 15;
                btn.FontFamily = new FontFamily("Arial");
                btn.Width = 105;
                btn.Height = 28;

                bool existsRequst = Request.Select().ToList().Exists(row => row["PublicationID"].ToString() == reader.GetValue(0).ToString());
                bool existsSubscribe = Subscribe.Select().ToList().Exists(row => row["PublicationID"].ToString() == reader.GetValue(0).ToString());
                if (existsRequst)
                {
                    btn.Style = (Style)btn.FindResource("Expect_Button");
                    btn.Content = "Очікуйте";
                    btn.IsEnabled = false;
                }
                else if (existsSubscribe)
                {
                    btn.Style = (Style)btn.FindResource("Ordered_Button");
                    btn.Content = "Оформлено";
                    btn.IsEnabled = false;
                }
                else
                {
                    btn.Style = (Style)btn.FindResource("Order_Button");
                    btn.Content = "Замовити";
                    btn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    btn.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 146, 84));
                    btn.Foreground = new SolidColorBrush(Color.FromRgb(31, 146, 84));
                    //btn.Click += CatalogOrderBtn_Click;
                }

                myGrid.Children.Add(btn);
                catalog.Children.Add(myGrid);
            }
            reader.Close();
            _connection.Close();
            CatalogAmount.Content = count.ToString();
        }
        public void CreateCatalogSubscriptions(string strQ, StackPanel catalog, Label SubscribtionAmount) 
        {
            catalog.Children.Clear();

            _connection.Open();
            SqlCommand command = new SqlCommand(strQ, _connection);
            SqlDataReader reader = command.ExecuteReader();

            int count = 0;
            bool background = true;
            while (reader.Read())
            {
                Grid myGrid = new Grid();
                myGrid.Height = 35;
                myGrid.Uid = reader.GetValue(1).ToString();
                count++;

                if (background)
                {
                    background = false;
                    myGrid.Background = new SolidColorBrush(Color.FromRgb(247, 246, 254));
                }
                else background = true;


                ColumnDefinition colDef1 = new ColumnDefinition();
                colDef1.Width = new GridLength(216.6);
                ColumnDefinition colDef2 = new ColumnDefinition();
                ColumnDefinition colDef3 = new ColumnDefinition();
                ColumnDefinition colDef4 = new ColumnDefinition();

                myGrid.ColumnDefinitions.Add(colDef1);
                myGrid.ColumnDefinitions.Add(colDef2);
                myGrid.ColumnDefinitions.Add(colDef3);
                myGrid.ColumnDefinitions.Add(colDef4);

                for (int i = 2; i <= 4; i++)
                {
                    Label label = new Label();
                    label.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    label.FontSize = 15;
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;

                    string a = "";
                    if (i == 2)
                    {
                        a = '\u0022' + reader.GetValue(i).ToString() + '\u0022';
                    }
                    else if (i == 3)
                    {
                        DateTime dt = (DateTime)reader.GetValue(i);
                        a = dt.Date.ToString("d");
                    }
                    else if (i == 4)
                    {
                        DateTime dt = (DateTime)reader.GetValue(i);
                        a = dt.Date.ToString("d");
                    }

                    label.Content = a;

                    Grid.SetColumn(label, i - 2);
                    myGrid.Children.Add(label);
                }

                Button btn = new Button();
                Grid.SetColumn(btn, 3);
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.VerticalAlignment = VerticalAlignment.Center;
                btn.FontSize = 15;
                btn.FontFamily = new FontFamily("Arial");
                btn.Width = 85;
                btn.Height = 28;
                btn.Style = (Style)btn.FindResource("Download_Button");
                btn.Content = "Скачати";
                btn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(18, 200, 71));
                btn.Foreground = new SolidColorBrush(Color.FromRgb(18, 200, 71));
                btn.IsEnabled = true;
                //btn.Click += SubscribtionOrderBtn_Click;

                myGrid.Children.Add(btn);
                catalog.Children.Add(myGrid);
            }
            reader.Close();
            _connection.Close();
            SubscribtionAmount.Content = count.ToString();
        }
    }
}
