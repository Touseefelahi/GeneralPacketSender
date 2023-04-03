using Avalonia;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

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

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            List<string> dlls = new() { "libHarfBuzzSharp.dll", "libSkiaSharp.dll", "av_libglesv2.dll" };
            ExtractDlls(dlls);
            return AppBuilder.Configure<App>()
                        .UsePlatformDetect()
                        .WithIcons(container => container.Register<FontAwesomeIconProvider>())
                        .LogToTrace();
        }

        private static void ExtractDlls(List<string> dllNames, string folderName = "")
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            var executeableName = executingAssembly.GetName().Name;
            string tempName = Path.GetTempFileName();
            var res = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            if (res.Length > 1)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    var splittedExeName = res[i].Split(".");
                    if (splittedExeName.Length > 1)
                    {
                        executeableName = splittedExeName[0];
                        break;
                    }
                }
            }
            string executableBasePath = Path.Combine(Path.GetDirectoryName(tempName), executeableName);
            Directory.CreateDirectory(executableBasePath);
            SetDllDirectory(executableBasePath);
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