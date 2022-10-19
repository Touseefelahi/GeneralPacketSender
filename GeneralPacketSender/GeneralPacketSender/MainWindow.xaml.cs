using GeneralPacketSender.Viewmodels;
using System.Windows;

namespace GeneralPacketSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new Mainviewmodel();
            InitializeComponent();
        }
    }
}
