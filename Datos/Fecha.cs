using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class Fecha
{
    public int IdFecha { get; set; }

    public DateOnly? Fecha1 { get; set; }

    public virtual ICollection<Disponibilidad> Disponibilidads { get; set; } = new List<Disponibilidad>();

    public virtual ICollection<Reserva> ReservaIdFechaFinNavigations { get; set; } = new List<Reserva>();

    public virtual ICollection<Reserva> ReservaIdFechaInicioNavigations { get; set; } = new List<Reserva>();
}
