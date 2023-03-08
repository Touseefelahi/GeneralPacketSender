using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PacketParser.Views
{
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private void OkPressed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
