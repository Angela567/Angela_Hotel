using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class TipoHabitacion
{
    public int IdTipoHabitacion { get; set; }

    public string? NombreTipo { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
