using Azure.Storage.Queues;

namespace TicketManagement.Services
{
    public class QueueService : IQueueService
    {
        private readonly string _connectionString;

        public QueueService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")
                ?? throw new InvalidOperationException("StorageConnection is not configured.");
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = new QueueClient(_connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            // Queue messages must be Base64-encoded by default
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            var base64Message = Convert.ToBase64String(bytes);

            await queueClient.SendMessageAsync(base64Message);
        }
    }
}