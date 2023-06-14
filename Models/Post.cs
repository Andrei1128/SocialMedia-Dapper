﻿
namespace SocialMedia_Dapper.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public Group Group { get; set; }

        public ICollection<User> Likes { get; set; }
        public ICollection<Post> Comments { get; set; }

        public string ImageURL { get; set; }
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
