using FscmBridgeServices.DTO;
using FscmBridgeServices.DTOS;
using FscmBridgeServices.Models;
using Microsoft.EntityFrameworkCore;

namespace FscmBridgeServices.Util
{
    public class DatabaseContext : DbContext
    {
       
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DatabaseContext() : base(new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer(GetConfig.AppSetting["ConnectionStrings:UrlDatabase"]) 
            .Options)
        {
        }

        public DbSet<ENUMMODULEPARAM> Enummoduleparams { get; set; }
        public DbSet<FscmLog> FscmLogs { get; set; }
        public DbSet<FscmOrganizationBuyer>  FscmOrganizationBuyer { get; set; }
        public DbSet<FscmFinanceOrganization> FscmFinanceOrganizations { get; set; }
    }
}