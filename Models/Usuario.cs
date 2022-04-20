using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaginaDeRegistros.Models
{
    public class Usuario
    {
        public int ID{ get; set; }
        public string Correo { get; set; }

        public string Contraseña { get; set; }

        public string ConfirmarContraseña { get; set; }


    }
}