using Avalonia.Controls;
using Avalonia.Media;
using PacketParser.ViewModels;
using System;
using System.Reflection;

namespace PacketParser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var version = (Assembly.GetExecutingAssembly().GetName().Version?.ToString()) ?? throw new FieldAccessException("Set version");
            DataContext = new MainViewModel(App.Current.Services)
            {
                Version = version
            };
            var color = ((SolidColorBrush)Background).Color;
            var colorBrush = new Color(130, color.R, color.G, color.B);
            Background = new SolidColorBrush(colorBrush);
        }
    }
}