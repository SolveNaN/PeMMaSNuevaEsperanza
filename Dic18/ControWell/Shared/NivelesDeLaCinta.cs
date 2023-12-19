using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class NivelesDeLaCinta
    {
        public int Id { get; set; }
        public double? Nominal { get; set; }
        public double? Correccion { get; set; }
        public Cinta? Cita { get; set; }
        public int CintaId { get; set; }

    }
}
