using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MonitoringSystem2
{
    /// <summary>
    /// Логика взаимодействия для CalibratorWindow.xaml
    /// </summary>
    public partial class CalibratorWindow : Window
    {
        public CalibratorWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Load data for CalibrationTypeComboBox from the database
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            // Logic for exporting the calibration data to a PDF
        }
    }
}
