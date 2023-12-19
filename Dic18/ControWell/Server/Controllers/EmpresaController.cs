using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmpresaController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empresa>>> GetEmpresa()
        {
            var lista = await _context.Empresas.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Empresa>>> GetSingleEmpresa(int id)
        {
            var miobjeto = await _context.Empresas.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<Empresa>> CreatePozo(Empresa objeto)
        {

            _context.Empresas.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbEmpresa());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Empresa>>> UpdateEmpresa(Empresa objeto)
        {

            var DbObjeto = await _context.Empresas.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Nombre = objeto.Nombre;
            DbObjeto.Nit = objeto.Nit;
            DbObjeto.Descripcion = objeto.Descripcion;           


            await _context.SaveChangesAsync();

            return Ok(await _context.Empresas.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Empresa>>> DeleteEmpresa(int id)
        {
            var DbObjeto = await _context.Empresas.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Empresas.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbEmpresa());
        }


        private async Task<List<Empresa>> GetDbEmpresa()
        {
            return await _context.Empresas.ToListAsync();
        }
    }
}
