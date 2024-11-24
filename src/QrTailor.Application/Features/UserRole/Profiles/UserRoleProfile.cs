using QrTailor.Application.Features.UserRole.Command;
using QrTailor.Application.Features.UserRole.Models;
using QrTailor.Domain.Entities;
using AutoMapper;

namespace QrTailor.Application.Features.UserRole.Profiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleVEntity, UserRoleResponse>().ReverseMap();
            CreateMap<CreateUserRoleCommand, UserRoleEntity>().ReverseMap();
        }
    }
}
