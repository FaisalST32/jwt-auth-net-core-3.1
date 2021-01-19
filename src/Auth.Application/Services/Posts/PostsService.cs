using Auth.Application.Repositories;
using Auth.Application.Services.Posts;
using Auth.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services.Application.Posts
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepo;

        public PostsService(IPostsRepository postsRepo)
        {
            _postsRepo = postsRepo;
        }
        public async Task<Guid> SavePostAsync(Post post)
        {
            var postId = await _postsRepo.SavePostAsync(post);
            return postId;
        }

        public async Task<Post> GetPostFromIdAsync(Guid postId)
        {
            return await _postsRepo.GetPostFromIdAsync(postId);
        }
    }
}
