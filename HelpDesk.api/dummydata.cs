using HelpDesk.api.Models;

namespace HelpDesk.api
{
    public static class dummydata
    {
        public static List<TicketsDTO> Tickets { get; set; } = new List<TicketsDTO>();

        public static void FillData()
        {
            Tickets = new List<TicketsDTO>()
            {
                new TicketsDTO()
                {
                    id=Guid.NewGuid(),
                    CreatedAt=DateTime.Now,
                    CreatedBy="Azeem Sarwar",
                    subject="Email not working",
                    message="I am unable to login on my newly creatly emial address",
                    requester="azeem.sarwar@gmail.com",
                    status=TicketsDTO.TicketStatus.open
                },
                new TicketsDTO()
                {
                    id=Guid.NewGuid(),
                    CreatedBy="Nadeem Sarwar",
                    subject="Email not recieved",
                    message="I am unable to send/recieve on my newly creatly emial address",
                    requester="nadeem.sarwar@gmail.com"
                },
                new TicketsDTO()
                {
                    id=Guid.NewGuid(),
                    CreatedBy="Naeem Sarwar",
                    subject="Email not recieved",
                    message="I am unable to send/recieve on my newly creatly emial address",
                    requester="naeem.sarwar@gmail.com"
                }
            };
        }

        public static void InsertTicket(TicketsDTO newTicket)
        {
            Tickets.Add(newTicket);
        }

        public static bool UpdateTicket(Guid id, TicketsDTO updatedTicket)
        {
            TicketsDTO ?orignalTicket = Tickets.Find(t => t.id == id);

            if(orignalTicket != null)
            {
                orignalTicket.CreatedBy = updatedTicket.CreatedBy;
                orignalTicket.subject = updatedTicket.subject;
                orignalTicket.message = updatedTicket.message;
                orignalTicket.requester = updatedTicket.requester;

                return true;
            }

            return false;
        }

        public static bool DeleteTicket(Guid id)
        {
            TicketsDTO? orignalTicket = Tickets.Find(t => t.id == id);

            if (orignalTicket != null)
            {
                Tickets.Remove(orignalTicket);

                return true;
            }

            return false;
        }
    }
}
