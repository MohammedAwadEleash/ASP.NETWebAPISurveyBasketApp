using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Persistence.EntitiesConfigurations;
using System.Numerics;
using System.Reflection;
using System.Security.Claims;

namespace SurveyBasket.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public   DbSet<Poll> Polls { set; get; }
        public   DbSet<Answer> Answers { set; get; }
        public   DbSet<Question> Questions { set; get; }
        public   DbSet<Vote> Votes { set; get; }
        public   DbSet<VoteAnswer> VoteAnswers { set; get; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(modelBuilder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entityEntry in entries)
            {

                if (entityEntry.State == EntityState.Added)
                    entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;

                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}