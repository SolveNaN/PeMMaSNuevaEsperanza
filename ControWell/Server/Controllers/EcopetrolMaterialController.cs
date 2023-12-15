using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcopetrolMaterialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EcopetrolMaterialController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EcopetrolMaterial>>> GetEcopetrolMaterial()
        {
            var lista = await _context.EcopetrolMaterials.ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<EcopetrolMaterial>>> GetSingleEcopetrolMaterial(int id)
        {
            var miobjeto = await _context.EcopetrolMaterials.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<EcopetrolMaterial>> CreatePozo(EcopetrolMaterial objeto)
        {

            _context.EcopetrolMaterials.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbEcopetrolMaterial());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<EcopetrolMaterial>>> UpdateEcopetrolMaterial(EcopetrolMaterial objeto)
        {

            var DbObjeto = await _context.EcopetrolMaterials.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Descripcion = objeto.Descripcion;
            DbObjeto.Codigo = objeto.Codigo;
            


            await _context.SaveChangesAsync();

            return Ok(await _context.EcopetrolMaterials.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<EcopetrolMaterial>>> DeleteEcopetrolMaterial(int id)
        {
            var DbObjeto = await _context.EcopetrolMaterials.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.EcopetrolMaterials.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbEcopetrolMaterial());
        }


        private async Task<List<EcopetrolMaterial>> GetDbEcopetrolMaterial()
        {
            return await _context.EcopetrolMaterials.ToListAsync();
        }
    }
}
