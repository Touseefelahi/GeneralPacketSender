using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PacketParser.Views
{
    public partial class EditCommandView : Window
    {
        public EditCommandView()
        {
            InitializeComponent();
        }

        public bool SaveRequested { get; private set; }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            SaveRequested = true;
            this.Close();
        }

        private void Canceled(object sender, RoutedEventArgs e)
        {
            SaveRequested = false;
            this.Close();
        }
    }
}