using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class Sede
{
    public int IdSede { get; set; }

    public string? Nombre { get; set; }

    public string? Ciudad { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
