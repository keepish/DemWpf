using DemWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DemWpf.Pages
{
    public partial class OrderPage : Page
    {
        private Frame _frame;
        private User _currentUser;
        private List<Order> _orders;

        public OrderPage(Frame frame, User user)
        {
            InitializeComponent();
            _frame = frame;
            _currentUser = user;

            LoadOrders();
#if DEBUG
#else
            AddOrderButton.Visibility = _currentUser.Role.Name == "Администратор"
                ? Visibility.Visible
                : Visibility.Collapsed;
#endif
        }

        private void LoadOrders()
        {
            using var db = new Dem21Context();

            _orders = db.Orders
                .Include(o => o.PickUpPoint)
                .Include(o => o.User)
                .Include(o => o.OrderedProducts)
                    .ThenInclude(op => op.Product)
                .ToList();

            OrdersListView.ItemsSource = _orders;
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(new EditOrderPage(_frame, _currentUser, 0));
        }

        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
#else
            if (_currentUser.Role.Name != "Администратор")
                return;
#endif

            if (OrdersListView.SelectedItem is Order order)
                _frame.Navigate(new EditOrderPage(_frame, _currentUser, order.OrderId));
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            _frame.GoBack();
        }
    }
}