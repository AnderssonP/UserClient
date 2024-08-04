namespace UserClient.Components.klient
{
    public static class SocketClientHandler
    {
        private static SocketClientService _socketClientService;

        public static void Initialize(SocketClientService socketClientService)
        {
            _socketClientService = socketClientService ?? throw new ArgumentNullException(nameof(socketClientService));
            Console.WriteLine("SocketClientService has been initialized.");
        }

        public static void SendUserToServer(string username, string password, string site)
        {
            if (_socketClientService == null)
            {
                throw new InvalidOperationException("SocketClientService has not been initialized.");
            }

            _socketClientService.SendUserToServer(username, password, site);
        }
    }

}
