using AutoMapper;
using HelpDesk.api.Entities;
using HelpDesk.api.Models;

namespace HelpDesk.api.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketsDTO>();
            CreateMap<TicketCreationDTO, Ticket>();
        }
    }
}
