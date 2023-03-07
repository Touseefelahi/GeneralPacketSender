using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Live.Avalonia;
using PacketParser.ViewModels;
using System;

namespace PacketParser
{
    public partial class App : Application, ILiveView
    {
        public object CreateView(Window window)
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
//#if DEBUG
//                desktop.MainWindow = new LiveViewHost(this, System.Console.WriteLine);
//                ((LiveViewHost)desktop.MainWindow).StartWatchingSourceFilesForHotReloading();
//#else
//                desktop.MainWindow = new MainWindow();
//#endif
                desktop.MainWindow = new MainWindow();
            }

            //((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).ShutdownRequested += delegate (object? sender, ShutdownRequestedEventArgs e)
            //{
            //    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            //    {
            //        if (desktop.MainWindow?.DataContext is MainViewModel viewModel)
            //        {
            //            viewModel.SaveXmlCommand.Execute(EventArgs.Empty);
            //        }
            //    }
            //};
            base.OnFrameworkInitializationCompleted();
        }
    }
}