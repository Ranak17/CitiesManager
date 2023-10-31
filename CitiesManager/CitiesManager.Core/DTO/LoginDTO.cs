using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email Can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in proper email address format")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password Can't be blank")]
        public string Password { get; set; } = string.Empty;
    }
}
