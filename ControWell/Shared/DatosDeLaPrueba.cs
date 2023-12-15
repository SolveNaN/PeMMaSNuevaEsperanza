using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class DatosDeLaPrueba
    {
        public int Id { get; set; }
        public TestPozo? TestPozo { get; set; }
        public int TestPozoId { get; set; }

        

        //datos de la prueba

        public DateTime Fecha { get; set; }= DateTime.Now;
        public double? GROSS { get; set; }
        public double? GSV { get; set; }
        public double? SyW { get; set; }
        public double? AceiteNeto { get; set; }
        public double? AGUA { get; set; }
        public double? NaftaInyectadaNeta { get; set; }


        //Metodos de la prueba proyectada a 24 horas
        public double? GROSS24()
        {
            return GROSS;
        }
        public double? GSV24()
        {
            return GSV;
        }
        public double? SyW24()

        {
            return SyW;
        }
        public double? ACEITENETO24()
        {
            return AceiteNeto;
        }
        public double? AGUA24()
        {
            return AGUA;
        }
        public double? NAFTAINYECTADANETA24()
        {
            return NaftaInyectadaNeta;
        }

        //Parametros VSD

        public double? THP { get; set; }//ES LO MISMO QUE WHP??
        public double? VOLTAJE_MOTOR { get; set; }
        public double? THT { get; set; }//ES LO MISMO QUE WHT
        public double? FRECUENCIA { get; set; }
        public double? PIP { get; set; }
        public double? PRESION_DESCARGA { get; set; }//PDP ES PRESION DESCARGA
        public double? T_INTAKE { get; set; }
        public double? T_MOTOR { get; set; }
        public double? CORRIENTE_MOTOR { get; set; }
        public double? SYW_CABEZA { get; set; }
        public double? SYW_MEZCLA { get; set; }
        public double? CABEZA_API60 { get; set; }
        public double? MEZCLA_API60 { get; set; }
        public double? PH { get; set; }
        public double? Cl { get; set; }
        public string OBESERVACIONES { get; set; } = string.Empty;
    }
}
