namespace TicketManagement.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Open";

        public string Priority { get; set; } = "Medium";

        public int CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? AssignedToUserId { get; set; }
    }
}