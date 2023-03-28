using Avalonia.Controls;
using PacketParser.ViewModels;

namespace PacketParser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(App.Current.Services);

            //Task.Run(async () =>
            //{
            //    await Task.Delay(5000);
            //           LogGrid.
            //});
        }
    }
}