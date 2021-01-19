using Auth.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services.Posts
{
    public interface IPostsService
    {
        Task<Guid> SavePostAsync(Post post);
        Task<Post> GetPostFromIdAsync(Guid postId);
    }
}
