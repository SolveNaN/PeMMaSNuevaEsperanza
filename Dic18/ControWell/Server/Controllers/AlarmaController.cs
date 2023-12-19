using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public AlarmaController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Alarma>>> GetAlarma()
        {
            var lista = await _context.Alarmas.Include(p=>p.Pozo).Include(v=>v.VariableProceso).ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Alarma>>> GetSingleAlarma(int id)
        {
            var miobjeto = await _context.Alarmas.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Alarma>> CreateAlarma(Alarma objeto)
        {

            _context.Alarmas.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbAlarma());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Alarma>>> UpdateAlarma(Alarma objeto)
        {

            var DbObjeto = await _context.Alarmas.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.PozoId = objeto.PozoId;
            DbObjeto.VariableProcesoId = objeto.VariableProcesoId;
            DbObjeto.Max = objeto.Max;
            DbObjeto.Min = objeto.Min;
            DbObjeto.Habilitado = objeto.Habilitado;


            await _context.SaveChangesAsync();

            return Ok(await _context.Alarmas.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Alarma>>> DeleteAlarma(int id)
        {
            var DbObjeto = await _context.Alarmas.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Alarmas.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbAlarma());
        }


        private async Task<List<Alarma>> GetDbAlarma()
        {
            return await _context.Alarmas.ToListAsync();
        }

    }
}
