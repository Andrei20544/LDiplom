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
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<MConnection> MConnection { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Subscriber> Subscriber { get; set; }
        public virtual DbSet<TypeEmployee> TypeEmployee { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>()
                .HasMany(e => e.MConnection)
                .WithRequired(e => e.Apartment)
                .HasForeignKey(e => e.IDApartment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.Apartment)
                .WithRequired(e => e.Building)
                .HasForeignKey(e => e.IDBuilding)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.MConnection)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.IDEmployee);

            modelBuilder.Entity<Rate>()
                .HasMany(e => e.Region)
                .WithOptional(e => e.Rate)
                .HasForeignKey(e => e.IDRate);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.Building)
                .WithOptional(e => e.Region)
                .HasForeignKey(e => e.IDRegion);

            modelBuilder.Entity<TypeEmployee>()
                .HasMany(e => e.Employee)
                .WithRequired(e => e.TypeEmployee)
                .HasForeignKey(e => e.IDTypeEmployee)
                .WillCascadeOnDelete(false);
        }
    }
}
