using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class Habitacion
{
    public int IdHabitacion { get; set; }

    public string? NumeroHabitacion { get; set; }

    public int? IdTipoHabitacion { get; set; }

    public int? IdSede { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Disponibilidad> Disponibilidads { get; set; } = new List<Disponibilidad>();

    public virtual Sede? IdSedeNavigation { get; set; }

    public virtual TipoHabitacion? IdTipoHabitacionNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
