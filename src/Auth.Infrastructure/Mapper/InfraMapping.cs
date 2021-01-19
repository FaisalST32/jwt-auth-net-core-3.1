using Auth.Domain.Posts;
using Auth.Domain.User;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Infrastructure.Mapper
{
    public class InfraMapping : Profile
    {
        public InfraMapping()
        {
            //Posts

            CreateMap<Post, Entities.Post>().ReverseMap();
            //Users

            CreateMap<User, Entities.User>()
                .ForMember(dest => dest.UserProfile, opt => opt.MapFrom((src, dest, i, context) =>
                {
                    return src.UserProfile != null ? context.Mapper.Map<Entities.UserProfile>(src.UserProfile) : null;
                }))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(role => new Entities.UserRole() { RoleId = role.Id, UserId = src.Id })));

            CreateMap<Entities.User, User>()
                .ForMember(dest => dest.UserProfile, opt => opt.MapFrom((src, dest, i, context) =>
                {
                    return src.UserProfile != null ? context.Mapper.Map<UserProfile>(src.UserProfile) : null;
                }))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(role => new Role() { Id = role.RoleId, Name = role.Role.Name })));

            CreateMap<UserProfile, Entities.UserProfile>().ReverseMap();
        }
    }
}
