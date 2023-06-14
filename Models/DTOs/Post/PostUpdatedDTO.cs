using System.ComponentModel.DataAnnotations;

namespace SocialMedia_Dapper.Models.DTOs
{
    public class PostUpdatedDTO
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
