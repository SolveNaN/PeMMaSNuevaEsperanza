using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Ruta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;        
        public string Estado { get; set; } = string.Empty;
        public int VigenciaHoras { get; set; }
    }
}
