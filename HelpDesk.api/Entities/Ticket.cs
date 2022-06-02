using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.api.Entities
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity )]
        public int id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = String.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        
        public int status { get; set; } = 0;
        public bool isActive { get; set; } = true;
        
        public int Priority { get; set; } = -10;

        [MaxLength(32)]
        public string subject { get; set; } = String.Empty;

        [Required]
        [MaxLength(200)]
        public string message { get; set; } = String.Empty;
        public string requester { get; set; } = String.Empty;
        public int rating { get; set; } = 0;
        public bool isSpam { get; set; } = false;

        public static implicit operator Task<object>(Ticket? v)
        {
            throw new NotImplementedException();
        }
    }
}
