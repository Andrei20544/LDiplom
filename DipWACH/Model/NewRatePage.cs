using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Model
{
    public class NewRatePage
    {
        public int ID { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double? PriceWIn { get; set; }
        public double? PriceWOut { get; set; }
        public string RegionName { get; set; }
    }
}
