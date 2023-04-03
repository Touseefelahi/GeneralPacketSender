using Avalonia.Controls;
using PacketParser.ViewModels;
using PacketParser.Views;
using PacketSender.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PacketParser.Services
{
    internal class DialogService : IDialogService
    {
        public async Task<bool> EditCommand(object senderViewModel)
        {
            if (senderViewModel is SenderViewModel sender)
            {
                var editWindow = new EditCommandView()
                {
                    DataContext = sender,
                };
                await editWindow.ShowDialog(App.Window);
                return true;
            }
            return false;
        }

        public async Task<string> OpenFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Open File",
                Filters = new List<FileDialogFilter>
                          {
                              new FileDialogFilter { Name = "Xml Files", Extensions = new List<string> { "xml" } }
                          }
            };
            string[] result = await dialog.ShowAsync(App.Window);
            if (result != null && result.Length > 0)
            {
                string filePath = result[0];
                return filePath;
            }
            return null;
        }

        public async Task<bool> ShowDialog(string message, string caption)
        {
            var dialog = new Dialog
            {
                Title = caption
            };
            dialog.Message.Text = message;
            await dialog.ShowDialog(App.Window);
            return true;
        }
    }
}