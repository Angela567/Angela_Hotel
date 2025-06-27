using System;
using System.Collections.Generic;

namespace Angela_Hotel.Datos;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public int? IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
