using ControWell.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalibracionTermometroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalibracionTermometroController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CalibracionTermometro>>> GetCalibracionTermometros()//Aqui se obtiene la lista ordenada siempre
        {
            var CalibracionTermometros = from af in _context.CalibracionTermometros
                              orderby af.Id ascending
                              select af;


            return Ok(CalibracionTermometros);
        }



        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<CalibracionTermometro>>> GetSingleCalibracionTermometro(int id)
        {
            var res = await _context.CalibracionTermometros.FirstOrDefaultAsync(a => a.Id == id);
            if (res == null)
            {
                return NotFound("El CalibracionTermometro no fue encontrado :/");
            }

            return Ok(res);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<CalibracionTermometro>>> DeleteCalibracionTermometro(int id)
        {
            var dbCalibracionTermometro = await _context.CalibracionTermometros.FirstOrDefaultAsync(a => a.Id == id);
            if (dbCalibracionTermometro == null)
            {
                return NotFound("El CalibracionTermometro no existe :/");
            }

            _context.CalibracionTermometros.Remove(dbCalibracionTermometro);
            await _context.SaveChangesAsync();

            return Ok(await GetDbCalibracionTermometro());
        }


        [HttpPost]

        public async Task<ActionResult<CalibracionTermometro>> CreateCalibracionTermometro(CalibracionTermometro res)
        {

            _context.CalibracionTermometros.Add(res);
            await _context.SaveChangesAsync();
            return Ok(await GetDbCalibracionTermometro());
        }

        private async Task<List<CalibracionTermometro>> GetDbCalibracionTermometro()
        {
            return await _context.CalibracionTermometros.ToListAsync();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<List<CalibracionTermometro>>> UpdateCalibracionTermometro(CalibracionTermometro res)
        {

            var DbRes = await _context.CalibracionTermometros.FindAsync(res.Id);
            if (DbRes == null)
                return BadRequest("El Cinta no se encuentra");
            DbRes.Nominal = res.Nominal;
            DbRes.Correccion = res.Correccion;
            DbRes.TermometroId = res.TermometroId;


            await _context.SaveChangesAsync();

            return Ok(await _context.CalibracionTermometros.ToListAsync());

        }

    }
}
