using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Security.Cryptography;

namespace titov_zotov.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            ComboBoxRole.SelectedIndex = 1;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей ввода
            TextBoxFullName.Clear();
            TextBoxLogin.Clear();
            PasswordBoxPassword.Clear();
            PasswordBoxConfirmPassword.Clear();
            ComboBoxRole.SelectedIndex = 1;
            NavigationService?.Navigate(new AuthPage());
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            // Проверка заполнения полей
            if (string.IsNullOrEmpty(TextBoxFullName.Text) ||
                string.IsNullOrEmpty(TextBoxLogin.Text) ||
                string.IsNullOrEmpty(PasswordBoxPassword.Password) ||
                string.IsNullOrEmpty(PasswordBoxConfirmPassword.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка наличия логина в базе данных (здесь просто пример)
            if (IsLoginExists(TextBoxLogin.Text))
            {
                MessageBox.Show("Логин уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка формата пароля
            if (!IsValidPassword(PasswordBoxPassword.Password))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, хотя бы одну цифру и заглавную букву использовать только английские символы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка совпадения паролей
            if (PasswordBoxPassword.Password != PasswordBoxConfirmPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Здесь можно добавить код для регистрации пользователя в базе данных

            bool? isChecked = MyCheckbox.IsChecked;

            using (var db = new Titov_ZotovEntities())
            {
                string pass;

                pass = PasswordBoxPassword.Password;

                if (isChecked == true)
                {
                    pass = GetHash(PasswordBoxPassword.Password);
                }

                    User userObject = new User
                {
                    FIO = TextBoxFullName.Text,
                    Login = TextBoxLogin.Text,
                    Password = pass,
                    Role = ComboBoxRole.Text
                };
            
                db.User.Add(userObject);
                db.SaveChanges();
                NavigationService?.Navigate(new AuthPage());
                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }   }

        private bool IsLoginExists(string login)
        {
            using (var db = new Titov_ZotovEntities())
            {
                var user = db.User.AsNoTracking().FirstOrDefault(u => u.Login == TextBoxFullName.Text);

                if (user == null)
                {
                    return false;
                }
                return true;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 6)
                return false;

            if (!Regex.IsMatch(password, @"^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$"))
                return false;

            
            return true;
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
