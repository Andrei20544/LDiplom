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
        public int Number { get; set; }
        public int QtyPeople { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
    }
}
