using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DipWACH.Model
{
    public partial class ModelBD : DbContext
    {
        public ModelBD()
            : base("name=ModelBD")
        {
        }

        public virtual DbSet<CalcOfAccruals> CalcOfAccruals { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Subscriber> Subscriber { get; set; }
        public virtual DbSet<TypeEmployee> TypeEmployee { get; set; }
        public virtual DbSet<TypeProperty> TypeProperty { get; set; }
        public virtual DbSet<TypeService> TypeService { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
