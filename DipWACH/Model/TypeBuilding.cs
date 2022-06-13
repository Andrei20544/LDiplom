namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TypeBuilding")]
    public partial class TypeBuilding
    {
        public int ID { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }
}
