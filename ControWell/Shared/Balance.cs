using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Balance
    {
        public int Id { get; set; }
        //INICIO LLAVES FORANEAS        
        public Tanque? Tanque { get; set; }
        public int TanqueId { get; set; }
        
        public Cinta? Cinta { get; set; }
        public int CintaId { get; set; }
        public Termometro? Termometro { get; set; }
        public int TermometroId { get; set; }
        
        //FINAL LLAVES FORANEAS
        //INICIO DATOS INGRESADOS
        public DateTime? Fecha { get; set; } = DateTime.Now;
        public string TipoMovimiento { get; set; } = string.Empty;
        public double? Nivel { get; set; }
        public double? Interfase { get; set; }
        public double? Api { get; set; }
        public double? TemFluidoApi { get; set; }
        public double? TemAmbiente { get; set; }
        public double? TemTanque { get; set; }
        public double? KarlFisher { get; set; }
        public double? Syw { get; set; }
        public int? IdentificadorPrueba { get; set; }//Para las pruebas de Pozos
        public string NombreUsuario { get; set; }=string.Empty;
        //FINAL DATOS INGRESADOS
        //INICIO DATOS CALCULADOS POR CONTROLADOR
        public double? FactorCinta { get; set; }
        public double? FactorInterface { get; set; }
        public double? FactorTemTanque { get; set; }
        public double Tov { get; set; }
        public double Fw { get; set; }
        public double Nsv { get; set; }
        public double AguaNeta { get; set; }
        public double? DeltaNivel { get; set; }
        public double? DeltaInterfase { get; set; }
        public double? DeltaTov { get; set; }
        public double? DeltaFw { get; set; }
        public double? DeltaGov { get; set; }
        public double? DeltaGsv { get; set; }
        public double? DeltaNsv { get; set; }
        public double? DeltaAguaNeta { get; set; }
        public string Observacion { get; set; } = string.Empty;
        //FINAL DATOS CALCULADOS POR CONTROLADOR
        //INICIO METODOS

        public double? NivelCorregido()
        {
            return (double?)Math.Round((decimal)(Nivel + FactorCinta), 2);
        }

        public double? InterfaseCorregida()
        {
            return (double?)Math.Round((decimal)(Interfase + FactorInterface), 2);
        }
        public double? TemTanqueCorregido()
        {
            return TemTanque + FactorTemTanque;
        }


        public double Api60F()
        {
            double miApi = -1;
            if (Tanque.TipoFluido == "Crudo")
            {
                double TempOb = (double)TemFluidoApi;
                double Pres = 0;
                double Apiob = (double)Api;
                double dens60 = 0;
                double den = ((141.5 / (Apiob + 131.5)) * 999.016) * (1 - 0.00001278 * (TempOb - 60) - (0.0000000062 * Math.Pow((TempOb - 60), 2)));

                if (den < 610.6)
                {
                    den = 610.6;
                }
                if (den > 1163.5)
                {
                    den = 1163.5;
                }
                dens60 = den;
                double tc90 = (TempOb - 32) / 1.8;
                double T = tc90 / 630;
                double a1 = -0.148759;
                double a2 = -0.267408;
                double a3 = 1.08076;
                double a4 = 1.269056;
                double a5 = -4.089591;
                double a6 = -1.871251;
                double a7 = 7.438081;
                double a8 = -3.536296;

                double DTT = (a1 + (a2 + (a3 + (a4 + (a5 + (a6 + (a7 + a8 * T) * T) * T) * T) * T) * T) * T) * T;
                double tc68 = tc90 + DTT;
                double tf68 = 1.8 * tc68 + 32;
                double k0 = 341.0957;
                double k1 = 0;
                double k2 = 0;
                double Da = 2;
                double s60 = 0.01374979547;
                double m = 0;

                double a = (s60 / 2) * ((((k0 / dens60) + k1) / dens60) + k2);
                double b = (2 * k0 + k1 + dens60) / (k0 + (k1 + k2 + dens60) * dens60);
                double De = dens60 * (1 + ((Math.Exp(a * (1 + 0.8 * a)) - 1) / (1 + a * (1 + 1.6 * a) * b)));
                double alfa60 = k2 + ((k0 / De) + k1) / De;


                double DT = tf68 - 60.0068749;
                double CTLc = Math.Exp(-alfa60 * DT * (1 + 0.8 * alfa60 * (DT + s60)));
                double Fp = Math.Exp(-1.9947 + 0.00013427 * tf68 + ((793920 + (2326 * tf68)) / Math.Pow(De, 2)));
                double CPL2 = 1 / (1 + Fp * Pres * Math.Pow(10, -5));
                double CTPL2 = CTLc * CPL2;
                CTPL2 = Math.Round(CTPL2, 5);
                double dt2 = TempOb - 60;
                double x = dens60 * CTPL2;
                double spo = dens60 - x;

                double e = (den / (CTLc * CPL2)) - dens60;
                double Dtm = 2 * alfa60 * dt2 * (1 + 1.6 * alfa60 * dt2);
                double Dp = (Da * CPL2 * Pres * Fp * (7.9392 + 0.02326 * TempOb)) / (Math.Pow(dens60, 2));
                double Dden60 = e / (1 + Dtm + Dp);

                if ((dens60 + Dden60) < 610.6)
                {
                    Dden60 = 610.6 - dens60;
                }
                if ((dens60 + Dden60) > 1163.5)
                {
                    Dden60 = 1163.5 - dens60;
                }
                if (Math.Abs(spo) < 0.000001)
                {
                    //
                }
                else
                {
                    dens60 = dens60 + Dden60;
                }
                double d60 = dens60;

                double API_60F_hyc = ((141.5 / (d60 / 999.016)) - 131.5);

                miApi= Convert.ToDouble(API_60F_hyc);
            }
            else if (Tanque.TipoFluido == "Refinado")

            {
                double TempOb = (double)TemFluidoApi;
                double Pres = 0;
                double Apiob = (double)Api;
                double dens60 = 0;
                double den = ((141.5 / (Apiob + 131.5)) * 999.016) * (1 - 0.00001278 * (TempOb - 60) - (0.0000000062 * Math.Pow((TempOb - 60), 2)));

                if (den < 610.6)
                {
                    den = 610.6;
                }
                if (den >= 1163.5)
                {
                    den = 1163.5;
                }

                dens60 = den;
                double tc90 = (TempOb - 32) / 1.8;
                double T = tc90 / 630;
                double a1 = -0.148759;
                double a2 = -0.267408;
                double a3 = 1.08076;
                double a4 = 1.269056;
                double a5 = -4.089591;
                double a6 = -1.871251;
                double a7 = 7.438081;
                double a8 = -3.536296;

                double DTT = (a1 + (a2 + (a3 + (a4 + (a5 + (a6 + (a7 + a8 * T) * T) * T) * T) * T) * T) * T) * T;
                double tc68 = tc90 - DTT;
                double tf68 = 1.8 * tc68 + 32;
                double k0 = 192.4571;
                double k1 = 0.2438;
                double k2 = 0;
                double m = 0;
                double Da = 2;
                double s60 = 0.01374979547;

                while (m < 15)
                {
                    double a = (s60 / 2) * ((((k0 / dens60) + k1) / dens60) + k2);
                    double b = (2 * k0 + k1 + dens60) / (k0 + (k1 + k2 + dens60) * dens60);
                    double De = dens60 * (1 + ((Math.Exp(a * (1 + 0.8 * a)) - 1) / (1 + a * (1 + 1.6 * a) * b)));
                    double alfa60 = k2 + ((k0 / De + k1) / De);


                    double DT = tf68 - 60.0068749;
                    double CTLc = Math.Exp(-alfa60 * DT * (1 + 0.8 * alfa60 * (DT + s60)));
                    double Fp = Math.Exp(-1.9947 + 0.00013427 * tf68 + ((793920 + (2326 * tf68)) / Math.Pow(De, 2)));
                    double CPL2 = 1 / (1 + Fp * Pres * Math.Pow(10, -5));
                    double CTPL2 = CTLc * CPL2;
                    CTPL2 = Math.Round(CTPL2, 5);
                    double dt2 = TempOb - 60;
                    double x = dens60 * CTPL2;
                    double spo = dens60 - x;

                    double e = (den / (CTLc * CPL2)) - dens60;
                    double Dtm = 1.5 * alfa60 * dt2 * (1 + 1.6 * alfa60 * dt2);

                    double Dp = (Da * CPL2 * Pres * Fp * (7.9392 + 0.02326 * TempOb)) / (Math.Pow(dens60, 2));
                    double Dden60 = e / (1 + Dtm + Dp);

                    if ((dens60 + Dden60) < 610.6)
                    {
                        Dden60 = 610.6 - dens60;
                    }
                    if ((dens60 + Dden60) > 770.352)
                    {
                        Dden60 = 770.352 - dens60;
                    }
                    if (Math.Abs(spo) < 0.000001)
                    {
                        break;
                    }
                    else
                    {
                        dens60 = dens60 + Dden60;
                        m = m + 1;
                    }
                    double d60 = dens60;

                    double API_60F_hyc = ((141.5 / (d60 / 999.016)) - 131.5);

                    miApi= Convert.ToDouble(API_60F_hyc);

                }


            }
            return Math.Round(miApi,2);
        }
        public double Ctsh()
        {
            double tempFluido = (double)TemTanqueCorregido();
            double tempAmbiente = (double)TemAmbiente;
            double tempBase = (double)Tanque.TBase;
            string material = Tanque.Material;

            double coefMaterial = 0;
            switch (material)
            {
                case "Mild Carbon":
                    coefMaterial = 0.0000062;
                    break;
                case "304 Stainless":
                    coefMaterial = 0.0000096;
                    break;
                case "316 Stainless":
                    coefMaterial = 0.00000883;
                    break;
                case "17-4PH Stainless":
                    coefMaterial = 0.000006;
                    break;
                case "N/A":
                    coefMaterial = 1;
                    break;
                    // default:
                    //throw new AssertionError();
            }
            double ctsh = Math.Round(1 + (2 * coefMaterial * (((Math.Round(((7 * tempFluido) + tempAmbiente) / 8, 0))) - tempBase)) + (Math.Pow(coefMaterial, 2) * Math.Pow(((Math.Round(((7 * tempFluido) + tempAmbiente) / 8, 0)) - tempAmbiente), 2)), 5);
            return ctsh;
        }
        public double GOV()
        {
            double tov = Tov;
            double fw = Fw;
            double ctsh = Ctsh();
            double gov = 0;
            if (fw > 0)
            {
                if (tov >= fw)
                {
                    gov = (tov - fw) * ctsh;
                }
                else
                {
                    // JOptionPane.showMessageDialog(null, "Revise los niveles ingresados");
                }
            }
            else
            {
                gov = tov * ctsh;
            }

            return Math.Round(gov,2);
        }
        public double CTL()
        {
            double api = (double)Api60F();
            double tempFluido = (double)TemTanque;
            string tipoFluido = Tanque.TipoFluido;
            double ctl = 0;
            double k0 = 0, k1 = 0, k2 = 0;
            //'CTL para crudo

            double RD = 141.5 / (api + 131.5); // gravedad especifica
            double d = RD * 999.016; // densidad en kg/m3

            if (tempFluido < -58 || tempFluido > 302)
            {
                //JOptionPane.showMessageDialog(null, "Temperatura fuera de rango");
            }
            else
            {
                double d60 = d;
                if (d60 < 610.6 || d60 > 1163.5)
                {
                    //JOptionPane.showMessageDialog(null, "API fuera de rango");
                }
                else
                {
                    //constantes K0, K1 y K2
                    if ("Crudo" == tipoFluido)
                    {
                        k0 = 341.0957;
                        k1 = 0;
                        k2 = 0;
                    }
                    else if ("Lubricante" == tipoFluido)
                    {
                        k0 = 0;
                        k1 = 0.34878;
                        k2 = 0;
                    }
                    else if ("Refinado" == tipoFluido && (d60 < 770.352))
                    {
                        k0 = 192.4571;
                        k1 = 0.2438;
                        k2 = 0;
                    }
                    else if ("Refinado" == tipoFluido && (d60 < 787.5195))
                    {
                        k0 = 1489.067;
                        k1 = 0;
                        k2 = -0.0018684;
                    }
                    else if (("Refinado" == tipoFluido) && (d60 < 838.3127))
                    {
                        k0 = 330.301;
                        k1 = 0;
                        k2 = 0;
                    }
                    else if (("Refinado" == tipoFluido) && (d60 < 1163.5))
                    {
                        k0 = 103.872;
                        k1 = 0.2701;
                        k2 = 0;
                    }
                    double tC90 = (tempFluido - 32) / 1.8;//'conversion ITS80 to IPTS-68 Basis
                    double r = tC90 / 630;
                    double DT = (-0.148759 + (-0.267408 + (1.08076 + (1.269056 + (-4.089591 + (-1.871251 + (7.438081 + (-3.536296) * r) * r) * r) * r) * r) * r) * r) * r;
                    double TC68 = tC90 - DT;
                    double TF68 = TC68 * 1.8 + 32;
                    double a = 0.01374979547 / 2 * ((k0 / d60 + k1) / d60 + k2);
                    double b = (2 * k0 + k1 * d60) / (k0 + (k1 + k2 * d60) * d60);
                    double dr = d60 * (1 + (Math.Exp(a * (1 + 0.8 * a)) - 1) / (1 + a * (1 + 1.6 * a) * b));// ' densidad base convertida a IPTS-68 60'F  (kg/m3)
                    double a60 = (k0 / dr + k1) / dr + k2;// 'Thermal expansion factor at 60'F
                    double DTr = TF68 - 60.0068749;
                    double vcfc = Math.Exp(-a60 * DTr * (1 + 0.8 * a60 * (DTr + 0.01374979547)));// 'CTL
                    double FP = Math.Exp(-1.9947 + (0.00013427 * TF68) + ((793920 + (2326 * TF68)) / (dr * dr)));
                    double CPL = (1 / (1 - (0.00001 * FP)));//PSIG???
                    ctl = vcfc * CPL;                    
                }
            }
            return Math.Round(ctl,5);
        }
        public double GSV()
        {
            double gov = GOV();
            double ctl = CTL();
            double gsv = gov * ctl;
            return gsv;
        }
        
        public double? CalcularSyw(double? Kar, double? api)
        {
            return (Kar * (141.5 / (131.5 + api)) + 0.01);
        }

        public double CSW()
        {
            double sYw = 0;
            if (Syw >= 0 && KarlFisher >= 0)
            {
                sYw = (double)CalcularSyw(KarlFisher, Api60F());
            }
            if (Syw >= 0 && KarlFisher == null)
            {
                sYw = (double)Syw;
            }
            if (Syw == null && KarlFisher >= 0)
            {
                sYw = (double)CalcularSyw(KarlFisher, Api60F());
            }
            if (Syw == null && KarlFisher == null)
            {
                sYw = -1;
            }
            double csw = (sYw >= 0 && sYw <= 100) ? Math.Round((1 - sYw / 100), 5) : 0;
            return Math.Round(csw,5);
        }

        public double CTLNafta(double api, double tempFluido, string tipoFluido)
        {
            api=Api60F();
            tempFluido = (double)TemFluidoApi;
            tipoFluido = Tanque.TipoFluido;
            double ctl = 0;
            double k0 = 0, k1 = 0, k2 = 0;
            //'CTL para nafta

            double RD = 141.5 / (api + 131.5); // gravedad especifica
            double d = RD * 999.016; // densidad en kg/m3
            double p = 0;
            double psig = p;
            if (tempFluido < -58 || tempFluido > 302)
            {
                //JOptionPane.showMessageDialog(null, "Temperatura fuera de rango");
            }
            else
            {
                double d60 = d;
                if (d60 < 610.6 || d60 > 1163.5)
                {
                    //JOptionPane.showMessageDialog(null, "API fuera de rango");
                }
                else
                {
                    //constantes K0, K1 y K2

                    if ("Refinado" == tipoFluido && (d60 < 770.352))
                    {
                        k0 = 192.4571;
                        k1 = 0.2438;
                        k2 = 0;
                    }


                    double tC90 = (tempFluido - 32) / 1.8;//'conversion ITS80 to IPTS-68 Basis
                    double r = tC90 / 630;
                    double DT = (-0.148759 + (-0.267408 + (1.08076 + (1.269056 + (-4.089591 + (-1.871251 + (7.438081 + (-3.536296) * r) * r) * r) * r) * r) * r) * r) * r;
                    double TC68 = tC90 - DT;
                    double TF68 = TC68 * 1.8 + 32;
                    double a = 0.01374979547 / 2 * ((k0 / d60 + k1) / d60 + k2);
                    double b = (2 * k0 + k1 * d60) / (k0 + (k1 + k2 * d60) * d60);
                    double dr = d60 * (1 + (Math.Exp(a * (1 + 0.8 * a)) - 1) / (1 + a * (1 + 1.6 * a) * b));// ' densidad base convertida a IPTS-68 60'F  (kg/m3)
                    double a60 = (k0 / dr + k1) / dr + k2;// 'Thermal expansion factor at 60'F
                    double DTr = TF68 - 60.0068749;
                    double vcfc = Math.Exp(-a60 * DTr * (1 + 0.8 * a60 * (DTr + 0.01374979547)));// 'CTL
                    double FP = Math.Exp(-1.9947 + (0.00013427 * TF68) + ((793920 + (2326 * TF68)) / (dr * dr)));
                    double CPL = (1 / (1 - (0.00001 * FP * psig)));
                    ctl = vcfc * CPL;
                    ctl = Math.Round(ctl, 5);
                }
            }
            return Math.Round(ctl,5);
        }
        public double NSV()
        {
            double nsv = Math.Round(GSV() * CSW(), 2);
            return Math.Round(nsv,2);
        }
        public double AGUANETA()
        {
            return Math.Round(Fw + GSV() - NSV(),2);
        }
    }
}
