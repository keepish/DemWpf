using DemWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace DemWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Frame _frame;

        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();
            _frame = mainFrame;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            using var db = new Dem21Context();

            try
            {
                var user = db.Users.Include(u => u.Role).FirstOrDefault(u =>
                    u.Login == LoginTextBox.Text &&
                    u.Password == PasswordBox.Password);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _frame.Navigate(new ProductPage(_frame, user));
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла непредвиденная ошибка",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(new ProductPage(_frame, null));
        }
    }
}
