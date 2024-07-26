using Microsoft.EntityFrameworkCore;
using Projet_gestionContacts.Models;

namespace Projet_gestionContacts.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<GContactModels.Utilisateurs> Utilisateurs { get; set; }
        public DbSet<GContactModels.Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GContactModels.Utilisateurs>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<GContactModels.Contact>()
                .HasKey(c => c.IdContact);

            modelBuilder.Entity<GContactModels.Contact>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
