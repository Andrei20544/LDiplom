namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Apartment")]
    public partial class Apartment
    {
        public int ID { get; set; }

        public int? Number { get; set; }

        public int QtyPeople { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public double? WMeter { get; set; }

        public int IDBuilding { get; set; }

        public bool? IsWMeter { get; set; }
    }
}
