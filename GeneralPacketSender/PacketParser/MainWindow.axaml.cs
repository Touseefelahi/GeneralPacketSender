using Avalonia;
using Avalonia.Controls;
using PacketParser.ViewModels;
using System.Threading.Tasks;

namespace PacketParser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(App.Current.Services);

#if DEBUG
            this.AttachDevTools();
#endif
            //Task.Run(async () =>
            //{
            //    await Task.Delay(5000);
            //           LogGrid.
            //});
        }
    }
}