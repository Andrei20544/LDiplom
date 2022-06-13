namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TypeRate")]
    public partial class TypeRate
    {
        public int ID { get; set; }

        [StringLength(80)]
        public string Name { get; set; }
    }
}
