using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace PacketParser.Views
{
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
            var color = ((SolidColorBrush)Background).Color;
            var colorBrush = new Color(130, color.R, color.G, color.B);
            Background = new SolidColorBrush(colorBrush);
        }

        private void OkPressed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}