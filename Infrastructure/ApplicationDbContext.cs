using Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>, IUnitOfWork
    {

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Link> Link { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:ApplicationDbContextConnection"]);
        }


        #region IUnitOfWork Implementations

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            base.Set<TEntity>().AddRange(entities);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            base.Set<TEntity>().RemoveRange(entities);
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry(entity).State = EntityState.Modified; // Or use ---> this.Update(entity);
        }

        public void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry(entity).State = EntityState.Deleted; // Or use ---> this.Update(entity);
        }

        public void ExecuteSqlCommand(string query)
        {
            base.Database.ExecuteSqlCommand(query);
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            base.Database.ExecuteSqlCommand(query, parameters);
        }

        public int SaveAllChanges()
        {
            return base.SaveChanges();
        }

        public Task<int> SaveAllChangesAsync()
        {
            return base.SaveChangesAsync();
        }


        #endregion

    }
}
