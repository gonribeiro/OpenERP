using Microsoft.EntityFrameworkCore;
using OpenERP.Data.Mappings.Auth;
using OpenERP.Data.Mappings.Global;
using OpenERP.Data.Mappings.HumanResource;
using OpenERP.Models.Auth;
using OpenERP.Models.Global;
using OpenERP.Models.HumanResource;
using System.Security.Claims;
using System.Text.Json;

namespace OpenERP.Data
{
    public class AppDbContext : DbContext
    {
        public bool IsSeeding { get; set; } // Prevents auditing during SeedDatabase()
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Audit> Audit { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployee { get; set; }
        public DbSet<EducationHistory> EducationHistories { get; set; }
        public DbSet<FileStorage> FileStorages { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobHistory> JobHistories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Remuneration> Remunerations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new ContactMap());
            modelBuilder.ApplyConfiguration(new CountryMap());
            modelBuilder.ApplyConfiguration(new DepartmentMap());
            modelBuilder.ApplyConfiguration(new EducationHistoryMap());
            modelBuilder.ApplyConfiguration(new EmployeeMap());
            modelBuilder.ApplyConfiguration(new JobHistoryMap());
            modelBuilder.ApplyConfiguration(new JobMap());
            modelBuilder.ApplyConfiguration(new StateMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new VacationMap());
            modelBuilder.ApplyConfiguration(new RemunerationMap());
        }

        public override int SaveChanges()
        {
            AddAuditLogs();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditLogs();
            return base.SaveChangesAsync(cancellationToken);
        }

        // @todo ID is not saved when the entity is added
        private void AddAuditLogs()
        {
            if (this.IsSeeding) return; // Prevents auditing during SeedDatabase()

            var auditEntries = new List<Audit>();
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                    || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // User not logged during loggin. Loggin create Refresh Token
                if (entry.Entity is RefreshToken) continue;
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new Exception("Unauthenticated user");

                var oldValues = entry.State != EntityState.Added ? JsonSerializer.Serialize(entry.OriginalValues.ToObject()) : string.Empty;
                var newValues = entry.State != EntityState.Deleted ? JsonSerializer.Serialize(entry.CurrentValues.ToObject()) : string.Empty;

                if (oldValues != newValues) // Prevent auditing if there are no changes
                {
                    var auditEntry = new Audit
                    {
                        EntityName = entry.Entity.GetType().Name,
                        Action = entry.State.ToString(),
                        OldValues = oldValues,
                        NewValues = newValues,
                        Timestamp = DateTime.UtcNow,
                        UserId = userId,
                    };

                    auditEntries.Add(auditEntry);
                }
            }

            Audit.AddRange(auditEntries);
        }
    }
}
