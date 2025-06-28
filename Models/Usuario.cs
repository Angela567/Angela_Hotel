using System.ComponentModel.DataAnnotations;

namespace Angela_Hotel.Models
{
    public class Usuario
    {
        public int ID_Usuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public int ID_Rol { get; set; }

        
        public string? NombreRol { get; set; }
    }
}
