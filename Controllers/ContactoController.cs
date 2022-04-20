using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaginaDeRegistros.Permisos;
using System.Data.SqlClient;
using PaginaDeRegistros.Models;
using System.Data;

namespace PaginaDeRegistros.Controllers
{

    [ValidarSesion]
    public class ContactoController : Controller
    {

        static string conectar = "Data Source=SQL8001.site4now.net;Initial Catalog=db_a85cc3_conectando12;User Id=db_a85cc3_conectando12_admin;Password=Morla.123321";
        private static List<Registros> Lista = new List<Registros>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }


        public ActionResult Register()
        {

            Lista = new List<Registros>();


            using (SqlConnection conexion = new SqlConnection(conectar))
            {
                SqlCommand cmd = new SqlCommand("Select * from [Registro de Clientes]", conexion);
                cmd.CommandType = CommandType.Text;
                conexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                
                    while (dr.Read())
                    {

                        Registros contador = new Registros();


                        contador.ID = Convert.ToInt32(dr["ID"]);
                        contador.Cedula = dr["Cedula"].ToString();
                        contador.Nombre = dr["Nombre"].ToString();
                        contador.Apellidos = dr["Apellidos"].ToString();

                        Lista.Add(contador);

                    }

                }

            }

            return View(Lista);
        }

        public ActionResult CrearNuevo() { 
            return View();  
        }

        [HttpPost]
        public ActionResult CrearNuevo(Registros Oregistro)
        {
            using (SqlConnection conectame = new SqlConnection(conectar))
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR", conectame);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Cedula", Oregistro.Cedula);
                cmd.Parameters.AddWithValue("Nombre", Oregistro.Nombre);
                cmd.Parameters.AddWithValue("Apellidos", Oregistro.Apellidos);
                conectame.Open();
                cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Register", "Contacto");
        }

        [HttpGet]
        public ActionResult Eliminar(int? ID) {

            if (ID==null) 
                return RedirectToAction("Register", "Contacto");

                Registros Oregistros = Lista.Where(x => x.ID == ID).FirstOrDefault();
            
            return View(Oregistros);

        }
        [HttpPost]
        public ActionResult Eliminar(string ID)
        {

            using (SqlConnection Oconection = new SqlConnection(conectar)) { 
            
                SqlCommand cmd = new SqlCommand("sp_Eliminar",Oconection);
                cmd.Parameters.AddWithValue("ID", ID);  
                cmd.CommandType=CommandType.StoredProcedure;
                Oconection.Open();
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("Register", "Contacto");

        }

    }
}