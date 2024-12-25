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
    /// Логика взаимодействия для GuestWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        MonitoringSystemDBEntities db;

        public GuestWindow()
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            LoadData();
        }

        private void LoadData()
        {
            // Load Test Results
            var testResults = from test in db.Test
                              join result in db.TestResult on test.TestID equals result.TestID
                              join employee in db.Employee on test.EmployeeID equals employee.EmployeeID
                              join sensor in db.Sensor on test.SensorID equals sensor.SensorID
                              join deviceType in db.DeviceType on sensor.DeviceTypeID equals deviceType.DeviceTypeID
                              join testType in db.TestType on test.TestTypeID equals testType.TestTypeID
                              select new
                              {
                                  TestID = test.TestID,
                                  TestDate = test.TestDate,
                                  Tester = employee.FullName,
                                  Sensor = deviceType.TypeName,
                                  TestType = testType.TestName,
                                  MeasuredValue = result.MeasuredValue,
                                  Deviation = result.Deviation,
                                  Description = result.Description
                              };

            TestResultsDataGrid.ItemsSource = testResults.ToList();

            // Load Calibration Results
            var calibrationResults = from calibration in db.Calibration
                                     join calibrationType in db.CalibrationType on calibration.CalibrationTypeID equals calibrationType.CalibrationTypeID
                                     join employee in db.Employee on calibration.EmployeeID equals employee.EmployeeID
                                     select new
                                     {
                                         CalibrationID = calibration.CalibrationID,
                                         CalibrationDate = calibration.CalibrationDate,
                                         Calibrator = employee.FullName,
                                         CalibrationType = calibrationType.CalibrationName
                                     };

            CalibrationResultsDataGrid.ItemsSource = calibrationResults.ToList();
        }

        private void QuitButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
