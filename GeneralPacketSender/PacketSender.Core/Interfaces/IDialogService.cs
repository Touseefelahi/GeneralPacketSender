namespace PacketSender.Core
{
    public interface IDialogService
    {
        public Task<bool> ShowDialog(string message, string caption);

        public Task<string> OpenFileDialog();

        public Task<bool> EditCommand(object senderViewModel);
    }
}