using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Auth.Application.Services.Posts;
using Auth.Domain.Posts;
using Auth.WebApi.DTOs.Posts;
using Auth.WebApi.Utilities.WebApi;
using Microsoft.AspNetCore.Authorization;

namespace Auth.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostsService _postsService;

        public PostsController(IMapper mapper, IPostsService postsService)
        {
            _mapper = mapper;
            _postsService = postsService;
        }


        #region Posts
        [Authorize]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetById(Guid postId)
        {
            try
            {
                var foundPost = await _postsService.GetPostFromIdAsync(postId);
                if (foundPost == null)
                { 
                    return NotFound();
                }
                return Ok(foundPost);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(ex);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(CreatePostDto post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newBlogId = await _postsService.SavePostAsync(_mapper.Map<Post>(post));
                    return Ok(newBlogId);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}
