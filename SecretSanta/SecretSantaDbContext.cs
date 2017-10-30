using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta
{
    public class SecretSantaDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=SecretSanta;Integrated Security=SSPI");
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable("Person");
            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Person>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Group>()
                .ToTable("Group");
            modelBuilder.Entity<Group>()
                .Property(g => g.Name)
                .HasMaxLength(255);
            modelBuilder.Entity<Group>()
                .Property(g => g.LeaderName)
                .HasMaxLength(100);
            modelBuilder.Entity<Group>()
                .Property(g => g.LeaderEmail)
                .HasMaxLength(50);

            modelBuilder.Entity<PersonGroup>()
                .ToTable("PersonGroup")
                .HasKey(pg => new { pg.PersonId, pg.GroupId });

            modelBuilder.Entity<ExceptionGroup>()
                .ToTable("ExceptionGroup");
            modelBuilder.Entity<ExceptionGroup>()
                .Property(eg => eg.Name)
                .HasMaxLength(255);

            modelBuilder.Entity<PersonExceptionGroup>()
                .ToTable("PersonExceptionGroup");
        }
    }
}
