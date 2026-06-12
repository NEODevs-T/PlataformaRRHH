using System.ComponentModel.DataAnnotations;

namespace NeoRH.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Ingrese el usuario")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la contraseña")]
        public string Password { get; set; } = string.Empty;

        public string Proyecto { get; set; } = string.Empty;
    }
}