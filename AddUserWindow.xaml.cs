using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace MonitoringSystem2
{
    public partial class AddUserWindow : Window
    {
        private MonitoringSystemDBEntities db;

        public AddUserWindow()
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            LoadRoles();
        }

        private void LoadRoles()
        {
            RoleComboBox.ItemsSource = db.Role.ToList();
            RoleComboBox.DisplayMemberPath = "RoleName";
            RoleComboBox.SelectedValuePath = "RoleID";
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newUser = new Employee
            {
                FullName = FullNameTextBox.Text,
                Login = LoginTextBox.Text,
                PasswordHash = CreateSHA256(PasswordBox.Password),
                RoleID = (int)RoleComboBox.SelectedValue
            };

            db.Employee.Add(newUser);
            db.SaveChanges();
            MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

        public static string CreateSHA256(string input)
        {
            string PassWithSalt = input + "Salt";
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(PassWithSalt));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
