using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipWACH.Model
{
    public class NewRegion
    {
        public int IDRegion { get; set; }
        public string Region { get; set; }
        public string Settlements { get; set; }
        public int QtyPrivateBuilding { get; set; }
        public int QtyBuilding { get; set; }
        public int QtyApartment { get; set; }
    }
}
