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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace titov_zotov.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Page
    {
        public UserMenu()
        {
            InitializeComponent();
            CmbSorting.SelectedIndex = 0;

            UserRoleCheckBox.IsChecked = false;

            var currentUsers = Titov_ZotovEntities.GetContext().User.ToList();

            ListUser.ItemsSource = currentUsers;

            UpdateUsers();

            SearchTextBox.TextChanged += (s, e) => UpdateUsers();
            CmbSorting.SelectionChanged += (s, e) => UpdateUsers();
            UserRoleCheckBox.Checked += (s, e) => UpdateUsers();
            UserRoleCheckBox.Unchecked += (s, e) => UpdateUsers();

        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty; // Очищаем текст поиска
            CmbSorting.SelectedIndex = 0; // Устанавливаем сортировку по возрастанию
            UserRoleCheckBox.IsChecked = false; // Снимаем галочку с CheckBox

            UpdateUsers(); // Обновляем список пользователей
        }
        private void UpdateUsers()

        {
            var currentUsers = Titov_ZotovEntities.GetContext().User.ToList();

            currentUsers = currentUsers.Where(x => x.FIO.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();

            if (UserRoleCheckBox.IsChecked.Value)

                currentUsers = currentUsers.Where(x => x.Role.Contains("Пользователь")).ToList();

            if (CmbSorting.SelectedIndex == 0)

                ListUser.ItemsSource = currentUsers.OrderBy(x => x.FIO).ToList();

            else ListUser.ItemsSource = currentUsers.OrderByDescending(x => x.FIO).ToList();
        }
    }
}
