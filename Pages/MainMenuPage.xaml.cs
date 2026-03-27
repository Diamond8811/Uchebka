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
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void PartnersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PartnersListPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.User = null;

            NavigationService.Navigate(new LoginPage());
        }
    }
}
