using System.Windows.Controls;

namespace KPI_DELIVERY
{
    public class Street
    {
        private int _streetID { get; set; }
        private string _nameStreet { get; set; }
        public Street(int streetID, string nameStreet)
        {
            this._streetID = streetID;
            this._nameStreet = nameStreet;
        }
        public ComboBoxItem ToComboBoxItem()
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = _nameStreet;
            item.Uid = _streetID.ToString();

            return item;
        }
    }
}
