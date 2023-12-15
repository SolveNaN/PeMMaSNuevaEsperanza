using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozoController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public PozoController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pozo>>> GetPozo()
        {
            var lista = await _context.Pozos.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Pozo>>> GetSinglePozo(int id)
        {
            var miobjeto = await _context.Pozos.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Pozo>> CreatePozo(Pozo objeto)
        {

            _context.Pozos.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbPozo());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Pozo>>> UpdatePozo(Pozo objeto)
        {

            var DbObjeto = await _context.Pozos.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.NombrePozo = objeto.NombrePozo;
            DbObjeto.Ubicacion = objeto.Ubicacion;
            DbObjeto.Operadora = objeto.Operadora;
            DbObjeto.Comentario = objeto.Comentario;
            DbObjeto.Estado = objeto.Estado;
            DbObjeto.Fre = objeto.Fre;


            await _context.SaveChangesAsync();

            return Ok(await _context.Pozos.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Pozo>>> DeletePozo(int id)
        {
            var DbObjeto = await _context.Pozos.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Pozos.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbPozo());
        }


        private async Task<List<Pozo>> GetDbPozo()
        {
            return await _context.Pozos.ToListAsync();
        }

    }
}
