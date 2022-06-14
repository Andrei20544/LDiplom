namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscriber")]
    public partial class Subscriber
    {
        public int ID { get; set; }

        public int IDProperty { get; set; }

        [Required]
        [StringLength(255)]
        public string FIO { get; set; }

        [Required]
        [StringLength(11)]
        public string PassportData { get; set; }

        [Required]
        [StringLength(25)]
        public string INN { get; set; }

        [StringLength(200)]
        public string Rates { get; set; }

        public int? QtyPeople { get; set; }
    }
}
