using System.ComponentModel.DataAnnotations;

namespace HelpDesk.api.Models
{
    public class TicketsDTO
    {
        public Guid id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public enum TicketStatus
        {
            open = 0,
            pending = 1,
            onhold = 2,
            solve = 3,
            closed = 4
        };

        public TicketStatus status { get; set; } = TicketStatus.open;
        public bool isActive { get; set; } = true;
        public enum TicketPriority
        {
            low=-10,
            medium=0,
            high=10,
            urgent=20
        };

        public TicketPriority Priority { get; set; } = TicketPriority.low;

        [Required(ErrorMessage ="Please enter subject of Ticket.")]
        [MaxLength(32)]
        public string subject { get; set; }

        [Required]
        [MaxLength(200)]
        public string message { get; set; }
        public string requester { get; set; }
        public int rating { get; set; } = 0;
        public bool isSpam { get; set; } = false;
    }
}
