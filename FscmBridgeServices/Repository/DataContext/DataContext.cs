
using FscmBridgeServices.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace FscmBridgeServices.Repository.DataContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt) { }
        public DbSet<OrganizationBuyer> organizationBuyer { get; set; }
        public DbSet<FinanceOrganization> financeOrganization { get; set; }
        public DbSet<FscmLog> fscmLogs { get; set; }
        public DbSet<FscmContract> fscmContracts { get; set; }
        public DbSet<Funder> funders { get; set; }
        public DbSet<Buyer> buyer { get; set; }
        public DbSet<Seller> seller { get; set; }
        public DbSet<OptionRate> optionRate { get; set; }
        public DbSet<Suspension> suspensions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrganizationBuyer>().HasNoKey();
            modelBuilder.Entity<FinanceOrganization>().HasNoKey();
            modelBuilder.Entity<FscmContract>().HasNoKey();
            modelBuilder.Entity<Funder>().HasNoKey();
            modelBuilder.Entity<Buyer>().HasNoKey();
            modelBuilder.Entity<Seller>().HasNoKey();
            modelBuilder.Entity<OptionRate>().HasNoKey();
            modelBuilder.Entity<Suspension>().HasNoKey();
        }
    }
}
