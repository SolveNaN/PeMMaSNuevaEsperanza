using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Consolidado
    {
        public double SumaNsv(List<Balance> balances)
        {
            double sumansv = 0;
            foreach (var balance in balances)
            {
                sumansv += balance.NSV();
            }
            return Math.Round(sumansv,2);
        }

        public double SumaDeltaNsv(List<Balance> balances)
        {
            double? deltansv = 0;
            foreach (var balance in balances)
            {
                deltansv += balance.DeltaNsv;
            }
            return (double)Math.Round((decimal)deltansv, 2);
        }
    }
}
