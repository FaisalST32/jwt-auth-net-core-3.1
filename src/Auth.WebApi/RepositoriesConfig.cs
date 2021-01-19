using Auth.Application.Repositories;
using Auth.Infrastructure.Repositories;
using Auth.Application.Repositories;
using Auth.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.WebApi
{
    public static class RepositoriesConfig
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPostsRepository, PostsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            return services;
        }
    }
}
