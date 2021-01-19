using Auth.Application.Services.Posts;
using Auth.Services.Application.Posts;
using Auth.Application.ServiceInterfaces;
using Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.WebApi
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }
    }
}
