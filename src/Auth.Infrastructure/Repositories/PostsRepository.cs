using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Auth.Application.Repositories;
using Auth.Common.Data;
using Auth.Domain.Posts;
using System;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PostsRepository(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }
        public async Task<Guid> SavePostAsync(Post post)
        {
            var postToAdd = _mapper.Map<Entities.Post>(post);
            if(postToAdd.UniqueId == Guid.Empty)
                postToAdd.UniqueId = Guid.NewGuid();
            _uow.Context.Set<Entities.Post>().Add(postToAdd);
            await _uow.SaveChangesAsync();
            return postToAdd.UniqueId;
        }

        public async Task<Post> GetPostFromIdAsync(Guid postId)
        {
            var foundPost = await _uow.Context.Set<Entities.Post>().SingleOrDefaultAsync(post => post.UniqueId == postId);
            return _mapper.Map<Post>(foundPost);
        }
    }
}
