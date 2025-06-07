using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdHabitacion { get; set; }

    public int? IdFechaInicio { get; set; }

    public int? IdFechaFin { get; set; }

    public string? Estado { get; set; }

    public virtual Fecha? IdFechaFinNavigation { get; set; }

    public virtual Fecha? IdFechaInicioNavigation { get; set; }

    public virtual Habitacion? IdHabitacionNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
