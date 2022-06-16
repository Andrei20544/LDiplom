using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Model
{
    public class NewApartment
    {
        public int ID { get; set; }
        public int? Number { get; set; }
        public int QtyPeople { get; set; }
        public string RegionName { get; set; }
        public double? RatePriceWIn { get; set; }
        public double? RatePriceWOut { get; set; }
        public double? WMeter { get; set; }
        public bool? IsWMeter { get; set; }
        public string Address { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
