using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Live.Avalonia;
using PacketParser.ViewModels;
using System;
using Microsoft.Extensions.DependencyInjection;
using PacketParser.Services;
using PacketSender.Core;

namespace PacketParser
{
    public partial class App : Application, ILiveView
    {
        public IServiceProvider Services { get; private set; } = null!;
        public new static App Current => (App)Application.Current!;
        public object CreateView(Window window)
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            Services = ConfigureServices();
            AvaloniaXamlLoader.Load(this);
        }
        internal static Window Window { get; private set; } = null!;
        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IDialogService, DialogService>();
            services.AddSingleton<ILogger, LoggerService>();
            return services.BuildServiceProvider();
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
                Window = desktop.MainWindow;
            }

            ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).ShutdownRequested += delegate (object? sender, ShutdownRequestedEventArgs e)
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (desktop.MainWindow?.DataContext is MainViewModel viewModel)
                    {
                        viewModel.SaveXmlCommand.Execute(EventArgs.Empty);
                    }
                }
            };
            base.OnFrameworkInitializationCompleted();
        }
    }
}