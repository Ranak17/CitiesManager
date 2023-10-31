using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person Name Can't be blank")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Can't be blank")]
        [Remote(action: "IsEmailAlreadyRegister", controller:"Account", ErrorMessage ="Email is Already in use")]
        public string Email {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number Can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number Contains Digit only")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Can't be blank")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage ="Confirm Password Can't be blank")]
        [Compare("Password",ErrorMessage ="Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
