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
    /// Логика взаимодействия для TestResultWindow.xaml
    /// </summary>
    public partial class TestResultWindow : Window
    {
        MonitoringSystemDBEntities db;
        private int testId;
        public int EmployeeID;
        public TestResultWindow(int testId, int employeeID)
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            this.testId = testId;
            EmployeeID = employeeID;
        }

        private void SaveResultButton_Click(object sender, RoutedEventArgs e)
        {
            string measuredValue = MeasuredValueTextBox.Text;
            string deviation = DeviationTextBox.Text;
            string description = DescriptionTextBox.Text;
            if (!string.IsNullOrEmpty(measuredValue) && !string.IsNullOrEmpty(deviation))
            {
                var testResult = new TestResult
                {
                    TestID = testId,
                    MeasuredValue = measuredValue,
                    Deviation = deviation,
                    Description = description
                };

                db.TestResult.Add(testResult);
                db.SaveChanges();

                MessageBox.Show("Результат теста успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                TestsWindow testsWindow = new TestsWindow(EmployeeID);
                testsWindow.Show();
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
