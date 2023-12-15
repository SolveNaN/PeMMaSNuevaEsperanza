using ControWell.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NivelesDeLaCintaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NivelesDeLaCintaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<NivelesDeLaCinta>>> GetNivelesDeLaCinta()//Aqui se obtiene la lista ordenada siempre
        {
            var Cintas = from af in _context.NivelesDeLaCintas
                         orderby af.Id ascending
                         select af;


            return Ok(Cintas);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<NivelesDeLaCinta>>> GetSingleNivelesDeLaCinta(int id)
        {
            var cinta = await _context.NivelesDeLaCintas.FirstOrDefaultAsync(a => a.Id == id);
            if (cinta == null)
            {
                return NotFound("El Cinta no fue encontrado :/");
            }

            return Ok(cinta);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<NivelesDeLaCinta>>> DeleteCinta(int id)
        {
            var dbCinta = await _context.NivelesDeLaCintas.FirstOrDefaultAsync(a => a.Id == id);
            if (dbCinta == null)
            {
                return NotFound("El Cinta no existe :/");
            }

            _context.NivelesDeLaCintas.Remove(dbCinta);
            await _context.SaveChangesAsync();

            return Ok(await GetDbCinta());
        }


        [HttpPost]

        public async Task<ActionResult<NivelesDeLaCinta>> CreateNivelesDeLaCinta(NivelesDeLaCinta cinta)
        {

            _context.NivelesDeLaCintas.Add(cinta);
            await _context.SaveChangesAsync();
            return Ok(await GetDbCinta());
        }

        private async Task<List<NivelesDeLaCinta>> GetDbCinta()
        {
            return await _context.NivelesDeLaCintas.ToListAsync();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<List<NivelesDeLaCinta>>> UpdateNivelesDeLaCinta(NivelesDeLaCinta Cinta)
        {

            var DbCinta = await _context.NivelesDeLaCintas.FindAsync(Cinta.Id);
            if (DbCinta == null)
                return BadRequest("El Cinta no se encuentra");
            DbCinta.Id = Cinta.Id;


            await _context.SaveChangesAsync();

            return Ok(await _context.NivelesDeLaCintas.ToListAsync());

        }
    }
}
