using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Schany.Data.Entities.Sys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Schany.Data.DataContext
{
    public class SchanyDbContext : DbContext
    {
        public SchanyDbContext(DbContextOptions<SchanyDbContext> options) : base(options)
        {
        }

        public DbSet<MyDictionary> MyDictionaries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOpenPlateAccount> CustomerOpenPlateAccounts { get; set; }
        public DbSet<PresetDataPermission> PresetDataPermissions { get; set; }
        public DbSet<Module> Modules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //模型验证（用于添加或修改数据时）
            var serviceProvider = ((IInfrastructure<IServiceProvider>)this).Instance;
            var items = new Dictionary<object, object>();
            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity, serviceProvider, items);
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
