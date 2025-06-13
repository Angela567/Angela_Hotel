using Microsoft.AspNetCore.Mvc;
using Angela_Hotel.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http; // para usar sesiones

namespace Angela_Hotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("LoginSP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", model.Correo);
                    cmd.Parameters.AddWithValue("@Contraseña", model.Contraseña);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Usuario encontrado
                        string nombre = reader["Nombre"].ToString();
                        string correo = reader["Correo"].ToString();

                        // Guardar en sesión
                        HttpContext.Session.SetString("NombreUsuario", nombre);
                        HttpContext.Session.SetString("CorreoUsuario", correo);

                        // Redirigir al Home
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Usuario no encontrado
                        ViewBag.Mensaje = "Correo o contraseña incorrectos.";
                        return View(model);
                    }
                }
            }

            return View(model);
        }
    }
}
