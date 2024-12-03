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
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private User _currentUser = new User();
        public AddUserPage(User SelectedUser)
        {
            InitializeComponent();

            if (SelectedUser != null)
                _currentUser = SelectedUser;

            DataContext = _currentUser;

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            bool? isChecked = MyCheckbox.IsChecked;

            if (string.IsNullOrWhiteSpace(_currentUser.Login)) errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password)) errors.AppendLine("Укажите пароль!");
            if (isChecked == true)
            {
                _currentUser.Password = GetHash(Password.Text);
            }
            if (cmbRole.SelectedItem == null) // Проверяем, выбран ли элемент
            {
                errors.AppendLine("Выберите роль!");
            }
            else
            {
                _currentUser.Role = cmbRole.Text;
            }

            if (string.IsNullOrWhiteSpace(_currentUser.FIO)) errors.AppendLine("Укажите Ф.И.О.!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Проверка на существование логина в базе данных
            var existingUser = Titov_ZotovEntities.GetContext().User
                .FirstOrDefault(u => u.Login == _currentUser.Login);

            if (existingUser != null && _currentUser.ID == 0) // Если логин уже существует и это новый пользователь
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            if (_currentUser.ID == 0)
                Titov_ZotovEntities.GetContext().User.Add(_currentUser);

            try
            {
                Titov_ZotovEntities.GetContext().SaveChanges();
                NavigationService?.Navigate(new AdminMenu());
                FullName.Clear();
                Login.Clear();
                Password.Clear();
                Img.Clear();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            FullName.Clear();
            Login.Clear();
            Password.Clear();
            Img.Clear();
            NavigationService?.Navigate(new AdminMenu());
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
