using System.ComponentModel.DataAnnotations;

namespace Angela_Hotel.Models

{

    public class LoginViewModel

    {

        [Required]

        public string Correo { get; set; }

        [Required]

        public string Contraseña { get; set; }

    }

}

