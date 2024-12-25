using System.Linq;
using System.Windows;

namespace MonitoringSystem2
{
    public partial class AddEquipmentWindow : Window
    {
        private MonitoringSystemDBEntities db;

        public AddEquipmentWindow()
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            LoadDeviceTypes();
        }

        private void LoadDeviceTypes()
        {
            DeviceTypeComboBox.ItemsSource = db.DeviceType.ToList();
            DeviceTypeComboBox.DisplayMemberPath = "TypeName";
            DeviceTypeComboBox.SelectedValuePath = "DeviceTypeID";
        }

        private void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceTypeComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                string.IsNullOrWhiteSpace(ManufacturerTextBox.Text) ||
                CommissionDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newEquipment = new Equipment
            {
                DeviceTypeID = (int)DeviceTypeComboBox.SelectedValue,
                Model = ModelTextBox.Text,
                Manufacturer = ManufacturerTextBox.Text,
                CommissionDate = CommissionDatePicker.SelectedDate.Value
            };

            db.Equipment.Add(newEquipment);
            db.SaveChanges();
            MessageBox.Show("Оборудование успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

    }
}
