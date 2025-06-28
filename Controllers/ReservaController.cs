using Angela_Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace Angela_Hotel.Controllers
{
    public class ReservaController : Controller
    {
        private readonly IConfiguration _config;

        public ReservaController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            int? idUsuario = HttpContext.Session.GetInt32("ID_Usuario");
            int? rol = HttpContext.Session.GetInt32("RolUsuario");

            if (idUsuario == null || rol == null)
                return RedirectToAction("Index", "Login");

            List<Reserva> lista = new List<Reserva>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                string query;

                if (rol == 1)
                {
                    query = @"
                SELECT r.ID_Reserva, r.ID_Usuario, u.Nombre AS NombreUsuario,
                       r.ID_Habitacion, h.Numero_Habitacion AS NumeroHabitacion,
                       r.ID_FechaInicio, f1.Fecha AS FechaInicioTexto,
                       r.ID_FechaFin, f2.Fecha AS FechaFinTexto,
                       r.Estado
                FROM Reserva r
                INNER JOIN Usuario u ON r.ID_Usuario = u.ID_Usuario
                INNER JOIN Habitacion h ON r.ID_Habitacion = h.ID_Habitacion
                INNER JOIN Fecha f1 ON r.ID_FechaInicio = f1.ID_Fecha
                INNER JOIN Fecha f2 ON r.ID_FechaFin = f2.ID_Fecha";
                }
                else 
                {
                    query = @"
                SELECT r.ID_Reserva, r.ID_Usuario, u.Nombre AS NombreUsuario,
                       r.ID_Habitacion, h.Numero_Habitacion AS NumeroHabitacion,
                       r.ID_FechaInicio, f1.Fecha AS FechaInicioTexto,
                       r.ID_FechaFin, f2.Fecha AS FechaFinTexto,
                       r.Estado
                FROM Reserva r
                INNER JOIN Usuario u ON r.ID_Usuario = u.ID_Usuario
                INNER JOIN Habitacion h ON r.ID_Habitacion = h.ID_Habitacion
                INNER JOIN Fecha f1 ON r.ID_FechaInicio = f1.ID_Fecha
                INNER JOIN Fecha f2 ON r.ID_FechaFin = f2.ID_Fecha
                WHERE r.ID_Usuario = @ID_Usuario";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                if (rol != 1)
                    cmd.Parameters.AddWithValue("@ID_Usuario", idUsuario);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Reserva
                    {
                        ID_Reserva = Convert.ToInt32(dr["ID_Reserva"]),
                        ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        ID_Habitacion = Convert.ToInt32(dr["ID_Habitacion"]),
                        NumeroHabitacion = dr["NumeroHabitacion"].ToString(),
                        ID_FechaInicio = Convert.ToInt32(dr["ID_FechaInicio"]),
                        FechaInicioTexto = Convert.ToDateTime(dr["FechaInicioTexto"]).ToShortDateString(),
                        ID_FechaFin = Convert.ToInt32(dr["ID_FechaFin"]),
                        FechaFinTexto = Convert.ToDateTime(dr["FechaFinTexto"]).ToShortDateString(),
                        Estado = dr["Estado"].ToString()
                    });
                }
            }

            return View(lista);
        }



        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Habitaciones = ObtenerHabitaciones();
            ViewBag.Fechas = ObtenerFechas();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Reserva model)
        {
            int? idUsuario = HttpContext.Session.GetInt32("ID_Usuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Reserva (ID_Usuario, ID_Habitacion, ID_FechaInicio, ID_FechaFin, Estado)
                        VALUES (@ID_Usuario, @ID_Habitacion, @ID_FechaInicio, @ID_FechaFin, 'Activa')", con);

                    cmd.Parameters.AddWithValue("@ID_Usuario", idUsuario.Value);
                    cmd.Parameters.AddWithValue("@ID_Habitacion", model.ID_Habitacion);
                    cmd.Parameters.AddWithValue("@ID_FechaInicio", model.ID_FechaInicio);
                    cmd.Parameters.AddWithValue("@ID_FechaFin", model.ID_FechaFin);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Habitaciones = ObtenerHabitaciones();
            ViewBag.Fechas = ObtenerFechas();
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            int? idUsuarioSesion = HttpContext.Session.GetInt32("ID_Usuario");
            if (idUsuarioSesion == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Reserva reserva = null;

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Reserva WHERE ID_Reserva = @ID AND ID_Usuario = @ID_Usuario", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@ID_Usuario", idUsuarioSesion);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reserva = new Reserva
                    {
                        ID_Reserva = Convert.ToInt32(dr["ID_Reserva"]),
                        ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                        ID_Habitacion = Convert.ToInt32(dr["ID_Habitacion"]),
                        ID_FechaInicio = Convert.ToInt32(dr["ID_FechaInicio"]),
                        ID_FechaFin = Convert.ToInt32(dr["ID_FechaFin"])
                    };
                }
            }

            if (reserva == null)
            {
                return NotFound(); 
            }

            ViewBag.Habitaciones = ObtenerHabitaciones();
            ViewBag.Fechas = ObtenerFechas();
            return View(reserva);
        }

        [HttpPost]
        public IActionResult Edit(Reserva reserva)
        {
            int? idUsuarioSesion = HttpContext.Session.GetInt32("ID_Usuario");
            if (idUsuarioSesion == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();

                    
                    SqlCommand validarCmd = new SqlCommand("SELECT COUNT(*) FROM Reserva WHERE ID_Reserva = @ID AND ID_Usuario = @ID_Usuario", con);
                    validarCmd.Parameters.AddWithValue("@ID", reserva.ID_Reserva);
                    validarCmd.Parameters.AddWithValue("@ID_Usuario", idUsuarioSesion);
                    int existe = (int)validarCmd.ExecuteScalar();

                    if (existe == 0)
                    {
                        return Unauthorized(); 
                    }

                   
                    SqlCommand cmd = new SqlCommand(@"UPDATE Reserva 
                SET ID_Habitacion = @ID_Habitacion, 
                    ID_FechaInicio = @ID_FechaInicio, 
                    ID_FechaFin = @ID_FechaFin 
                WHERE ID_Reserva = @ID_Reserva AND ID_Usuario = @ID_Usuario", con);

                    cmd.Parameters.AddWithValue("@ID_Reserva", reserva.ID_Reserva);
                    cmd.Parameters.AddWithValue("@ID_Usuario", idUsuarioSesion);
                    cmd.Parameters.AddWithValue("@ID_Habitacion", reserva.ID_Habitacion);
                    cmd.Parameters.AddWithValue("@ID_FechaInicio", reserva.ID_FechaInicio);
                    cmd.Parameters.AddWithValue("@ID_FechaFin", reserva.ID_FechaFin);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Habitaciones = ObtenerHabitaciones();
            ViewBag.Fechas = ObtenerFechas();
            return View(reserva);
        }




        private List<Habitacion> ObtenerHabitaciones()
        {
            List<Habitacion> habitaciones = new List<Habitacion>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID_Habitacion, Numero_Habitacion FROM Habitacion", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    habitaciones.Add(new Habitacion
                    {
                        ID_Habitacion = Convert.ToInt32(dr["ID_Habitacion"]),
                        NumeroHabitacion = dr["Numero_Habitacion"].ToString()
                    });
                }
            }

            return habitaciones;
        }

        private List<Fecha> ObtenerFechas()
        {
            List<Fecha> fechas = new List<Fecha>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID_Fecha, Fecha FROM Fecha", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    fechas.Add(new Fecha
                    {
                        ID_Fecha = Convert.ToInt32(dr["ID_Fecha"]),
                        FechaTexto = Convert.ToDateTime(dr["Fecha"]).ToShortDateString()
                    });
                }
            }

            return fechas;
        }
        private List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID_Usuario, Nombre FROM Usuario", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                        Nombre = dr["Nombre"].ToString()
                    });
                }
            }

            return usuarios;
        }


        public class Habitacion
        {
            public int ID_Habitacion { get; set; }
            public string NumeroHabitacion { get; set; }
        }

        public class Fecha
        {
            public int ID_Fecha { get; set; }
            public string FechaTexto { get; set; }
        }
    }
    

}
