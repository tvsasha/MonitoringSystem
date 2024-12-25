using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace MonitoringSystem2
{
    public partial class EditUserWindow : Window
    {
        private MonitoringSystemDBEntities db;
        private Employee currentUser;

        public EditUserWindow(Employee user)
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            currentUser = user;
            LoadRoles();
            LoadUserData();
        }

        private void LoadRoles()
        {
            RoleComboBox.ItemsSource = db.Role.ToList();
            RoleComboBox.DisplayMemberPath = "RoleName";
            RoleComboBox.SelectedValuePath = "RoleID";
        }

        private void LoadUserData()
        {
            FullNameTextBox.Text = currentUser.FullName;
            RoleComboBox.SelectedValue = currentUser.RoleID;
            LoginTextBox.Text = currentUser.Login;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentUser.FullName = FullNameTextBox.Text;
            currentUser.Login = LoginTextBox.Text;
            currentUser.RoleID = (int)RoleComboBox.SelectedValue;

            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                currentUser.PasswordHash = CreateSHA256(PasswordBox.Password);
            }

            db.SaveChanges();
            MessageBox.Show("Пользователь успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
