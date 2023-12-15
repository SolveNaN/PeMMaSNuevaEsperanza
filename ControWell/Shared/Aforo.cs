using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Aforo
    {
        public int Id { get; set; }
        public Tanque? Tanque { get; set; }
        public int TanqueId { get; set; }
        public double Nivel { get; set; }
        public double Volumen { get; set; }
        public double Incremento { get; set; }
    }
}
