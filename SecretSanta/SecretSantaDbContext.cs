using Microsoft.EntityFrameworkCore;
using SecretSanta.Models;

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
        public DbSet<Event> Events { get; set; }
        public DbSet<ExceptionGroup> ExceptionGroups { get; set; }
        public DbSet<GiftPair> GiftPairs { get; set; }

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
                .IsRequired()
                .HasMaxLength(255);

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

            modelBuilder.Entity<Event>()
                .ToTable("Event");
            modelBuilder.Entity<Event>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<Event>()
                .Property(e => e.LeaderName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Event>()
                .Property(e => e.LeaderEmail)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<GiftPair>()
                .ToTable("GiftPair");
            modelBuilder.Entity<GiftPair>()
                .HasOne(gp => gp.Giver)
                .WithMany(p => p.GiverPairs)
                .HasForeignKey(gp => gp.GiverId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GiftPair>()
                .HasOne(gp => gp.Recipient)
                .WithMany(p => p.RecipientPairs)
                .HasForeignKey(gp => gp.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
