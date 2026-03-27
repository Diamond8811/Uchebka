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
    /// Логика взаимодействия для PartnerSalesHistoryPage.xaml
    /// </summary>
    public partial class PartnerSalesHistoryPage : Page
    {
        private Partners _partner;

        public string PartnerName => _partner?.NamePartner ?? "неизвестный партнёр";

        public PartnerSalesHistoryPage(Partners partner)
        {
            InitializeComponent();
            _partner = partner;
            DataContext = this;
            LoadSalesHistory();
        }

        private void LoadSalesHistory()
        {
            try
            {
                var db = ConnectionClass.comfortEntities;

                var completedRequests = db.Request
                    .Where(r => r.Id_partner == _partner.Id_partner && r.Id_status == 5)
                    .Select(r => r.Id_request)
                    .ToList();

                if (completedRequests.Any())
                {
                    var sales = db.RequestDetails
                        .Where(rd => completedRequests.Contains(rd.Id_request.Value))
                        .Select(rd => new
                        {
                            ProductName = rd.Products.NameProduct,
                            Quantity = rd.Quantity,
                            SaleDate = rd.Request.RequestDate
                        })
                        .OrderByDescending(s => s.SaleDate)
                        .ToList();

                    SalesListView.ItemsSource = sales;
                }
                else
                {
                    SalesListView.ItemsSource = new List<object>();
                    MessageBox.Show("Для этого партнёра пока нет завершённых продаж.", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории продаж: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
