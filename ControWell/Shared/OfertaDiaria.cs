using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class OfertaDiaria
    {
        public int Id { get; set; }
        public double? DocDeTransporte { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public Empresa? Empresa { get; set; }
        public int EmpresaId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string PlacaTanque { get; set; } = string.Empty;
        public Conductor? Conductor { get; set; }
        public int ConductorId { get; set; }
        public Ruta? Ruta { get; set; }
        public int RutaId { get; set; }
        public string Observacion { get; set; } = string.Empty;
        public int Disponible { get; set; } = 1;
        
    }
}
/*
 CREATE TABLE OfertaDiarias(
Id INT PRIMARY KEY IDENTITY(1,1),
DocDeTransporte VARCHAR(200) NULL,
FechaCreacion DATETIME NULL,
EmpresaId INT NULL,
Placa VARCHAR(200) NULL,
PlacaTanque VARCHAR(200) NULL,
ConductorId INT NULL,
RutaId INT NULL,
Observacion VARCHAR(200) NULL,
Disponible INT NULL,
FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id),
FOREIGN KEY (ConductorId) REFERENCES Conductors(Id),
FOREIGN KEY (RutaId) REFERENCES Rutas(Id),
)
 */