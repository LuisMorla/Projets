using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaginaDeRegistros.Models;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Data;

namespace PaginaDeRegistros.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        static string conexion = "Data Source=SQL8001.site4now.net;Initial Catalog=db_a85cc3_conectando12;User Id=db_a85cc3_conectando12_admin;Password=Morla.123321";
       
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Registro(Usuario Ousuario)
        {
            bool registrado;
            string Mensaje;

            if (Ousuario.Contraseña == Ousuario.ConfirmarContraseña)
            {

                Ousuario.Contraseña = Convertir(Ousuario.Contraseña);

            }
            else {

                ViewData["Mensaje"] = "Las contraseñas no coinsiden";

                return View();

            }

            using (SqlConnection cn = new SqlConnection(conexion)) {
                SqlCommand cmd = new SqlCommand("SP_registrarusuario", cn);
                cmd.Parameters.AddWithValue("Correo", Ousuario.Correo);
                cmd.Parameters.AddWithValue("Contraseña", Ousuario.Contraseña);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction=ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();


                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);

                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = Mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else { 
                return View();
            }

        }
        [HttpPost]
        public ActionResult Login(Usuario Ousuario)
        {

            Ousuario.Contraseña = Convertir(Ousuario.Contraseña);

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_logeoValidacion", cn);
                cmd.Parameters.AddWithValue("Correo", Ousuario.Correo);
                cmd.Parameters.AddWithValue("Contraseña", Ousuario.Contraseña);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                Ousuario.ID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if (Ousuario.ID != 0)
            {

                Session["Usuario"] = Ousuario;
                return RedirectToAction("Register", "Contacto");

            }
            else {
                ViewData["Mensaje"] = "Usuario no encontrado";
            return View();
            }


        }

        public static string Convertir(string texto) { 
        
                StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create()) { 
                Encoding enc = Encoding.UTF8;
                byte[] result=hash.ComputeHash(enc.GetBytes(texto));

                foreach(byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        
        }
    }
}