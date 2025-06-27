using System.ComponentModel.DataAnnotations;

namespace Angela_Hotel.Models
{
    public class Reserva
    {
        public int ID_Reserva { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public int ID_Usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una habitación")]
        public int ID_Habitacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la fecha de inicio")]
        public int ID_FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la fecha de fin")]
        public int ID_FechaFin { get; set; }

        public string? Estado { get; set; }

        // Datos extras para mostrar en la vista
        public string? NombreUsuario { get; set; }
        public string? NumeroHabitacion { get; set; }
        public string? FechaInicioTexto { get; set; }
        public string? FechaFinTexto { get; set; }
    }
}

