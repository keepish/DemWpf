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
            LoadSuppliers();

            SupplierFilterBox.SelectedIndex = 0;
            SortBox.SelectedItem = 0;
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

        private void LoadSuppliers()
        {
            using var db = new Dem21Context();

            SupplierFilterBox.Items.Clear();
            SupplierFilterBox.Items.Add("Все поставщики");

            var suppliers = db.Suppliers.Select(x => x.Supplier1);

            foreach (var supplier in suppliers)
            {
                SupplierFilterBox.Items.Add(supplier);
            }

        }

        private void FilterChanged(object sender, EventArgs e)
        {
            IEnumerable<Product> result = _products;

            string search = SearchTextBox.Text;

            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(p =>
                    p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Manufacturer.Manufacturer1.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Supplier.Supplier1.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Article.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (SupplierFilterBox.SelectedIndex > 0)
            {
                string supplier = SupplierFilterBox.SelectedItem?.ToString();
                result = result.Where(p => p.Supplier.Supplier1 == supplier);
            }

            switch (SortBox.SelectedIndex)
            {
                case 1:
                    result = result.OrderBy(p => p.Amount); 
                    break;
                case 2:
                    result = result.OrderByDescending(p => p.Amount);
                    break;
            }

            ProductsListView.ItemsSource = result.ToList();
        }
    }
}
