using System.ComponentModel.DataAnnotations;

namespace SocialMedia_Dapper.Models.DTOs
{
    public class GroupUpdatedDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
        public string ImageURL { get; set; }
    }
}
