using BIZFEST_Event.Models;
using Microsoft.EntityFrameworkCore;

namespace BIZFEST_Event.DataAccess
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<UserEvent> UserEvent { get; set;}
        public DbSet<UsersRegistration> UserRegistration { get; set;}
        public DbSet<AdminLogin> AdminLogin { get; set;}
        public DbSet<Cities> Cities { get; set;}
        public DbSet<States> States { get; set;}
        public DbSet<CategoryMaster> CategoryMaster { get; set;}
        public DbSet<ResponsePayment> ResponsePayment { get; set;}
        public DbSet<CustomFields> CustomFields { get; set; }
        public DbSet<DynamicFields> DynamicFields { get; set; }

        public DbSet<EventCustomForm> EventCustomForm { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<CustomFieldType>().HasNoKey();

        //    modelBuilder.Entity<CustomFields>()
        //        .HasMany(c => c.Types)
        //        .WithOne(t => t.CustomFields)
        //        .HasForeignKey(t => t.CustomFieldsId);

        //    base.OnModelCreating(modelBuilder);
        //}

    }
}
