using DemWpf.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
        private Dem21Context db = new Dem21Context();

        public EditProductPage(Frame frame, int productId)
        {
            InitializeComponent();
            _frame = frame;

            LoadLists();

            if (productId != 0)
            {
                LoadProduct(productId);
                Title = "Редактирование товара";
            }
            else
            {
                _product = new Product();
                Title = "Добавление товара";
            }

        }

        private void LoadLists()
        {
            CategoryBox.ItemsSource = db.Categories.ToList();
            ManufacturerBox.ItemsSource = db.Manufacturers.ToList();
            SupplierBox.ItemsSource = db.Suppliers.ToList();
        }

        private void LoadProduct(int productId)
        {
            _product = db.Products.First(x => x.ProductId == productId);

            ArticleBox.Text = _product.Article;
            NameBox.Text = _product.Name;
            DescriptionBox.Text = _product.Description;
            PriceBox.Text = _product.Price.ToString();
            UnitBox.Text = _product.Unit;
            DiscountBox.Text = _product.Discount.ToString();
            AmountBox.Text = _product.Amount.ToString();

            CategoryBox.SelectedItem = _product.Category;
            ManufacturerBox.SelectedItem = _product.Manufacturer;
            SupplierBox.SelectedItem = _product.Supplier;

            ProductImage.Source = new BitmapImage(new Uri(_product.PhotoPath));
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image|*.png;*.jpg";

            if (dialog.ShowDialog() != true)
                return;

            var image = new BitmapImage(new Uri(dialog.FileName));

            if (image.PixelWidth > 300 || image.PixelHeight > 200)
            {
                MessageBox.Show("Максимальный размер изображения - 300x200 пикселей",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var folder = Path.Combine(Environment.CurrentDirectory, "Images");
            Directory.CreateDirectory(folder);
            if (ProductImage.Source is not null)
            {
                _newImagePath = Path.Combine(folder, Path.GetFileName(dialog.FileName));
                File.Copy(dialog.FileName, _newImagePath, true);
                ProductImage.Source = image;
            }
            else
            {
                _newImagePath = Path.Combine(folder, Path.GetFileName(dialog.FileName));
                File.Copy(dialog.FileName, _newImagePath, true);
                ProductImage.Source = image;
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _frame.GoBack();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            _product.Article = ArticleBox.Text;
            _product.Name = NameBox.Text;
            _product.Description = DescriptionBox.Text;
            try
            {
                _product.Price = decimal.Parse(PriceBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Необходимо заполнить цену",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (_product.Price < 0)
            {
                MessageBox.Show("Цена не может быть меньше 0",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _product.Unit = UnitBox.Text;
            try
            {
                _product.Amount = int.Parse(AmountBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Необходимо заполнить количество",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_product.Amount < 0)
            {
                MessageBox.Show("Количество не может быть меньше 0",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                _product.Discount = int.Parse(DiscountBox.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Необходимо заполнить скидку",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return ;
            }
            
            if (_product.Discount < 0)
            {
                MessageBox.Show("Скидка не может быть меньше 0",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _product.Supplier = SupplierBox.SelectedItem as Supplier;
            _product.Category = CategoryBox.SelectedItem as Category;
            _product.Manufacturer = ManufacturerBox.SelectedItem as Manufacturer;


            if (_product.ProductId != 0 && !string.IsNullOrEmpty(_product.PhotoPath))
            {
                try { File.Delete(_product.PhotoPath); }
                catch {}
            }
            try
            {
                db.Products.Update(_product);
                db.SaveChanges();
                _frame.GoBack();
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла непредвиденная ошибка",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            bool isOrdered = db.OrderedProducts.Any(p => p.ProductId == _product.ProductId);

            if (isOrdered)
            {
                MessageBox.Show("Товар не может быть удалён т.к. содержится в заказе",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                db.Products.Remove(_product);
                db.SaveChanges();
                _frame.GoBack();
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла непредвиденная ошибка",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }
    }
}
