using AutoMapper;
using Planner.API.DTOs;
using Planner.DAL.Models;

namespace Planner.API.AutoMapper
{
    public class ProjectMapperProfile : Profile
    {
        public ProjectMapperProfile()
        {
            CreateMap<Project, ProjectDTO>().ReverseMap();               
        }
    }
}
