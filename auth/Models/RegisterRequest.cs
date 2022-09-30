using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class RegisterRequest
    {
        [Required, MaxLength(250)]
        public string FIO { get; set; }

        [Required, RegularExpression("^[7]{1}\\d{10}$"), MaxLength(11)]
        public string Phone { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; }

        [Required, MaxLength(20), DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password"), DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
