namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Building")]
    public partial class Building
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Building()
        {
            Apartment = new HashSet<Apartment>();
        }

        public int ID { get; set; }

        public int? QtyFloor { get; set; }

        public int? QtyApartment { get; set; }

        public int? QtyPeople { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        public int? IDRegion { get; set; }

        public bool? IsPrivate { get; set; }

        public double? Penalties { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Apartment> Apartment { get; set; }

        public virtual Region Region { get; set; }
    }
}
