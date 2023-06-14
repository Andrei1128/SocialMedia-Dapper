

using System.ComponentModel.DataAnnotations;

namespace SocialMedia_Dapper.Models.DTOs
{
    public class UserUpdatedDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
