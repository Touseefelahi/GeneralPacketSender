using Avalonia.Controls;
using PacketParser.ViewModels;
using System.Reflection;
using System;

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
        }
    }
}