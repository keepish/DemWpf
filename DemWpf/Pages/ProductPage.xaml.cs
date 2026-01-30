using DemWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace DemWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private Frame _frame;
        private User _currentUser;
        private List<Product> _products;

        public ProductPage(Frame frame, Models.User user)
        {
            InitializeComponent();

            _frame = frame;
            _currentUser = user;
            
            LoadUser();
            LoadProducts();
        }

        private void LoadUser()
        {
            UserNameText.Text = _currentUser == null
                ? "Гость"
                : _currentUser.FullName;
        }

        private void LoadProducts()
        {
            using var db = new Dem21Context();

            _products = db.Products
                .Include(x => x.Manufacturer)
                .Include(x => x.Supplier)
                .Include(x => x.Category)
                .ToList();

            ProductsListView.ItemsSource = _products;
        }
    }
}
