using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class TestPozo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }=string.Empty;
        public DateTime Fecha { get; set; }=DateTime.Now;        
        public int? TiempoProyectado { get; set; }
        public Tanque? Tanque { get; set; }
        public int TanqueId { get; set; }
        public Pozo? Pozo { get; set; }
        public int PozoId { get; set; }

        //datos de la prueba
        public double GROSS { get; set; }
        public double GSV { get; set; }
        public double SyW { get; set; }
        public double AceiteNeto { get; set; }
        public double AGUA { get; set; }
        public double NaftaInyectadaNeta { get; set; }


        //Metodos de la prueba proyectada a 24 horas
        public double GROSS24() { 
            return (double)(GROSS *24/TiempoProyectado);
        }
        public double GSV24()
        { 
        return (double)(GSV *24/TiempoProyectado);
        }
        public double SyW24()
        { 
        return SyW;
        }
        public double ACEITENETO24()
        { 
        return (double)(AceiteNeto *24/TiempoProyectado);
        }
        public double AGUA24()
        { 
        return (double)(AGUA *24/TiempoProyectado);
        }
        public double NAFTAINYECTADANETA24()
        { 
        return (double)(NaftaInyectadaNeta *24/TiempoProyectado);
        }

        //Parametros VSD

        public double? THP { get; set; } = null;
        public double? VOLTAJE_MOTOR { get; set; } = null;
        public double? THT { get; set; }= null;
        public double? FRECUENCIA { get; set; }=null;
        public double? PIP { get; set; } = null;
        public double? PRESION_DESCARGA { get; set; } = null;
        public double? T_INTAKE { get; set; } = null;
        public double? T_MOTOR { get; set; } = null;
        public double? CORRIENTE_MOTOR { get; set; } = null;
        public double? SYW_CABEZA { get; set; } = null;
        public double? SYW_MEZCLA { get; set; }
        public double? CABEZA_API60 { get; set; } = null;
        public double? MEZCLA_API60 { get; set; }
        public double? PH { get; set; } = null;
        public double? Cl { get; set; } = null;
        public string OBESERVACIONES { get; set; }=string.Empty;

    }
}
