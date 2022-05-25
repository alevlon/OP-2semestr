using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI_DELIVERY
{
    public class Operator //дані користувача
    {
        public int ID { get; set; }
        public string password { get; set; }
        public string exception { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public Operator(int ID, string password)
        {
            this.ID = ID;
            this.password = password;
            this.exception = "";
        }
        public Operator(int ID, string password, string surname, string firstname, string middlename) : this(ID, password)
        {
            this.surname = surname;
            this.firstname = firstname;
            this.middlename = middlename;
        }
    }
}
