using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Auth.Infrastructure.Mapper;
using Auth.WebApi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.WebApi
{
    public static class MapperConfig
    {
        public static IServiceCollection AddMapperConfiguration(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new ApiMapping());
                mc.AddProfile(new InfraMapping());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
