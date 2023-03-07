using Avalonia;
using Avalonia.Controls;
using PacketParser.ViewModels;

namespace PacketParser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

#if DEBUG
            this.AttachDevTools();
#endif
        }
    }
}