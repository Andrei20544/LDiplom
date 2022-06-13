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

        public virtual DbSet<Apartment> Apartment { get; set; }
        public virtual DbSet<Building> Building { get; set; }
        public virtual DbSet<CalcOfAccruals> CalcOfAccruals { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<TypeBuilding> TypeBuilding { get; set; }
        public virtual DbSet<TypeEmployee> TypeEmployee { get; set; }
        public virtual DbSet<TypeRate> TypeRate { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
