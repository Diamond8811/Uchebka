using Uchebka.Model;
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

namespace Uchebka.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartnersListPage.xaml
    /// </summary>
    public partial class PartnersListPage : Page
    {
        private List<Partners> _allPartners;

        public PartnersListPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _allPartners = ConnectionClass.comfortEntities.Partners.ToList();
            TypeFilterComboBox.ItemsSource = ConnectionClass.comfortEntities.TypeOfBusiness.ToList();
            TypeFilterComboBox.SelectedIndex = -1;
            ApplyFilters();
            bool canEdit = CurrentUser.UserRole == "Admin" || CurrentUser.UserRole == "Manager";
            AddBtn.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
            EditBtn.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
            DeleteBtn.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ApplyFilters()
        {
            var filtered = _allPartners.AsEnumerable();

            string search = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    (p.NamePartner != null && p.NamePartner.ToLower().Contains(search)) ||
                    (p.SurnameDirector != null && p.SurnameDirector.ToLower().Contains(search)) ||
                    (p.NameDirector != null && p.NameDirector.ToLower().Contains(search)) ||
                    (p.PatronumicDirector != null && p.PatronumicDirector.ToLower().Contains(search)) ||
                    (p.Phone != null && p.Phone.ToLower().Contains(search)) ||
                    (p.Email != null && p.Email.ToLower().Contains(search))
                );
            }

            if (TypeFilterComboBox.SelectedItem is TypeOfBusiness selectedType)
                filtered = filtered.Where(p => p.Id_type == selectedType.Id_type);

            PartnersLV.ItemsSource = filtered.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();

        private void TypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            TypeFilterComboBox.SelectedIndex = -1;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnersLV.SelectedItem as Partners;
            if (selPartner != null)
                NavigationService.Navigate(new PartnersAddEditPage(selPartner));
            else
                MessageBox.Show("Вы не выбрали партнёра!");
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PartnersAddEditPage(new Partners()));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var partner = button?.Tag as Partners;
            if (partner != null)
                NavigationService.Navigate(new PartnerSalesHistoryPage(partner));
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenuPage());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnersLV.SelectedItem as Partners;
            if (selPartner == null)
            {
                MessageBox.Show("Вы не выбрали партнёра для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить партнёра \"{selPartner.NamePartner}\"?\nВсе связанные данные будут удалены.", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                var db = ConnectionClass.comfortEntities;
                db.Partners.Remove(selPartner);
                db.SaveChanges();

                _allPartners = db.Partners.ToList();
                ApplyFilters();

                MessageBox.Show("Партнёр успешно удалён.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
