using DemWpf.Models;
using Microsoft.EntityFrameworkCore;
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

namespace DemWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        private Frame _frame;
        private Product _product;
        private string _newImagePath;

        public EditProductPage(Frame frame, int productId)
        {
            InitializeComponent();

            _frame = frame;

            LoadList();

            if (productId is not 0)
            {
                LoadProduct(productId);
            }
            else
            {
                _product = new Product();
            }
        }

        private void LoadList()
        {
            using var db = new Dem21Context();

            CategoryBox.ItemsSource = db.Categories.ToList();
            ManufacturerBox.ItemsSource = db.Manufacturers.ToList();    
            SupplierBox.ItemsSource = db.Suppliers.ToList();
        }

        private void LoadProduct(int productId)
        {
            using var db = new Dem21Context();

            _product = db.Products.First(x => x.ProductId == productId);

            ArticleBox.Text = _product.Article;
            NameBox.Text = _product.Name;
            DescriptionBox.Text = _product.Description;
            PriceBox.Text = _product.Price.ToString();
            UnitBox.Text = _product.Unit.ToString();
            DiscountBox.Text = _product.Discount.ToString();
            AmountBox.Text = _product.Amount.ToString();

            CategoryBox.SelectedItem = _product.Category;
            ManufacturerBox.SelectedItem = _product.Manufacturer;
            SupplierBox.SelectedItem = _product.Supplier;

            ProductImage.Source = new BitmapImage(new Uri(_product.PhotoPath));
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
