using System.Linq;
using System.Windows;

namespace MonitoringSystem2
{
    public partial class EditEquipmentWindow : Window
    {
        private MonitoringSystemDBEntities db;
        private Equipment currentEquipment;

        public EditEquipmentWindow(Equipment equipment)
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            currentEquipment = equipment;
            LoadDeviceTypes();
            LoadEquipmentData();
        }

        private void LoadDeviceTypes()
        {
            DeviceTypeComboBox.ItemsSource = db.DeviceType.ToList();
            DeviceTypeComboBox.DisplayMemberPath = "TypeName";
            DeviceTypeComboBox.SelectedValuePath = "DeviceTypeID";
        }

        private void LoadEquipmentData()
        {
            DeviceTypeComboBox.SelectedValue = currentEquipment.DeviceTypeID;
            ModelTextBox.Text = currentEquipment.Model;
            ManufacturerTextBox.Text = currentEquipment.Manufacturer;
            CommissionDatePicker.SelectedDate = currentEquipment.CommissionDate;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceTypeComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                string.IsNullOrWhiteSpace(ManufacturerTextBox.Text) ||
                CommissionDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentEquipment.DeviceTypeID = (int)DeviceTypeComboBox.SelectedValue;
            currentEquipment.Model = ModelTextBox.Text;
            currentEquipment.Manufacturer = ManufacturerTextBox.Text;
            currentEquipment.CommissionDate = CommissionDatePicker.SelectedDate.Value;

            db.SaveChanges();
            MessageBox.Show("Оборудование успешно обновлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

    }
}
