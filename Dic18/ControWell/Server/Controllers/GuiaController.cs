using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuiaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public GuiaController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Guia>>> GetGuia()
        {
            var lista = await _context.Guias.Where(g=>g.Estado==1).ToListAsync();
            return Ok(lista);
        }
        [HttpGet]
        [Route("guasanuladas")]
        public async Task<ActionResult<List<Guia>>> GetGuiaAnulada()
        {
            var lista = await _context.Guias.Where(g => g.Estado == -1&&g.Fecha>=DateTime.Today.Date).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        [Route("guasusadas")]
        public async Task<ActionResult<List<Guia>>> GetGuiaUsada()
        {
            var lista = await _context.Guias.Where(g => g.Estado == 0 && g.Fecha >= DateTime.Today.Date).ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Guia>>> GetSingleGuia(int id)
        {
            var miobjeto = await _context.Guias.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Guia>> CreateGuia(Guia objeto)
        {

            _context.Guias.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbGuia());
        }

        [HttpPost]
        [Route("CrearGuias")]
        public async Task<ActionResult<int>> CrearListadoSellos(List<Guia> guias)
        {
            foreach (var i in guias)
            {
                _context.Guias.Add(i);
                await _context.SaveChangesAsync();
            }

            return 1;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<List<Guia>>> UpdateGuia(Guia objeto)
        {

            var DbObjeto = await _context.Guias.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.NumeroGuia = objeto.NumeroGuia;
            DbObjeto.Estado = objeto.Estado;


            await _context.SaveChangesAsync();

            return Ok(await _context.Guias.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Guia>>> DeleteGuia(int id)
        {
            var DbObjeto = await _context.Guias.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Guias.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbGuia());
        }


        private async Task<List<Guia>> GetDbGuia()
        {
            return await _context.Guias.ToListAsync();
        }
    }
}

