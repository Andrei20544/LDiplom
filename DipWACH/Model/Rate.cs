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

        [Required]
        [StringLength(255)]
        public string Region { get; set; }

        public double PriceNDS { get; set; }

        public double PriceWNDS { get; set; }
    }
}
