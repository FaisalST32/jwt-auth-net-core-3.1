using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.WebApi.DTOs.Posts
{
    public class CreatePostDto
    {
        [Required]
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Content { get; set; }
    }
}
