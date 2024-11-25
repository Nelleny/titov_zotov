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



namespace titov_zotov.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBoxPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            bool? isChecked = MyCheckbox.IsChecked;

            using (var db = new Titov_ZotovEntities())
            {
                string pass;

                pass = PasswordBoxPassword.Password;

                if (isChecked == true)
                {
                    pass = GetHash(PasswordBoxPassword.Password);
                }
                var user = db.User.AsNoTracking().FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == pass);

                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден!");
                    return;
                }
                else
                {
                    MessageBox.Show("Пользователь успешно найден!");

                    switch (user.Role)
                    {
                        case "администратор":
                            NavigationService?.Navigate(new AdminMenu());
                            break;
                        case "пользователь":
                            NavigationService?.Navigate(new UserMenu());
                            break;
                    }
                }
            }

        }

        private void ButtonRegl_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}
