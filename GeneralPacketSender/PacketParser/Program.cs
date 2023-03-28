using Avalonia;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PacketParser
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            List<string> dlls = new() { "libHarfBuzzSharp.dll", "libSkiaSharp.dll" };
            //ExtractDlls(dlls);
            return AppBuilder.Configure<App>()
                        .UsePlatformDetect()
                        .WithIcons(container => container.Register<FontAwesomeIconProvider>())
                        .LogToTrace();
        }

        private static void ExtractDlls(List<string> dllNames)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            var executeableName = executingAssembly.GetName().Name;
            string tempName = Path.GetTempFileName();
            string executableBasePath = Path.Combine(Path.GetDirectoryName(tempName), executeableName);
            Directory.CreateDirectory(executableBasePath);
            File.Delete(tempName);
            foreach (string dllName in dllNames)
            {
                string resName = executeableName + ".Native." + dllName;
                var executablePath = Path.Combine(executableBasePath, dllName);
                using Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName);
                using Stream file = File.Create(executablePath);
                resource?.CopyTo(file);
            }
        }
    }
}