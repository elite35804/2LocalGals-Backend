using System.ComponentModel.DataAnnotations;

namespace TwoLocalGals.DTO
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}