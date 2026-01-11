using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Payspace.Domain;
using System.IO;
using Newtonsoft.Json;
using Payspace.Domain.Security;
using Payspace.Utilities;

namespace Payspace.API.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

     
        public DbSet<ProgressiveTaxRate> ProgressiveTaxRates { get; set; }
        public DbSet<PostalCode> PostalCodeMappings { get; set; }       
        public DbSet<TaxCalculationEvent> TaxCalculationEvents { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        #region seeding
        private IEnumerable<PostalCode> GetPostalMappingData()
        {
            string content = File.ReadAllText("./postalCodes.json");
            return JsonConvert.DeserializeObject<IEnumerable<PostalCode>>(content);
        }
        private IEnumerable<ProgressiveTaxRate> GetProgressiveTaxRateData()
        {
            string content = File.ReadAllText("./taxtable.json");
            return JsonConvert.DeserializeObject<IEnumerable<ProgressiveTaxRate>>(content);
        }
        private IEnumerable<User> GetUsers()
        {
            string content = File.ReadAllText("./users.json");
            return JsonConvert.DeserializeObject<IEnumerable<User>>(content).Select(x => new User()
            {
                CreatedOn = DateTime.Now,
                EmailAddress = x.EmailAddress,
                Id = x.Id,
                PasswordHash = PasswordFunctions.GetSHA256(x.PasswordHash)
            });
        }
        private IEnumerable<Role> GetRoles()
        {
            string content = File.ReadAllText("./roles.json");
            return JsonConvert.DeserializeObject<IEnumerable<Role>>(content);
        }
        private IEnumerable<UserRole> GetUserRoles()
        {
            string content = File.ReadAllText("./userroles.json");
            return JsonConvert.DeserializeObject<IEnumerable<UserRole>>(content);
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserRole>()
                   .HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(p => p.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<PostalCode>().HasData(GetPostalMappingData());
            modelBuilder.Entity<ProgressiveTaxRate>().HasData(GetProgressiveTaxRateData());
            
            modelBuilder.Entity<User>().HasData(GetUsers());
            modelBuilder.Entity<Role>().HasData(GetRoles());
            modelBuilder.Entity<UserRole>().HasData(GetUserRoles());
        }
    }
}
