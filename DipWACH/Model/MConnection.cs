namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MConnection")]
    public partial class MConnection
    {
        public int ID { get; set; }

        public int IDApartment { get; set; }

        public int? IDEmployee { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public bool? BPlan { get; set; }

        public bool? Procuration { get; set; }

        public virtual Apartment Apartment { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
