using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Angela_Hotel.Models;
using System.Data;

namespace Angela_Hotel.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _config;

        public UsuarioController(IConfiguration config)
        {
            _config = config;
        }

        private bool EsAdmin()
        {
            int? rol = HttpContext.Session.GetInt32("RolUsuario");
            return rol == 1;
        }

        public IActionResult Index()
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");

            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string query = @"SELECT u.ID_Usuario, u.Nombre, u.Correo, u.ID_Rol, r.NombreRol
                                 FROM Usuario u
                                 INNER JOIN Rol r ON u.ID_Rol = r.ID_Rol";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Usuario
                    {
                        ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                        Nombre = dr["Nombre"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Contrasena = "", // No se muestra por seguridad
                        ID_Rol = Convert.ToInt32(dr["ID_Rol"]),
                        NombreRol = dr["NombreRol"].ToString()
                    });
                }
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");

            ViewBag.Roles = ObtenerRoles();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Usuario (Nombre, Correo, Contraseña, ID_Rol) VALUES (@Nombre, @Correo, @Contrasena, @ID_Rol)", con);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    cmd.Parameters.AddWithValue("@ID_Rol", usuario.ID_Rol);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }

            ViewBag.Roles = ObtenerRoles();
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");

            Usuario usuario = new Usuario();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE ID_Usuario = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario.ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]);
                    usuario.Nombre = dr["Nombre"].ToString();
                    usuario.Correo = dr["Correo"].ToString();
                    usuario.Contrasena = dr["Contraseña"].ToString();
                    usuario.ID_Rol = Convert.ToInt32(dr["ID_Rol"]);
                }
            }

            ViewBag.Roles = ObtenerRoles();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        UPDATE Usuario 
                        SET Nombre = @Nombre, Correo = @Correo, Contraseña = @Contrasena, ID_Rol = @ID_Rol 
                        WHERE ID_Usuario = @ID", con);
                    cmd.Parameters.AddWithValue("@ID", usuario.ID_Usuario);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    cmd.Parameters.AddWithValue("@ID_Rol", usuario.ID_Rol);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Roles = ObtenerRoles();
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Usuario WHERE ID_Usuario = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        private List<Rol> ObtenerRoles()
        {
            List<Rol> roles = new List<Rol>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rol", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    roles.Add(new Rol
                    {
                        ID_Rol = Convert.ToInt32(dr["ID_Rol"]),
                        NombreRol = dr["NombreRol"].ToString()
                    });
                }
            }

            return roles;
        }
    }
}
