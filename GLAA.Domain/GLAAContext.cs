using GLAA.Domain.Models;
using System.Diagnostics.CodeAnalysis;
using GLAA.Domain.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GLAA.Domain
{
    [ExcludeFromCodeCoverage]
    public class GLAAContext : IdentityDbContext<GLAAUser, GLAARole, string>
    {
        public GLAAContext(DbContextOptions<GLAAContext> options) : base(options)
        {            
        }

        public DbSet<StatusReason> StatusReasons { get; set; }
        public DbSet<LicensingStandard> LicensingStandards { get; set; }
        public DbSet<LicenceStatus> LicenceStatuses { get; set; }
        public DbSet<LicenceStatusChange> LicenceStatusChanges { get; set; }
        public DbSet<Licence> Licences { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<WorkerCountry> WorkerCountries { get; set; }
        public DbSet<Multiple> Multiples { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<PrincipalAuthority> PrincipalAuthorities { get; set; }
        public DbSet<AlternativeBusinessRepresentative> AlternativeBusinessRepresentatives { get; set; }
        public DbSet<DirectorOrPartner> DirectorOrPartners { get; set; }
        public DbSet<NamedIndividual> NamedIndividuals { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RestraintOrder> RestraintOrders { get; set; }
        public DbSet<Conviction> Convictions { get; set; }
        public DbSet<OffenceAwaitingTrial> OffencesAwaitingTrial { get; set; }
        public DbSet<PreviousTradingName> PreviousTradingNames { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<PAYENumber> PAYENumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<GLAAUser>().ToTable("User", "dbo");

            modelBuilder.Entity<GLAARole>().ToTable("Role", "dbo");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            // many-to-many
            modelBuilder.Entity<LicenceWorkerCountry>()
                .HasKey(x => new { x.LicenceId, CountryId = x.WorkerCountryId });

            modelBuilder.Entity<LicenceWorkerCountry>()
                .HasOne(x => x.Licence)
                .WithMany(x => x.OperatingCountries)
                .HasForeignKey(x => x.LicenceId);

            modelBuilder.Entity<LicenceWorkerCountry>()
                .HasOne(x => x.WorkerCountry)
                .WithMany(x => x.Licences)
                .HasForeignKey(x => x.WorkerCountryId);

            modelBuilder.Entity<LicenceIndustry>()
                .HasKey(x => new { x.LicenceId, x.IndustryId });

            modelBuilder.Entity<LicenceIndustry>()
                .HasOne(x => x.Licence)
                .WithMany(x => x.OperatingIndustries)
                .HasForeignKey(x => x.LicenceId);

            modelBuilder.Entity<LicenceIndustry>()
                .HasOne(x => x.Industry)
                .WithMany(x => x.Licences)
                .HasForeignKey(x => x.IndustryId);

            modelBuilder.Entity<LicenceMultiple>()
                .HasKey(x => new { x.LicenceId, x.MultipleId });

            modelBuilder.Entity<LicenceMultiple>()
                .HasOne(x => x.Licence)
                .WithMany(x => x.SelectedMultiples)
                .HasForeignKey(x => x.LicenceId);

            modelBuilder.Entity<LicenceMultiple>()
                .HasOne(x => x.Multiple)
                .WithMany(x => x.Licences)
                .HasForeignKey(x => x.MultipleId);

            modelBuilder.Entity<LicenceSector>()
                .HasKey(x => new { x.LicenceId, x.SectorId });

            modelBuilder.Entity<LicenceSector>()
                .HasOne(x => x.Licence)
                .WithMany(x => x.SelectedSectors)
                .HasForeignKey(x => x.LicenceId);

            modelBuilder.Entity<LicenceSector>()
                .HasOne(x => x.Sector)
                .WithMany(x => x.Licences)
                .HasForeignKey(x => x.SectorId);



            modelBuilder.Entity<LicenceStatusNextStatus>()
                .HasOne(x => x.LicenceStatus)
                .WithMany(x => x.NextStatuses)
                .HasForeignKey(x => x.NextStatusId);

            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            //// https://stackoverflow.com/a/12331031
            //modelBuilder.Entity<LicenceStatus>().HasMany(s => s.NextStatuses).WithMany().Map(m =>
            //{
            //    m.MapLeftKey("Id");
            //    m.MapRightKey("NextStatusId");
            //    m.ToTable("LicenceStatusNextStatuses");
            //});

            //modelBuilder.Entity<DirectorOrPartner>()
            //    .HasOptional(dop => dop.PrincipalAuthority)
            //    .WithOptionalPrincipal(pa => pa.DirectorOrPartner);

            base.OnModelCreating(modelBuilder);            
        }
    }    
}
