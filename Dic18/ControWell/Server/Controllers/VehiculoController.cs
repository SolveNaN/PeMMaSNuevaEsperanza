using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehiculoController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehiculo>>> GetVehiculo()
        {
            var Vehiculo = await _context.Vehiculos.ToListAsync();
            return Ok(Vehiculo);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Vehiculo>>> GetSingleVehiculo(int id)
        {
            var Vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(p => p.Id == id);
            if (Vehiculo == null)
            {
                return NotFound("La Vehiculo de proceso no ha sido creada :/");
            }

            return Ok(Vehiculo);
        }

        [HttpPost]

        public async Task<ActionResult<Vehiculo>> CreateVehiculo(Vehiculo Vehiculo)
        {

            _context.Vehiculos.Add(Vehiculo);
            await _context.SaveChangesAsync();
            return Ok(await GetDbVehiculo());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Vehiculo>>> UpdateVehiculo(Vehiculo Vehiculo)
        {

            var DbVehiculo = await _context.Vehiculos.FindAsync(Vehiculo.Id);
            if (DbVehiculo == null)
                return BadRequest("La Vehiculo no se encuentra");
            DbVehiculo.TipoVehiculo = Vehiculo.TipoVehiculo;
            DbVehiculo.Capacidad = Vehiculo.Capacidad;
            DbVehiculo.Estado = Vehiculo.Estado;

            await _context.SaveChangesAsync();

            return Ok(await _context.Vehiculos.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Vehiculo>>> DeleteVehiculo(int id)
        {
            var DbVehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Id == id);
            if (DbVehiculo == null)
            {
                return NotFound("La Vehiculo no existe :/");
            }

            _context.Vehiculos.Remove(DbVehiculo);
            await _context.SaveChangesAsync();

            return Ok(await GetDbVehiculo());
        }


        private async Task<List<Vehiculo>> GetDbVehiculo()
        {
            return await _context.Vehiculos.ToListAsync();
        }
    }
}
