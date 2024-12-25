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
        private MonitoringSystemDBEntities db;
        public int EmployeeID;
        public CalibratorWindow(int employeeID)
        {
            EmployeeID = employeeID;
            InitializeComponent();
            db = new MonitoringSystemDBEntities(); // Убедитесь, что это правильный контекст данных для вашей базы
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка данных для ComboBox с типами калибровки
            var calibrationTypes = db.CalibrationType.ToList();
            CalibrationTypeComboBox.ItemsSource = calibrationTypes;
            CalibrationTypeComboBox.DisplayMemberPath = "CalibrationName"; // Название калибровки
            CalibrationTypeComboBox.SelectedValuePath = "CalibrationTypeID";

        }


        private void CalibrationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalibrationTypeComboBox.SelectedValue != null)
            {
                int calibrationTypeId = (int)CalibrationTypeComboBox.SelectedValue;

                // Подгрузка эталонных значений для выбранного типа калибровки
                var referenceValues = db.ReferenceValue
                                         .Where(r => r.CalibrationTypeID == calibrationTypeId)
                                         .Select(r => new { r.ReferenceValueID, r.ValueName, r.Value })
                                         .ToList();

                // Если есть эталонные значения, то отображаем первое
                if (referenceValues.Any())
                {
                    ReferenceValueTextBox.Text = referenceValues.First().Value; // Заполняем эталонным значением для информации
                }
                else
                {
                    ReferenceValueTextBox.Clear(); // Очищаем, если значения отсутствуют
                }
            }
        }


        private void SaveCalibration_Click(object sender, RoutedEventArgs e)
        {
            if (CalibrationTypeComboBox.SelectedValue != null)
            {
                try
                {
                    // Получаем выбранный тип калибровки
                    int calibrationTypeId = (int)CalibrationTypeComboBox.SelectedValue;
                    string description = DescriptionTextBox.Text;

                    // Создаем новую запись калибровки
                    var calibration = new Calibration
                    {
                        CalibrationTypeID = calibrationTypeId,
                        Description = description,
                        CalibrationDate = DateTime.Now,
                        EmployeeID = EmployeeID // Учитываем переданный идентификатор сотрудника
                    };

                    // Сохраняем запись в базе данных
                    db.Calibration.Add(calibration);
                    db.SaveChanges();

                    MessageBox.Show("Калибровка успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении калибровки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тип калибровки.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            CalibratorsWindows window = new CalibratorsWindows(EmployeeID);
            window.Show();
            this.Close();
        }





    }
}
