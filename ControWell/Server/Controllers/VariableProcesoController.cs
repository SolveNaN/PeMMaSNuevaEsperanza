using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariableProcesoController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public VariableProcesoController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<VariableProceso>>> GetVariableProceso()
        {
            var lista = await _context.VariableProcesos.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<VariableProceso>>> GetSingleVariableProceso(int id)
        {
            var miobjeto = await _context.VariableProcesos.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<VariableProceso>> CreateVariableProceso(VariableProceso objeto)
        {

            _context.VariableProcesos.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbVariableProceso());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<VariableProceso>>> UpdateVariableProceso(VariableProceso objeto)
        {

            var DbObjeto = await _context.VariableProcesos.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Nombre = objeto.Nombre;
            DbObjeto.Unidad = objeto.Unidad;


            await _context.SaveChangesAsync();

            return Ok(await _context.VariableProcesos.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<VariableProceso>>> DeleteVariableProceso(int id)
        {
            var DbObjeto = await _context.VariableProcesos.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.VariableProcesos.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbVariableProceso());
        }


        private async Task<List<VariableProceso>> GetDbVariableProceso()
        {
            return await _context.VariableProcesos.ToListAsync();
        }

    }
}
