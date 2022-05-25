using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI_DELIVERY
{
    public class User //дані користувача
    {
        public string login { get; set; }
        public string password { get; set; }
        public string exception { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
            this.exception = "";
        }
        public User(string login, string password, string surname, string firstname, string middlename) : this(login, password)
        {
            this.surname = surname;
            this.firstname = firstname;
            this.middlename = middlename;
        }
    }
}
