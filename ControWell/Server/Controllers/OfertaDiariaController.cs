using ControWell.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaDiariaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfertaDiariaController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OfertaDiaria>>> GetOfertaDiarias()//Aqui se obtiene la lista ordenada siempre
        {

            var lista = await _context.OfertaDiarias.Include(e => e.Empresa).Include(c => c.Conductor).Include(r => r.Ruta).ToListAsync();//acceder a tercer nivel de llave foranea

            var listaOrdenada=lista.OrderByDescending(x=>x.Id).ToList();
            return Ok(listaOrdenada);
        }



        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<OfertaDiaria>>> GetSingleOfertaDiaria(int id)
        {
            var res = await _context.OfertaDiarias.FirstOrDefaultAsync(a => a.Id == id);
            if (res == null)
            {
                return NotFound("El OfertaDiaria no fue encontrado :/");
            }

            return Ok(res);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<OfertaDiaria>>> DeleteOfertaDiaria(int id)
        {
            var dbOfertaDiaria = await _context.OfertaDiarias.FirstOrDefaultAsync(a => a.Id == id);
            if (dbOfertaDiaria == null)
            {
                return NotFound("El OfertaDiaria no existe :/");
            }

            _context.OfertaDiarias.Remove(dbOfertaDiaria);
            await _context.SaveChangesAsync();

            return Ok(await GetDbOfertaDiaria());
        }


        [HttpPost]

        public async Task<ActionResult<OfertaDiaria>> CreateOfertaDiaria(OfertaDiaria res)
        {

            _context.OfertaDiarias.Add(res);
            await _context.SaveChangesAsync();
            return Ok(await GetDbOfertaDiaria());
        }

        private async Task<List<OfertaDiaria>> GetDbOfertaDiaria()
        {
            return await _context.OfertaDiarias.ToListAsync();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<List<OfertaDiaria>>> UpdateOfertaDiaria(OfertaDiaria res)
        {

            var DbRes = await _context.OfertaDiarias.FindAsync(res.Id);
            if (DbRes == null)
                return BadRequest("El Cinta no se encuentra");
            DbRes.FechaCreacion = res.FechaCreacion;
            DbRes.Disponible = res.Disponible;


            await _context.SaveChangesAsync();

            return Ok(await _context.OfertaDiarias.ToListAsync());

        }



    }
}
