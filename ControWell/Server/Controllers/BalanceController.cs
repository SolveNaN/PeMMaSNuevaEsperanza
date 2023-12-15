using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BalanceController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Balance>>> GetBalance()
        {
            var lista = await _context.Balances.Include(t=>t.Tanque).Include(c=>c.Cinta).Include(t=>t.Termometro).ToListAsync();
            return Ok(lista);
        }

        [HttpPost]
        [Route("Rango")]
        public async Task<ActionResult<List<Balance>>> GetBalanceRango(Consulta consulta)
        {
            var lista = await _context.Balances.Include(t => t.Tanque).Include(c => c.Cinta).Include(t => t.Termometro).Where(x=>x.Fecha>=consulta.FechaInicial&&x.Fecha<=consulta.FechaFinal).ToListAsync();
            return Ok(lista);
        }

        [HttpPost]
        [Route("Aforos")]
        public async Task<ActionResult<List<Balance>>> ListadoEstadoUltimo(Consulta consulta)
        {
            
            List<Balance> TodosLosBalances = new List<Balance>();
            List<Balance> UltimosRegistrosDeBalances = new List<Balance>();            
            TodosLosBalances = await _context.Balances.Include(t => t.Tanque).Where(x => x.Fecha <= consulta.FechaFinal).ToListAsync();
            foreach (var j in _context.Tanques)
            {
                var BalancesPorTanque = TodosLosBalances.Where(x => x.TanqueId == j.Id);
                if (BalancesPorTanque.Count() > 0)
                {
                    int UltimoId = BalancesPorTanque.Max(x => x.Id);
                    Balance UltimoBalance = BalancesPorTanque.Where(x => x.Id == UltimoId).FirstOrDefault();
                    UltimosRegistrosDeBalances.Add(UltimoBalance);
                }

            }

            return Ok(UltimosRegistrosDeBalances);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<Balance>>> GetSingleBalance(int id)
        {
            var miobjeto = await _context.Balances.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]
        public async Task<ActionResult<Balance>> CreateBalance(Balance mibalance)
        {
            Tanque? mitanque = new Tanque();
            Termometro? mitermometro = new Termometro();
            Cinta? micinta = new Cinta();
            mitanque = await _context.Tanques.Where(x => x.Id == mibalance.TanqueId).FirstOrDefaultAsync();
            mitermometro = await _context.Termometros.Where(x => x.Id == mibalance.TermometroId).FirstOrDefaultAsync();
            micinta = await _context.Cintas.Where(x => x.Id == mibalance.CintaId).FirstOrDefaultAsync();
            mibalance.Tanque = mitanque;
            mibalance.Cinta = micinta;
            mibalance.Termometro = mitermometro;
            double Interpolar(double x1, double x2, double y1, double y2, double x)
            {

                return y1 + ((x - x1) / (x2 - x1)) * (y2 - y1);
            }
            var Niveles = await _context.NivelesDeLaCintas.Where(x => x.CintaId == mibalance.CintaId).ToListAsync();//Los factores de correccion cinta para ese tanque
            var CintaOrdenada = Niveles.OrderBy(x => x.Nominal).ToList();
            double subX1 = 0; double subX2 = 0; double subY1 = 0; double subY2 = 0;//inicializo los parametros de entrada de la funcion
            double correccionencm = 0;//inicializo la variable que me toma el factor de correccion
            //los nominales que estan en una posicion mayor al nivel
            var NominalMayoresAlNivel = CintaOrdenada.Where(x => x.Nominal > mibalance.Nivel).ToList();
            //los nominales que estan en una posicion menor al nivel
            var NominalMenoresAlNivel = CintaOrdenada.Where(x => x.Nominal < mibalance.Nivel).ToList();
            var NominalMinimoCintaNivel = CintaOrdenada.Min(x => x.Nominal);
            var Val2 = NominalMayoresAlNivel.Min(x => x.Nominal);//este es el valor que está por encima del nivel
            var Val1 = NominalMenoresAlNivel.Max(x => x.Nominal);//este es el valor que está por debajo
            if (Val2 != NominalMinimoCintaNivel)
            {
                foreach (var i in CintaOrdenada)
                {
                    if (i.Nominal == Convert.ToDouble(Val1)) { subX1 = Convert.ToDouble(i.Nominal); subY1 = Convert.ToDouble(i.Correccion); }
                    if (i.Nominal == Convert.ToDouble(Val2)) { subX2 = Convert.ToDouble(i.Nominal); subY2 = Convert.ToDouble(i.Correccion); }
                }
                correccionencm = Interpolar(subX1, subX2, subY1, subY2, Convert.ToDouble(mibalance.Nivel));

            }

            correccionencm = Math.Round(correccionencm, 2);

            mibalance.FactorCinta = correccionencm;

            //FIN-------------------------------------------------------------------------CORRECCIÓN POR CINTA PARA NIVEL TOTAL*******************************************************************
            //INICIO-------------------------------------------------------------------------CORRECCIÓN POR TERMOMETRO PARA TEMPERATURA DEL TANQUE*******************************************************************

            var CalibracionActual = _context.CalibracionTermometros.Where(x => x.TermometroId == mibalance.TermometroId).ToList();
            var CalibracionActualOrdenada = CalibracionActual.OrderBy(x => x.Nominal).ToList();
            double subX1Cal = 0; double subX2Cal = 0; double subY1Cal = 0; double subY2Cal = 0;//inicializo los parametros de entrada de la funcion
            double correccionF = 0;//inicializo la variable que me toma el factor de correccion
            var NominalMayoresATemp = CalibracionActualOrdenada.Where(x => x.Nominal > mibalance.TemTanque).ToList();
            var NominalMenoresTemp = CalibracionActualOrdenada.Where(x => x.Nominal < mibalance.TemTanque).ToList();
            var NominalMinimoTermo = CalibracionActualOrdenada.Min(x => x.Nominal);
            var Val1Temp = NominalMenoresTemp.Max(x => x.Nominal);
            var Val2Temp = NominalMayoresATemp.Min(x => x.Nominal);

            if (Val2Temp != NominalMinimoTermo)
            {
                foreach (var i in CalibracionActualOrdenada)
                {
                    if (i.Nominal == Convert.ToDouble(Val1Temp)) { subX1Cal = Convert.ToDouble(i.Nominal); subY1Cal = Convert.ToDouble(i.Correccion); }
                    if (i.Nominal == Convert.ToDouble(Val2Temp)) { subX2Cal = Convert.ToDouble(i.Nominal); subY2Cal = Convert.ToDouble(i.Correccion); }
                }
                correccionF = Interpolar(subX1Cal, subX2Cal, subY1Cal, subY2Cal, Convert.ToDouble(mibalance.TemTanque));


            }
            mibalance.FactorTemTanque = correccionF;
            //FINAL-------------------------------------------------------------------------CORRECCIÓN POR TERMOMETRO PARA TEMPERATURA DEL TANQUE*******************************************************************
            //INICIO-------------------------------------------------------------------------CORRECCIÓN POR CINTA PARA INTERFASE*******************************************************************
            double subX1Agua = 0; double subX2Agua = 0; double correccionagua = 0; double valorTemporalAgua = 0; double subY1Agua = 0; double subY2Agua = 0;//INICIALIZO VARIABLES
            double correccionencmagua = 0;
            var NominalMayoresAlNivelAgua = CintaOrdenada.Where(x => x.Nominal > mibalance.Interfase).ToList();
            //los nominales que estan en una posicion menor al nivel
            var NominalMenoresAlNivelAgua = CintaOrdenada.Where(x => x.Nominal < mibalance.Interfase).ToList();
            var NominalMinimoCinta = CintaOrdenada.Min(x => x.Nominal);
            var Val2Agua = NominalMayoresAlNivelAgua.Min(x => x.Nominal);
            var Val1Agua = NominalMenoresAlNivelAgua.Max(x => x.Nominal);
            if (Val2Agua != NominalMinimoCinta)
            {
                foreach (var i in CintaOrdenada)
                {
                    if (i.Nominal == Convert.ToDouble(Val1Agua)) { subX1Agua = Convert.ToDouble(i.Nominal); subY1Agua = Convert.ToDouble(i.Correccion); }
                    if (i.Nominal == Convert.ToDouble(Val2Agua)) { subX2Agua = Convert.ToDouble(i.Nominal); subY2Agua = Convert.ToDouble(i.Correccion); }
                }

                correccionencmagua = Interpolar(subX1Agua, subX2Agua, subY1Agua, subY2Agua, Convert.ToDouble(mibalance.Interfase));
            }

            mibalance.FactorInterface = correccionencmagua;

            //FIN-------------------------------------------------------------------------CORRECCIÓN POR CINTA PARA INTERFASE*******************************************************************

            //INICIO*-*-*-*-*-*-----------------------------------------------------------------------------FIN CALCULO DE VOLUMEN TOTAL*****************************************************************

            var AforosDelTanque = _context.AforoTKs.Where(x => x.TanqueId == mibalance.TanqueId).ToList();//estos son los afoforos para ese tanque
            var AforoCorrespondiente = AforosDelTanque.Where(x => x.Nivel == mibalance.NivelCorregido()).FirstOrDefault();
            mibalance.Tov = Convert.ToDouble(Math.Round(Convert.ToDecimal(AforoCorrespondiente.Volumen), 2));


            //FIN*-*-*-*-*-*-----------------------------------------------------------------------------FIN CALCULO DE VOLUMEN TOTAL*****************************************************************

            //INIIO****************************************************************************************calculo del agua libre FW****************************************************************


            var AforoCorrespondienteAgua = AforosDelTanque.Where(x => x.Nivel == mibalance.InterfaseCorregida()).FirstOrDefault();
            mibalance.Fw = Convert.ToDouble(Math.Round(Convert.ToDecimal(AforoCorrespondienteAgua.Volumen), 2));
            //FIN***********************************************************************************CALCULO FW AGUA LIBRE*****************************************************************
            //CALCULO DE LOS DELTA
            var balancesDelTan =_context.Balances.Where(y => y.TanqueId == mibalance.TanqueId).ToList();
            if(balancesDelTan.Count > 0)
            {
                var fechita = balancesDelTan.Max(x => x.Id);
                var Ultimo = balancesDelTan.Where(x => x.Id == fechita).FirstOrDefault();
                mibalance.DeltaNivel = mibalance.Nivel - Ultimo.Nivel;
                mibalance.DeltaInterfase = mibalance.Interfase - Ultimo.Interfase;
                mibalance.DeltaTov = mibalance.Tov - Ultimo.Tov;
                mibalance.DeltaFw = mibalance.Fw - Ultimo.Fw;
                mibalance.DeltaGov = mibalance.GOV() - Ultimo.GOV();
                mibalance.DeltaGsv = mibalance.GSV() - Ultimo.GSV();
                mibalance.DeltaNsv = mibalance.NSV() - Ultimo.NSV();
                mibalance.DeltaAguaNeta = mibalance.AGUANETA() - Ultimo.AGUANETA();
            }
            else
            {
                mibalance.DeltaNivel = mibalance.Nivel;
                mibalance.DeltaInterfase = mibalance.Interfase;
                mibalance.DeltaTov = mibalance.Tov;
                mibalance.DeltaFw = mibalance.Fw;
                mibalance.DeltaGov = mibalance.GOV();
                mibalance.DeltaGsv = mibalance.GSV();
                mibalance.DeltaNsv = mibalance.NSV();
                mibalance.DeltaAguaNeta = mibalance.AGUANETA();
            }            
            
            _context.Balances.Add(mibalance);
            await _context.SaveChangesAsync();
            return Ok(await GetBalance());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Balance>>> UpdateBalance(Balance objeto)
        {

            var DbObjeto = await _context.Balances.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.Fecha = objeto.Fecha;
            DbObjeto.TipoMovimiento = objeto.TipoMovimiento;
            DbObjeto.Nivel = objeto.Nivel;
            DbObjeto.Interfase = objeto.Interfase;
            DbObjeto.Api = objeto.Api;
            DbObjeto.TemFluidoApi = objeto.TemFluidoApi;
            DbObjeto.TemAmbiente = objeto.TemAmbiente;
            DbObjeto.TemTanque = objeto.TemTanque;
            DbObjeto.KarlFisher = objeto.KarlFisher;
            DbObjeto.Syw = objeto.Syw;
            DbObjeto.IdentificadorPrueba = objeto.IdentificadorPrueba;


            await _context.SaveChangesAsync();

            return Ok(await _context.Balances.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<Balance>>> DeleteBalance(int id)
        {
            var DbObjeto = await _context.Balances.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.Balances.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbBalance());
        }


        private async Task<List<Balance>> GetDbBalance()
        {
            return await _context.Balances.ToListAsync();
        }
        //CALCULOS AÑADIDOS, VALIDACION
        [HttpPost]
        [Route("Validar")]
        public async Task<ActionResult> LlenarObjetoBalance([FromBody] Balance mibalance)
        {

            Tanque? mitanque = new Tanque();
            Termometro? mitermometro = new Termometro();
            Cinta? micinta = new Cinta();
            mitanque = await _context.Tanques.Where(x => x.Id == mibalance.TanqueId).FirstOrDefaultAsync();
            mitermometro = await _context.Termometros.Where(x => x.Id == mibalance.TermometroId).FirstOrDefaultAsync();
            micinta = await _context.Cintas.Where(x => x.Id == mibalance.CintaId).FirstOrDefaultAsync();
            mibalance.Tanque = mitanque;
            mibalance.Cinta = micinta;
            mibalance.Termometro = mitermometro;
            double Interpolar(double x1, double x2, double y1, double y2, double x)
            {

                return y1 + ((x - x1) / (x2 - x1)) * (y2 - y1);
            }
            var Niveles = await _context.NivelesDeLaCintas.Where(x => x.CintaId == mibalance.CintaId).ToListAsync();//Los factores de correccion cinta para ese tanque
            var CintaOrdenada = Niveles.OrderBy(x => x.Nominal).ToList();
            double subX1 = 0; double subX2 = 0; double subY1 = 0; double subY2 = 0;//inicializo los parametros de entrada de la funcion
            double correccionencm = 0;//inicializo la variable que me toma el factor de correccion
            //los nominales que estan en una posicion mayor al nivel
            var NominalMayoresAlNivel = CintaOrdenada.Where(x => x.Nominal > mibalance.Nivel).ToList();
            //los nominales que estan en una posicion menor al nivel
            var NominalMenoresAlNivel = CintaOrdenada.Where(x => x.Nominal < mibalance.Nivel).ToList();
            var NominalMinimoCintaNivel = CintaOrdenada.Min(x => x.Nominal);
            var Val2 = NominalMayoresAlNivel.Min(x => x.Nominal);//este es el valor que está por encima del nivel
            var Val1 = NominalMenoresAlNivel.Max(x => x.Nominal);//este es el valor que está por debajo
            if (Val2 != NominalMinimoCintaNivel)
            {
                foreach (var i in CintaOrdenada)
                {
                    if (i.Nominal == Convert.ToDouble(Val1)) { subX1 = Convert.ToDouble(i.Nominal); subY1 = Convert.ToDouble(i.Correccion); }
                    if (i.Nominal == Convert.ToDouble(Val2)) { subX2 = Convert.ToDouble(i.Nominal); subY2 = Convert.ToDouble(i.Correccion); }
                }
                correccionencm = Interpolar(subX1, subX2, subY1, subY2, Convert.ToDouble(mibalance.Nivel));

            }

            correccionencm = Math.Round(correccionencm, 2);

            mibalance.FactorCinta = correccionencm;

            //FIN-------------------------------------------------------------------------CORRECCIÓN POR CINTA PARA NIVEL TOTAL*******************************************************************
            //INICIO-------------------------------------------------------------------------CORRECCIÓN POR TERMOMETRO PARA TEMPERATURA DEL TANQUE*******************************************************************

            var CalibracionActual = _context.CalibracionTermometros.Where(x => x.TermometroId == mibalance.TermometroId).ToList();
            var CalibracionActualOrdenada = CalibracionActual.OrderBy(x => x.Nominal).ToList();
            double subX1Cal = 0; double subX2Cal = 0; double subY1Cal = 0; double subY2Cal = 0;//inicializo los parametros de entrada de la funcion
            double correccionF = 0;//inicializo la variable que me toma el factor de correccion
            var NominalMayoresATemp = CalibracionActualOrdenada.Where(x => x.Nominal > mibalance.TemTanque).ToList();
            var NominalMenoresTemp = CalibracionActualOrdenada.Where(x => x.Nominal < mibalance.TemTanque).ToList();
            var NominalMinimoTermo = CalibracionActualOrdenada.Min(x => x.Nominal);
            var Val1Temp = NominalMenoresTemp.Max(x => x.Nominal);
            var Val2Temp = NominalMayoresATemp.Min(x => x.Nominal);

            if (Val2Temp != NominalMinimoTermo)
            {
                foreach (var i in CalibracionActualOrdenada)
                {
                    if (i.Nominal == Convert.ToDouble(Val1Temp)) { subX1Cal = Convert.ToDouble(i.Nominal); subY1Cal = Convert.ToDouble(i.Correccion); }
                    if (i.Nominal == Convert.ToDouble(Val2Temp)) { subX2Cal = Convert.ToDouble(i.Nominal); subY2Cal = Convert.ToDouble(i.Correccion); }
                }
                correccionF = Interpolar(subX1Cal, subX2Cal, subY1Cal, subY2Cal, Convert.ToDouble(mibalance.TemTanque));


            }
            mibalance.FactorTemTanque = correccionF;
            //FINAL-------------------------------------------------------------------------CORRECCIÓN POR TERMOMETRO PARA TEMPERATURA DEL TANQUE*******************************************************************
            //INICIO-------------------------------------------------------------------------CORRECCIÓN POR CINTA PARA INTERFASE*******************************************************************
            double subX1Agua = 0; double subX2Agua = 0; double correccionagua = 0; double valorTemporalAgua = 0; double subY1Agua = 0; double subY2Agua = 0;//INICIALIZO VARIABLES
            double correccionencmagua = 0;
            var NominalMayoresAlNivelAgua = CintaOrdenada.Where(x => x.Nominal > mibalance.Interfase).ToList();
            //los nominales que estan en una posicion menor al nivel
            var NominalMenoresAlNivelAgua = CintaOrdenada.Where(x => x.Nominal < mibalance.Interfase).ToList();
            var NominalMinimoCinta = CintaOrdenada.Min(x => x.Nominal);
            var Val2Agua = NominalMayoresAlNivelAgua.Min(x => x.Nominal);
            var Val1Agua = NominalMenoresAlNivelAgua.Max(x => x.Nominal);
            if (Val2Agua != NominalMinimoCinta)
            {
                foreach (var i in CintaOrdenada)
                {
                    if (i.Nominal == Convert.ToDouble(Val1Agua)) { subX1Agua = Convert.ToDouble(i.Nominal); subY1Agua = Convert.ToDouble(i.Correccion); }
                    if (i.Nominal == Convert.ToDouble(Val2Agua)) { subX2Agua = Convert.ToDouble(i.Nominal); subY2Agua = Convert.ToDouble(i.Correccion); }
                }

                correccionencmagua = Interpolar(subX1Agua, subX2Agua, subY1Agua, subY2Agua, Convert.ToDouble(mibalance.Interfase));
            }

            mibalance.FactorInterface = correccionencmagua;

            //FIN-------------------------------------------------------------------------CORRECCIÓN POR CINTA PARA INTERFASE*******************************************************************

            //INICIO*-*-*-*-*-*-----------------------------------------------------------------------------FIN CALCULO DE VOLUMEN TOTAL*****************************************************************

            var AforosDelTanque = _context.AforoTKs.Where(x => x.TanqueId == mibalance.TanqueId).ToList();//estos son los afoforos para ese tanque
            var AforoCorrespondiente = AforosDelTanque.Where(x => x.Nivel == mibalance.NivelCorregido()).FirstOrDefault();
            mibalance.Tov = Convert.ToDouble(Math.Round(Convert.ToDecimal(AforoCorrespondiente.Volumen), 2));


            //FIN*-*-*-*-*-*-----------------------------------------------------------------------------FIN CALCULO DE VOLUMEN TOTAL*****************************************************************

            //INIIO****************************************************************************************calculo del agua libre FW****************************************************************


            var AforoCorrespondienteAgua = AforosDelTanque.Where(x => x.Nivel == mibalance.InterfaseCorregida()).FirstOrDefault();
            mibalance.Fw = Convert.ToDouble(Math.Round(Convert.ToDecimal(AforoCorrespondienteAgua.Volumen), 2));
            //FIN***********************************************************************************CALCULO FW AGUA LIBRE*****************************************************************
            //CALCULO DE LOS DELTA
            var balancesDelTan = _context.Balances.Where(y => y.TanqueId == mibalance.TanqueId).ToList();
            if (balancesDelTan.Count > 0)
            {
                var fechita = balancesDelTan.Max(x => x.Id);
                var Ultimo = balancesDelTan.Where(x => x.Id == fechita).FirstOrDefault();
                mibalance.DeltaNivel = mibalance.Nivel - Ultimo.Nivel;
                mibalance.DeltaInterfase = mibalance.Interfase - Ultimo.Interfase;
                mibalance.DeltaTov = mibalance.Tov - Ultimo.Tov;
                mibalance.DeltaFw = mibalance.Fw - Ultimo.Fw;
                mibalance.DeltaGov = mibalance.GOV() - Ultimo.GOV();
                mibalance.DeltaGsv = mibalance.GSV() - Ultimo.GSV();
                mibalance.DeltaNsv = mibalance.NSV() - Ultimo.NSV();
                mibalance.DeltaAguaNeta = mibalance.AGUANETA() - Ultimo.AGUANETA();
            }
            else
            {
                mibalance.DeltaNivel = mibalance.Nivel;
                mibalance.DeltaInterfase = mibalance.Interfase;
                mibalance.DeltaTov = mibalance.Tov;
                mibalance.DeltaFw = mibalance.Fw;
                mibalance.DeltaGov = mibalance.GOV();
                mibalance.DeltaGsv = mibalance.GSV();
                mibalance.DeltaNsv = mibalance.NSV();
                mibalance.DeltaAguaNeta = mibalance.AGUANETA();
            }
                        
            return Ok(mibalance);

        }

    }
}
