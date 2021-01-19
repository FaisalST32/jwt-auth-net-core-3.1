using Auth.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Repositories
{
    public interface IPostsRepository
    {
        Task<Guid> SavePostAsync(Post post);
        Task<Post> GetPostFromIdAsync(Guid postId);
    }
}
