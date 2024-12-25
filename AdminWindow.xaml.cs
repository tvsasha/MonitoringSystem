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
        private MonitoringSystemDBEntities db;

        public AdminWindow()
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка данных для пользователей
            UsersDataGrid.ItemsSource = db.Employee.ToList();

            // Загрузка данных для оборудования
            EquipmentDataGrid.ItemsSource = db.Equipment.ToList();

            // Загрузка данных для датчиков
            SensorsDataGrid.ItemsSource = db.Sensor.ToList();

            // Загрузка данных для тестов
            TestsDataGrid.ItemsSource = db.Test.ToList();

            // Загрузка данных для эталонных значений
            ReferenceValuesDataGrid.ItemsSource = db.ReferenceValue.ToList();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow();
            if (addUserWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Employee selectedUser)
            {
                var editUserWindow = new EditUserWindow(selectedUser);
                if (editUserWindow.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Employee selectedUser)
            {
                db.Employee.Remove(selectedUser);
                db.SaveChanges();
                LoadData();
            }
        }
        private void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            var addEquipmentWindow = new AddEquipmentWindow();
            if (addEquipmentWindow.ShowDialog() == true)
            {
                LoadData(); // Обновление данных после добавления
            }
        }

        // Редактирование оборудования
        private void EditEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentDataGrid.SelectedItem is Equipment selectedEquipment)
            {
                var editEquipmentWindow = new EditEquipmentWindow(selectedEquipment);
                if (editEquipmentWindow.ShowDialog() == true)
                {
                    LoadData(); // Обновление данных после редактирования
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите оборудование для редактирования.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Удаление оборудования
        private void DeleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentDataGrid.SelectedItem is Equipment selectedEquipment)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить оборудование '{selectedEquipment.Model}'?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    db.Equipment.Remove(selectedEquipment);
                    db.SaveChanges();
                    LoadData(); // Обновление данных после удаления
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите оборудование для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteTest_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditTest_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteSensor_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddSensor_Click(object sender, RoutedEventArgs e)
        {

        }
        private void EditSensor_Click(object sender, RoutedEventArgs e)
        {

        }
        private void EditReferenceValue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QuitButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
