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
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Page
    {
        public AdminMenu()
        {
            InitializeComponent();
            DataGridUser.ItemsSource = Titov_ZotovEntities.GetContext().User.ToList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddUserPage(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = DataGridUser.SelectedItems.Cast<User>().ToList();
            if(MessageBox.Show($"Вы точно хотите удалить записи" +
                $" в количестве {usersForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);

            try
            {
                Titov_ZotovEntities.GetContext().User.RemoveRange(usersForRemoving);
                Titov_ZotovEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно удалены!");

                DataGridUser.ItemsSource = Titov_ZotovEntities.GetContext().User.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DataGridUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddUserPage((sender as Button).DataContext as User));
        }
    }
}
