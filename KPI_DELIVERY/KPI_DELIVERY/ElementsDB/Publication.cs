using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI_DELIVERY
{
    public class Publication //дані видання
    {
        public int ID { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public bool type { get; set; }

        public Publication(int ID, string Title, double Price, bool Type)
        {
            this.ID = ID;
            this.title = Title;
            this.price = Price;
            this.type = Type;
        }
    }
}
