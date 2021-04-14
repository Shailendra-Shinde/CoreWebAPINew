using CoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Data
{
    public class BatchContext : DbContext
    {
        public BatchContext(DbContextOptions<BatchContext> options) : base(options)
        {
        }

        public DbSet<BatchViewModel> BatchDetails { get; set; }

        public DbSet<Acl> Acl { get; set; }

        public DbSet<Models.Attribute> Attribute { get; set; }

        public DbSet<BusinessUnit> BusinessUnits { get; set; }

        //public DbSet<ReadUser> ReadUsers { get; set; }

        //public DbSet<ReadGroup> ReadGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchViewModel>().ToTable("BatchDetails");

            modelBuilder.Entity<Acl>()
            .Property(e => e.ReadUsers)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Acl>()
            .Property(e => e.ReadGroups)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<BusinessUnit>().HasData(
                new BusinessUnit
                {
                    BusinessUnitId = 1,
                    BusinessUnitName = "UKHO"
                }
            );
        }
    }
}
