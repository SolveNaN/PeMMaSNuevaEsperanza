using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public RutaController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ruta>>> GetRuta()
        {
            var lista = await _context.Rutas.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Ruta>>> GetSingleRuta(int id)
        {
            var miobjeto = await _context.Rutas.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Ruta>> CreateRuta(Ruta objeto)
        {

            _context.Rutas.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbRuta());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Ruta>>> UpdateRuta(Ruta objeto)
        {

            var DbObjeto = await _context.Rutas.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Nombre = objeto.Nombre;


            await _context.SaveChangesAsync();

            return Ok(await _context.Rutas.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Ruta>>> DeleteRuta(int id)
        {
            var DbObjeto = await _context.Rutas.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Rutas.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbRuta());
        }


        private async Task<List<Ruta>> GetDbRuta()
        {
            return await _context.Rutas.ToListAsync();
        }

    }
}
