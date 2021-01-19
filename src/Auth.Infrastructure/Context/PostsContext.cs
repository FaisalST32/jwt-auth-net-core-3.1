using Microsoft.EntityFrameworkCore;
using Auth.Application.Repositories;
using Auth.Infrastructure.Entities;
using System;
using Auth.Infrastructure.Entities;

namespace Auth.Infrastructure.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
