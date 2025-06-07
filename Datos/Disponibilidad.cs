using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class Disponibilidad
{
    public int IdDisponibilidad { get; set; }

    public int? IdHabitacion { get; set; }

    public int? IdFecha { get; set; }

    public bool? Disponible { get; set; }

    public virtual Fecha? IdFechaNavigation { get; set; }

    public virtual Habitacion? IdHabitacionNavigation { get; set; }
}
