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
    public class DownloadTxtFile
    {
        private string _filling;
        public DownloadTxtFile(string Filling) 
        {
            this._filling = Filling;
        }

        public void SaveFile() 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.Title = "Зберегти";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, _filling);
        }
    }
}
