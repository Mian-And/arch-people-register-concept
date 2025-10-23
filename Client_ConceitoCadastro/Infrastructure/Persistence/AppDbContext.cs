using Client_ConceitoCadastro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Infrastructure.Persistence
{
    // Infrastructure/Persistence/AppDbContext.cs
    public class AppDbContext : DbContext
    {
        public DbSet<Delivery> Deliveries => Set<Delivery>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var delivery = modelBuilder.Entity<Delivery>();

            delivery.HasKey(x => x.Id);
            delivery.Property(x => x.RecipientName)
                .IsRequired()
                .HasMaxLength(200);
            delivery.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(2000);
            delivery.Property(x => x.HouseNumber)
                .HasMaxLength(20);
            
            // Owned Type (Address)
            delivery.OwnsOne(x => x.Address, a =>
            {
                a.Property(p => p.ZipCode)
                    .HasColumnName("Address_ZipCode")
                    .HasMaxLength(8);
                a.Property(p => p.Street)
                    .HasColumnName("Address_Street")
                    .HasMaxLength(200);
                a.Property(p => p.Neighborhood)
                    .HasColumnName("Address_Neighborhood")
                    .HasMaxLength(120);
                a.Property(p => p.City)
                    .HasColumnName("Address_City")
                    .HasMaxLength(120);
                a.Property(p => p.State)
                    .HasColumnName("Address_State")
                    .HasMaxLength(2);
                a.Property(p => p.Complement)
                    .HasColumnName("Address_Complement")
                    .HasMaxLength(120);
            });
        }


    }

}
