using DemWpf.Pages;
using System.Windows;
using System.Windows.Controls;

namespace DemWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigated += MainFrame_Navigated;

            MainFrame.Navigate(new LoginPage(MainFrame));
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.Title = (MainFrame.Content as Page)?.Title;
        }
    }
}