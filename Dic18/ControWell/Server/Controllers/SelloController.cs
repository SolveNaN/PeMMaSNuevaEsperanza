using ControWell.Server.Data;
using ControWell.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelloController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SelloController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sello>>> GetSello()
        {
            var sellos = await _context.Sellos.Where(s=>s.Estado==1).ToListAsync();
            return Ok(sellos);
        }
        [HttpGet]
        [Route("SellosAnulados")]
        public async Task<ActionResult<List<Sello>>> GetSelloAnulado()
        {
            var sellos = await _context.Sellos.Where(s => s.Estado == -1&&s.CreatedAt>=DateTime.Today.Date).ToListAsync();
            return Ok(sellos);
        }

        [HttpGet]
        [Route("SellosUsados")]
        public async Task<ActionResult<List<Sello>>> GetSelloUsado()
        {
            var sellos = await _context.Sellos.Where(s => s.Estado == 0 && s.CreatedAt >= DateTime.Today.Date).ToListAsync();
            return Ok(sellos);
        }
        [HttpPost]
        public async Task<ActionResult<Sello>> CreateSello(Sello sello)
        {

            _context.Sellos.Add(sello);
            await _context.SaveChangesAsync();
            return Ok(await GetDbSello());
        }
        [HttpPost]
        [Route("CrearSellos")]
        public async Task<ActionResult<int>> CrearListadoSellos(List<Sello> sellos)
        {
            foreach(var i in sellos)
            {
                _context.Sellos.Add(i);
                await _context.SaveChangesAsync();
            }
           
            return 1;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Sello>>> UpdateSello(Sello sello)
        {
            var DbSello = await _context.Sellos.FindAsync(sello.Id);
            if (DbSello == null)
                return BadRequest("EL sello no se encuentra");
            DbSello.NumeroSello = sello.NumeroSello;
            DbSello.IndiceSello = sello.IndiceSello;
            DbSello.Lote = sello.Lote;
            DbSello.Estado = sello.Estado;
            DbSello.CreatedAt = sello.CreatedAt;
            DbSello.EnUso = sello.EnUso;

            await _context.SaveChangesAsync();

            return Ok(await _context.Sellos.ToListAsync());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Sello>>> DeleteSello(int id)
        {
            var DbSello = await _context.Sellos.FirstOrDefaultAsync(v => v.Id == id);
            if (DbSello == null)
            {
                return NotFound("La Sello no existe :/");
            }

            _context.Sellos.Remove(DbSello);
            await _context.SaveChangesAsync();

            return Ok(await GetDbSello());
        }


        private async Task<List<Sello>> GetDbSello()
        {
            return await _context.Sellos.ToListAsync();
        }


        [HttpGet]
        [Route("ValidarSellos/{SellosSerealizados}")]
        public async Task<ActionResult> ValidarSellos(string SellosSerealizados)
        {

            int Valido = 1;
            int respuesta = 0;
            var sellos = JsonSerializer.Deserialize<List<Sello>>(SellosSerealizados);
            var sellosUsados = _context.Sellos.Where(x => x.Estado == 2);
            foreach(var i in sellosUsados)
            {
                foreach(var j in sellos)
                {
                    if (i.Id == j.Id)
                    {
                        Valido = 0;
                    }
                }
            }
            if (Valido == 1)
            {
                respuesta = 1;
            }
            else
            {
                respuesta = 0;
            }
            return Ok(respuesta);
        }
        


    }
}