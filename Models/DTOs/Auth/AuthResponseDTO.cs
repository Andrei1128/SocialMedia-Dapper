using System.ComponentModel.DataAnnotations;

namespace SocialMedia_Dapper.Models.DTOs
{
    public class AuthResponseDTO
    {
        public UserDTO User { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
