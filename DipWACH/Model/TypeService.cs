namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TypeService")]
    public partial class TypeService
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string TypeName { get; set; }
    }
}
