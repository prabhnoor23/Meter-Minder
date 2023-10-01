using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pspcl.Core.Domain;
using System.Reflection.Emit;

namespace Pspcl.DBConnect
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Circle> Circle { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialGroup> MaterialGroup { get; set; }
        public DbSet<MaterialType> MaterialType { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<RatingMaterialTypeMapping> RatingMaterialTypeMapping { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockBookMaterial> StockBookMaterial { get; set; }
        public DbSet<StockIssueBook> StockIssueBook { get; set; }
        public DbSet<StockMaterial> StockMaterial { get; set; }
        public DbSet<StockMaterialSeries> StockMaterialSeries { get; set; }
        public DbSet<SubDivision> SubDivision { get; set; }

        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(b =>
            {
                b.ToTable("User");

                // Relationships
                b.Property(all => all.FirstName).HasMaxLength(256).IsRequired(false);
                b.Property(all => all.LastName).HasMaxLength(256).IsRequired(false);

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                b.ToTable("Role");
                // Primary key
                b.HasKey(r => r.Id);

                // Index for "normalized" role name to allow efficient lookups
                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                // Limit the size of columns to use efficient database types
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            builder.Entity<UserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId }).IsClustered(true);
                b.ToTable("User_Role_Mapping");
            });

            builder.Entity<Stock>(b =>
            {
                b.ToTable("Stock");

                // Relationships
                b.Property(all => all.TestReportReference).HasMaxLength(256).IsRequired(false);
                b.Property(all => all.TransactionId).HasMaxLength(256).IsRequired(false);
                b.Property(all => all.Rate).HasColumnType("Numeric(18, 2)").IsRequired(true);
            });
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> Save(bool acceptAllChangesOnSuccess)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

        public async Task<int> Save()
        {
            return await base.SaveChangesAsync();
        }
    }
}
