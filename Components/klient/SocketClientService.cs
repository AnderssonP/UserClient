using System.Collections.Concurrent;
namespace UserClient.Components.klient
{
    public class SocketClientService : BackgroundService
    {
        private readonly ILogger<SocketClientService> _logger;
        private readonly BlockingCollection<(string Username, string Password, string site)> _dataQueue;

        public SocketClientService(ILogger<SocketClientService> logger)
        {
            _logger = logger;
            _dataQueue = new BlockingCollection<(string Username, string Password, string Site)>();
        }

        public void SendUserToServer(string username, string password, string site)
        {
            _dataQueue.Add((username, password, site));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await SocketConnection.SocketClientAsync();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var data = _dataQueue.Take(stoppingToken);
                    await SocketConnection.SendToServer(data.Username, data.Password, data.site);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while running the socket client.");
            }
            finally
            {
                _dataQueue.CompleteAdding();
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _dataQueue.CompleteAdding();
            await base.StopAsync(stoppingToken);
        }
    }

}
