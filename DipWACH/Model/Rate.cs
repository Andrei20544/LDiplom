namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rate")]
    public partial class Rate
    {
        public int ID { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public double? PriceWInto { get; set; }

        public double? PriceWOut { get; set; }
    }
}
