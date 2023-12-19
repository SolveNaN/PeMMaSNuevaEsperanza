using ControWell.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AforoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AforoController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Aforo>>> GetAforo()
        {
            var lista = await _context.AforoTKs.ToListAsync();
            return Ok(lista);
        }
        [HttpGet]
        [Route("Tanque/{idTanque}")]
        public async Task<ActionResult<List<Aforo>>> GetAforoTanque(int idTanque)
        {
            var lista = await _context.AforoTKs.Where(x=>x.TanqueId==idTanque).ToListAsync();
            return Ok(lista);
        }



        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Aforo>>> GetSingleAforo(int id)
        {
            var miobjeto = await _context.AforoTKs.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Aforo>> CreateAforo(Aforo objeto)
        {

            _context.AforoTKs.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbAforo());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Aforo>>> UpdateAforo(Aforo objeto)
        {

            var DbObjeto = await _context.AforoTKs.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.TanqueId = objeto.TanqueId;
            DbObjeto.Nivel = objeto.Nivel;
            DbObjeto.Volumen = objeto.Volumen;
            DbObjeto.Incremento = objeto.Incremento;


            await _context.SaveChangesAsync();

            return Ok(await _context.AforoTKs.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Aforo>>> DeleteAforo(int id)
        {
            var DbObjeto = await _context.AforoTKs.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.AforoTKs.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbAforo());
        }


        private async Task<List<Aforo>> GetDbAforo()
        {
            return await _context.AforoTKs.ToListAsync();
        }

        [HttpPost]
        [Route("ActualizarAforos")]
        public async Task<ActionResult<string>> CreateAforo(List<Aforo> aforos)
        {
            foreach (var i in aforos)
            {
                _context.AforoTKs.Add(i);
                await _context.SaveChangesAsync();
            }

            return Ok("Bien");
        }
    }
}
