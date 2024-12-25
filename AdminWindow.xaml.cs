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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void ManageSensors_Click(object sender, RoutedEventArgs e)
        {
            // Open sensor management window
        }

        private void ManageTests_Click(object sender, RoutedEventArgs e)
        {
            // Open test management window
        }

        private void ManageCalibrations_Click(object sender, RoutedEventArgs e)
        {
            // Open calibration management window
        }

        private void ManageReferenceValues_Click(object sender, RoutedEventArgs e)
        {
            // Open reference values management window
        }
    }
}
