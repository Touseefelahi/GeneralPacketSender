namespace PacketParser.ViewModels
{
    internal static class Services
    {
        internal static IServiceProvider ServiceProvider { get; private set; } = null!;
        internal static void SetProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
