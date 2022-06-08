namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Service")]
    public partial class Service
    {
        public int ID { get; set; }

        public int IDRate { get; set; }

        public int IDTypeService { get; set; }
    }
}
