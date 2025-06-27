using Microsoft.AspNetCore.Mvc;
using Angela_Hotel.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Angela_Hotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
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
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("LoginSP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", model.Correo);
                    cmd.Parameters.AddWithValue("@Contraseña", model.Contraseña);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int idUsuario = Convert.ToInt32(reader["ID_Usuario"]);
                        string nombre = reader["Nombre"].ToString();
                        int rol = reader["ID_Rol"] != DBNull.Value ? Convert.ToInt32(reader["ID_Rol"]) : 0;

                        HttpContext.Session.SetInt32("ID_Usuario", idUsuario); // 👈 ESTA ES LA CLAVE
                        HttpContext.Session.SetString("NombreUsuario", nombre);
                        HttpContext.Session.SetInt32("RolUsuario", rol);

                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ViewBag.Mensaje = "Correo o contraseña incorrectos";
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Usuario WHERE Correo = @Correo", con);
                    checkCmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        ViewBag.Mensaje = "Este correo ya está registrado.";
                        return View(usuario);
                    }

                    SqlCommand insertCmd = new SqlCommand("INSERT INTO Usuario (Nombre, Correo, Contraseña, ID_Rol) VALUES (@Nombre, @Correo, @Contrasena, @Rol)", con);
                    insertCmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    insertCmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    insertCmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    insertCmd.Parameters.AddWithValue("@Rol", 2); // Rol 2 = Cliente
                    insertCmd.ExecuteNonQuery();

                    return RedirectToAction("Index", "Login");
                }
            }
            return View(usuario);
        }
    }
}
