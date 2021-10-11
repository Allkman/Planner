using AutoMapper;
using Planner.API.DTOs;
using Planner.DAL.Models;

namespace Planner.API.AutoMapper
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<TicketDTO, Ticket>().ReverseMap();
        }
    }
}
