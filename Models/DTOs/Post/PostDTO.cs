using System.ComponentModel.DataAnnotations;

namespace SocialMedia_Dapper.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        [Required]
        public UserDTO Author { get; set; }
        public GroupDTO Group { get; set; }

        public ICollection<UserDTO> Likes { get; set; }
        public ICollection<PostDTO> Comments { get; set; }

        public string ImageURL { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
