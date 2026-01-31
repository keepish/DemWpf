using DemWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

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

        public ProductPage(Frame frame, User user)
        {
            InitializeComponent();
            try
            {
                _frame = frame;
                _currentUser = user;

                _frame.Navigated += Frame_Navigated;

                LoadUser();
                LoadProducts();
                LoadSuppliers();

                SupplierFilterBox.SelectedIndex = 0;
                SortBox.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка подключения к базе данных",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            LoadProducts();
            FilterChanged(null, null);
        }

        private void LoadUser()
        {
            UserNameText.Text = _currentUser == null
                ? "Гость"
                : _currentUser.FullName;


#if DEBUG

#else
            if (_currentUser != null && (_currentUser.Role.Name == "Администратор" 
                || _currentUser.Role.Name == "Менеджер"))
            {
                FilterStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                FilterStackPanel.Visibility = Visibility.Collapsed;
            }
#endif
        }

        private void LoadProducts()
        {
            using var db = new Dem21Context();
            try
            {
                _products = db.Products
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Supplier)
                    .Include(x => x.Category).ToList();

                ProductListView.ItemsSource = _products;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка загрузки продкутов",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void LoadSuppliers()
        {
            using var db = new Dem21Context();
            try
            {
                SupplierFilterBox.Items.Clear();
                SupplierFilterBox.Items.Add("Все поставщики");

                var suppliers = db.Suppliers.Select(x => x.Name);

                foreach (var supplier in suppliers)
                {
                    SupplierFilterBox.Items.Add(supplier);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка загрузки поставщиков",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                    p.Manufacturer.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Supplier.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Article.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (SupplierFilterBox.SelectedIndex > 0)
            {
                string supplier = SupplierFilterBox.SelectedItem.ToString();
                result = result.Where(p => p.Supplier.Name == supplier);
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
            try
            {
                ProductListView.ItemsSource = result.ToList();
            }
            catch (Exception)
            {

                MessageBox.Show("Возникла ошибка применения фильтра",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }

        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (
#if DEBUG
                ProductListView.SelectedItem is Product product
#else
                && _currentUser != null &&
                   _currentUser.Role.Name == "Администратор" s
#endif
                )
            {
                _frame.Navigate(new EditProductPage(_frame, product.ProductId));
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            _frame.GoBack();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(new EditProductPage(_frame, 0));
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(new OrderPage(_frame, _currentUser));
        }
    }
}
