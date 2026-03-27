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
    /// Логика взаимодействия для PartnersAddEditPage.xaml
    /// </summary>
    public partial class PartnersAddEditPage : Page
    {
        Partners partners;
        public PartnersAddEditPage(Partners _partners)
        {

            InitializeComponent();
            partners = _partners;
            this.DataContext = partners;
            TypeCB.ItemsSource = ConnectionClass.comfortEntities.TypeOfBusiness.ToList();
            TypeCB.DisplayMemberPath = "NameBusiness";

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                partners.Id_type = (TypeCB.SelectedItem as TypeOfBusiness).Id_type;
                if (partners.Id_partner == 0)
                {
                    ConnectionClass.comfortEntities.Partners.Add(partners);
                }
                ConnectionClass.comfortEntities.SaveChanges();
                MessageBox.Show("Операция прошла успешно");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CanselBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}
