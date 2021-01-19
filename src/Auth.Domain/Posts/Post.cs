using System;
using System.Collections.Generic;

namespace Auth.Domain.Posts
{
    public class Post
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Content { get; set; }
        public DateTime? DatePublished { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
    }
}
