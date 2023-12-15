using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermometroController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public TermometroController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Termometro>>> GetTermometro()
        {
            var lista = await _context.Termometros.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Termometro>>> GetSingleTermometro(int id)
        {
            var miobjeto = await _context.Termometros.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Termometro>> CreateTermometro(Termometro objeto)
        {

            _context.Termometros.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbTermometro());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Termometro>>> UpdateTermometro(Termometro objeto)
        {

            var DbObjeto = await _context.Termometros.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Nombre = objeto.Nombre;
            DbObjeto.Descripcion = objeto.Descripcion;


            await _context.SaveChangesAsync();

            return Ok(await _context.Termometros.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Termometro>>> DeleteTermometro(int id)
        {
            var DbObjeto = await _context.Termometros.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Termometros.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbTermometro());
        }


        private async Task<List<Termometro>> GetDbTermometro()
        {
            return await _context.Termometros.ToListAsync();
        }

    }
}
