using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Repozytorium.Models
{
    // You can add profile data for the user by adding more properties to your Uzytkownik class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class FundPortalContext : IdentityDbContext
    {
        public FundPortalContext()
            : base("DefaultConnection")
        {
        }
        public static FundPortalContext Create()
        {
            return new FundPortalContext();
        }

        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundTyp> FundTyp { get; set; }
        public DbSet<Uzytkownik> Uzytkownik { get; set; }
        public DbSet<Transakcja> Transakcje { get; set; }
        public DbSet <FundTowarzystwo>FundTowarzystwa { get; set; }
        public DbSet <FundZakup>FundZakup { get; set; }
        public DbSet <FundWaluta>FundWaluta { get; set; }
        public DbSet<FundWycena> FundWycena { get; set; }
        public DbSet <FundMessages>FundMessages { get; set; }
        public DbSet <FundOplataTyp>FundOplatyTyp { get; set; }
        public DbSet<FundFile> FundFile { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Potrzebne dla klas Identity
            base.OnModelCreating(modelBuilder);
            // using System.Data.Entity.ModelConfiguration.Conventions;
            // Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel
            // w bazie danych
            // Zamiast Kategorie zostałaby utworzona tabela o nazwie Kategories
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Wyłącza konwencję CascadeDelete
            // CascadeDelete zostanie włączone za pomocą Fluent API
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Code First dla procedur w bazie danych
            modelBuilder.Entity<Fund>().MapToStoredProcedures();
            modelBuilder.Entity<FundTyp>().MapToStoredProcedures();
            modelBuilder.Entity<Transakcja>().MapToStoredProcedures();
            modelBuilder.Entity<FundTowarzystwo>().MapToStoredProcedures();
            modelBuilder.Entity<FundZakup>().MapToStoredProcedures();
            modelBuilder.Entity<FundWaluta>().MapToStoredProcedures();
            modelBuilder.Entity<FundWycena>().MapToStoredProcedures();
            modelBuilder.Entity<FundMessages>().MapToStoredProcedures();
            modelBuilder.Entity<FundOplataTyp>().MapToStoredProcedures();
            modelBuilder.Entity<FundFile>().MapToStoredProcedures();
        }
    }
}