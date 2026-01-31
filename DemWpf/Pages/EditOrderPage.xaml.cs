using DemWpf.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DemWpf.Pages
{
    public partial class EditOrderPage : Page
    {
        private Frame _frame;
        private User _currentUser;
        private Order _order;
        private Dem21Context db = new Dem21Context();

        public EditOrderPage(Frame frame, User user, int orderId)
        {
            InitializeComponent();
            _frame = frame;
            _currentUser = user;

            StatusBox.ItemsSource = new List<string>
            { "Новый", "В обработке", "Готов к выдаче", "Выдан", "Отменён" };

            PickUpPointBox.ItemsSource = db.PickUpPoints.ToList();
            ArticleBox.ItemsSource = db.Products.ToList();

            if (orderId != 0)
            {
                _order = db.Orders.First(o => o.OrderId == orderId);
                LoadOrder();
                Title = "Редактирование заказа";
            }
            else
            {
                _order = new Order();
                Title = "Создание заказа";
            }

            DeleteButton.Visibility = orderId != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadOrder()
        {
            var orderProduct = db.OrderedProducts.FirstOrDefault(op => op.OrderId == _order.OrderId);
            if (orderProduct != null)
            {
                Title = "Редактирование заказа";
                ArticleBox.SelectedValue = orderProduct.ProductId;
            }
            Title = "Создание заказа";
            StatusBox.SelectedItem = _order.Status;
            PickUpPointBox.SelectedItem = _order.PickUpPoint;
            OrderDatePicker.SelectedDate = _order.OrderDate;
            DeliveryDatePicker.SelectedDate = _order.DeliveryDate;
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StatusBox.SelectedItem == null ||
                    PickUpPointBox.SelectedItem == null ||
                    OrderDatePicker.SelectedDate == null ||
                    DeliveryDatePicker.SelectedDate == null ||
                    ArticleBox.SelectedValue == null
                    )
                {
                    MessageBox.Show("Заполните все поля",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _order.Status = StatusBox.SelectedItem.ToString();
                _order.PickUpPoint = PickUpPointBox.SelectedItem as PickUpPoint;
                _order.OrderDate = OrderDatePicker.SelectedDate.Value;
                _order.DeliveryDate = DeliveryDatePicker.SelectedDate.Value;
                //_order.Code = int.Parse(ArticleBox.SelectedValue.ToString);
#if DEBUG
                _order.UserId = 1;
#else
            _order.UserId = _currentUser.UserId;
#endif
                if (_order.DeliveryDate < _order.OrderDate)
                {
                    MessageBox.Show("Неправильно выбрана дата доставки",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (_order.OrderId == 0)
                    db.Orders.Add(_order);
                else
                    db.Orders.Update(_order);

                db.SaveChanges();

                _frame.GoBack();
            }
            catch (Exception ex) {
                MessageBox.Show("Ошибка сохранения",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            db.Orders.Remove(_order);
            db.SaveChanges();
            _frame.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _frame.GoBack();
        }
    }
}