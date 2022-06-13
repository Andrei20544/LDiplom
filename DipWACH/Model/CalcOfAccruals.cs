namespace DipWACH.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CalcOfAccruals
    {
        public int ID { get; set; }

        public int IDEmployee { get; set; }

        public int IDSubscriber { get; set; }
    }
}
