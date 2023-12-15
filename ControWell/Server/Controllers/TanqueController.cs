using ControWell.Server.Data;
using ControWell.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace NombreProyecto.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TanqueController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public TanqueController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tanque>>> GetTanque()
        {
            var lista = await _context.Tanques.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Tanque>>> GetSingleTanque(int id)
        {
            var miobjeto = await _context.Tanques.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Tanque>> CreateTanque(Tanque objeto)
        {

            _context.Tanques.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbTanque());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Tanque>>> UpdateTanque(Tanque objeto)
        {

            var DbObjeto = await _context.Tanques.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.NombreTanque = objeto.NombreTanque;
            DbObjeto.Capacidad = objeto.Capacidad;
            DbObjeto.TipoFluido = objeto.TipoFluido;
            DbObjeto.Material = objeto.Material;
            DbObjeto.TBase = objeto.TBase;


            await _context.SaveChangesAsync();

            return Ok(await _context.Tanques.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Tanque>>> DeleteTanque(int id)
        {
            var DbObjeto = await _context.Tanques.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Tanques.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbTanque());
        }


        private async Task<List<Tanque>> GetDbTanque()
        {
            return await _context.Tanques.ToListAsync();
        }

        [HttpGet]
        [Route("SinMovimiento")]
        public async Task<ActionResult<List<Tanque>>> TanquessinMovimientos()
        {              
            List<Tanque> TanqueSinMovimientos = new List<Tanque>();            
            List<Balance> balances = new List<Balance>();            
            balances= await _context.Balances.Include(t => t.Tanque).ToListAsync();
            foreach (var j in _context.Tanques)
            {
                var BalancesPorTanque= balances.Where(x=>x.TanqueId==j.Id).ToList();
                if (BalancesPorTanque.Count() > 0)
                {
                    //Ya está iniciado
                }
                else
                {
                    
                    TanqueSinMovimientos.Add(j);
                }
            }           
            return Ok(TanqueSinMovimientos);
        }      

    }
}
