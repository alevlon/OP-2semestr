using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace KPI_DELIVERY
{
    public class SQLHandler 
    {
        private SqlConnection _connection;
        public SQLHandler(SqlConnection connection) 
        {
            this._connection = connection;
        }

        public bool CreateNewAccount(string login, string password, string surname, string firstname, string middlename, string streetID, string house) 
        {
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    string strQ = "INSERT INTO dbo.Users";
                    strQ += $" values('{login}', '{password}', '{surname}', '{firstname}', '{middlename}');";

                    SqlCommand command;
                    command = new SqlCommand(strQ, _connection);
                    command.ExecuteNonQuery();

                    strQ = "INSERT INTO dbo.Location";
                    strQ += $" values('{login}', '{streetID}', '{house}');";

                    command = new SqlCommand(strQ, _connection);
                    command.ExecuteNonQuery();

                    _connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    _connection.Close();
                    return false;
                }
            }

            _connection.Close();
            return false;
        }
        public bool CreateRequest(string login, int publicationId, int period)
        {
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    string strQ = "INSERT INTO dbo.Requests";
                    strQ += $" values('{login}', '{publicationId}', '{period}');";
                    SqlCommand command;

                    command = new SqlCommand(strQ, _connection);
                    command.ExecuteNonQuery();

                    _connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            _connection.Close();
            return false;
        }
        public bool CreateNewPublication(string title, string price, bool type) 
        {
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    string strQ = "INSERT INTO dbo.Publications";
                    strQ += $" values('{title}', '{price}', '{type}');";
                    SqlCommand command;

                    command = new SqlCommand(strQ, _connection);
                    command.ExecuteNonQuery();

                    _connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            _connection.Close();
            return false;
        }

        public bool UpdateOldAccount(string login, string password, string surname, string firstname, string middlename, string streetID, string house) 
        {
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    SqlCommand command;

                    string strQ = $"UPDATE Users SET Password = '{password}', Surname = '{surname}', Firstname = '{firstname}', Middlename = '{middlename}' ";
                    strQ += $"WHERE Login = '{login}';";
                    command = new SqlCommand(strQ, _connection);
                    command.ExecuteNonQuery();

                    strQ = $"UPDATE Location SET StreetId = '{streetID}', HouseNumber = '{house}' ";
                    strQ += $"WHERE UserLogin = '{login}';";
                    command = new SqlCommand(strQ, _connection);
                    command.ExecuteNonQuery();

                    _connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    _connection.Close();
                    return false;
                }
            }

            _connection.Close();
            return false;
        }

        public User AuthorizationUser(User user) 
        {
            string strQ = "SELECT * FROM Users WHERE Login='" + user.login + "';";
            DataTable dt = ConstructData(strQ);

            User result = new User(user.login, user.password);

            if (dt.Rows.Count == 0) result.exception = "Введіть справжній логін";
            else
            {
                if (dt.Rows[0][1].ToString() == user.password) 
                {

                    string surname = dt.Rows[0][2].ToString();
                    string firstname = dt.Rows[0][3].ToString();
                    string middlename = dt.Rows[0][4].ToString();

                    result.surname = surname;
                    result.firstname = firstname;
                    result.middlename = middlename;
                }
                else
                {
                    result.exception = "Невірна спроба входу";
                }
            }

            return result;
        }
        public Operator AuthorizationOperator(Operator @operator)
        {
            string strQ = "SELECT * FROM Operators WHERE OperatorID='" + @operator.ID + "';";
            DataTable dt = ConstructData(strQ);

            Operator result = new Operator(@operator.ID, @operator.password);

            if (dt.Rows.Count == 0) result.exception = "Введіть справжній ідентифікатор";
            else
            {
                if (dt.Rows[0][1].ToString() == @operator.password)
                {

                    string surname = dt.Rows[0][2].ToString();
                    string firstname = dt.Rows[0][3].ToString();
                    string middlename = dt.Rows[0][4].ToString();

                    result.surname = surname;
                    result.firstname = firstname;
                    result.middlename = middlename;
                }
                else
                {
                    result.exception = "Невірна спроба входу";
                }
            }

            return result;
        }

        public List<Street> GetAllStreets()
        {
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                string strQ = "SELECT * FROM dbo.Streets;";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand(strQ, _connection);

                List<Street> streetList = ConstructStreets(command);

                _connection.Close();
                return streetList;
            }


            _connection.Close();
            return null; 
        }
        private List<Street> ConstructStreets(SqlCommand command) 
        {
            try 
            {
                SqlDataReader reader = command.ExecuteReader();
                List<Street> streets = new List<Street>();


                while (reader.Read())
                {
                    streets.Add(new Street(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString()));
                }

                reader.Close();
                return streets;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public DataTable ConstructData(string strQ) 
        {
            try
            {
                _connection.Open();
                DataTable dt = new DataTable();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(strQ, _connection);

                    adapter.Fill(dt);
                }

                _connection.Close();
                return dt;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
