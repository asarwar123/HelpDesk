namespace HelpDesk.api.Models
{
    public class TicketCreationDTO
    {
        public string CreatedBy { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string requester { get; set; }
    }
}
