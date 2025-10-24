using System.Windows;

namespace Client_ConceitoCadastro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(DeliveryViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;            
        }
    }
}