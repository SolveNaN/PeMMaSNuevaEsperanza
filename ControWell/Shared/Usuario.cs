using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }=string.Empty;
        public string Identificacion { get; set; } = string.Empty;//Cedula
        public string NombreUsuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }
}
