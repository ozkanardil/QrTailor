using QrTailor.Application.Features.Role.Models;
using QrTailor.Domain.Entities;
using AutoMapper;

namespace QrTailor.Application.Features.Role.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleEntity, RoleResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
