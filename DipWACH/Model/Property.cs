namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Property")]
    public partial class Property
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string IDTypeProperty { get; set; }
    }
}
