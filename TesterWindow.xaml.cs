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
    /// Логика взаимодействия для TesterWindow.xaml
    /// </summary>
    public partial class TesterWindow : Window
    {
        MonitoringSystemDBEntities db;
        public int EmployeeID;
        public TesterWindow(int employeeID)
        {
            EmployeeID = employeeID;
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            LoadData();
        }

        private void LoadData()
        {
            var testTypes = db.TestType.ToList();
            TestTypeComboBox.ItemsSource = testTypes;
            TestTypeComboBox.DisplayMemberPath = "TestName";
            TestTypeComboBox.SelectedValuePath = "TestTypeID";

            var sensors = from sensor in db.Sensor
                          join deviceType in db.DeviceType on sensor.DeviceTypeID equals deviceType.DeviceTypeID
                          select new
                          {
                              SensorID = sensor.SensorID,
                              TypeName = deviceType.TypeName
                          };

            DeviceComboBox.ItemsSource = sensors.ToList();
            DeviceComboBox.DisplayMemberPath = "TypeName";
            DeviceComboBox.SelectedValuePath = "SensorID";
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestTypeComboBox.SelectedValue != null && DeviceComboBox.SelectedValue != null)
            {
                int testTypeId = (int)TestTypeComboBox.SelectedValue;
                int sensorId = (int)DeviceComboBox.SelectedValue;
                int employeeId = EmployeeID;
                DateTime testDate = DateTime.Now;
                string description = DescriptionTextBox.Text;

                var test = new Test
                {
                    TestTypeID = testTypeId,
                    SensorID = sensorId,
                    EmployeeID = employeeId,
                    TestDate = testDate,
                    Description = description
                };

                db.Test.Add(test);
                db.SaveChanges();

                MessageBox.Show("Параметры теста успешно записаны!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                var resultWindow = new TestResultWindow(test.TestID, EmployeeID);
                this.Hide();
                resultWindow.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите как тип теста, так и датчик", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
