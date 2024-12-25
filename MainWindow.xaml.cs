using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonitoringSystem2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MonitoringSystemDBEntities db;

        public MainWindow()
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string pass = CreateSHA256(PasswordTextBox.Text);
            var user = db.Employee.FirstOrDefault(u => u.Login == login && u.PasswordHash == pass);
            if (user != null)
            {
                OpenWindowByRole(user.RoleID, user.EmployeeID);
            }
            else
            {
                MessageBox.Show("Invalid login or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenWindowByRole(int roleId, int employeeID)
        {
            Window window = null;

            switch (roleId)
            {
                case 1: // Tester
                    window = new TestsWindow(employeeID);
                    break;
                case 2: // Calibrator
                    window = new CalibratorWindow();
                    break;
                case 3: // Admin
                    window = new AdminWindow();
                    break;
                case 4: // Guest
                    window = new GuestWindow();
                    break;
                default:
                    MessageBox.Show("Role not recognized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            this.Close();
            window.ShowDialog();

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
