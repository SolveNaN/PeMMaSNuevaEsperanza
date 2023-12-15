using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Sello
    {
        public int Id { get;set; }
        public string IndiceSello { get; set; } = string.Empty;
        public int? NumeroSello { get; set; }
        public string Lote { get; set; } = string.Empty;
        public int Estado { get; set; }
        public int EnUso { get; set; } = 0;
        public DateTime CreatedAt { get; set; }=DateTime.Now;
    }
}
