using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class AuthenticationRequest
    {
        [Required, RegularExpression("^[7]{1}\\d{10}$")]
        public string Phone { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
