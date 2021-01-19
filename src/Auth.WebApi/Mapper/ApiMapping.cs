using Auth.Domain.Posts;
using Auth.WebApi.DTOs.Posts;
using AutoMapper;

namespace Auth.WebApi.Mapper
{
    public class ApiMapping : Profile
    {
        public ApiMapping()
        {
            CreateMap<CreatePostDto, Post>();
        }
    }
}
