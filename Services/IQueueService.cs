namespace TicketManagement.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync(string queueName, string message);
    }
}