using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConductorController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ConductorController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Conductor>>> GetConductor()
        {
            var lista = await _context.Conductors.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Conductor>>> GetSingleConductor(int id)
        {
            var miobjeto = await _context.Conductors.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Conductor>> CreatePozo(Conductor objeto)
        {

            _context.Conductors.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbConductor());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Conductor>>> UpdateConductor(Conductor objeto)
        {

            var DbObjeto = await _context.Conductors.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Nombre = objeto.Nombre;
            DbObjeto.Cedula = objeto.Cedula;
            DbObjeto.Descripcion = objeto.Descripcion;


            await _context.SaveChangesAsync();

            return Ok(await _context.Conductors.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Conductor>>> DeleteConductor(int id)
        {
            var DbObjeto = await _context.Conductors.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Conductors.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbConductor());
        }


        private async Task<List<Conductor>> GetDbConductor()
        {
            return await _context.Conductors.ToListAsync();
        }
    }  
}
