using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class CalibracionTermometro
    {
        public int Id { get; set; }
        public double? Nominal { get; set; }
        public double? Correccion { get; set; }
        public Termometro? Termometro { get; set; }
        public int TermometroId { get; set; }
    }
}
