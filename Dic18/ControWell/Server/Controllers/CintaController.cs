using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CintaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CintaController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cinta>>> GetCinta()
        {
            var lista = await _context.Cintas.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Cinta>>> GetSingleCinta(int id)
        {
            var miobjeto = await _context.Cintas.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Cinta>> CreateCinta(Cinta objeto)
        {

            _context.Cintas.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbCinta());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Cinta>>> UpdateCinta(Cinta objeto)
        {

            var DbObjeto = await _context.Cintas.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Nombre = objeto.Nombre;
            DbObjeto.Descripcion = objeto.Descripcion;


            await _context.SaveChangesAsync();

            return Ok(await _context.Cintas.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Cinta>>> DeleteCinta(int id)
        {
            var DbObjeto = await _context.Cintas.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Cintas.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbCinta());
        }


        private async Task<List<Cinta>> GetDbCinta()
        {
            return await _context.Cintas.ToListAsync();
        }

    }
}
