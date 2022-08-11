using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTrackR.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrackR.API.Persistence
{
    public class DevTrackRContext : DbContext
    {
        public DevTrackRContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageUpdate> PackageUpdate { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Package>(e => {
                e.HasKey(p => p.Id);

                e.HasMany(p => p.Updates)
                .WithOne()
                .HasForeignKey(pu => pu.PackageId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PackageUpdate> (e => {
                e.HasKey(p => p.Id);
            });
        }
    }
}