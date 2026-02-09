using Microsoft.EntityFrameworkCore;
using FactoryAPI.Models;
namespace FactoryAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 這行最關鍵：它告訴系統要在資料庫建立一張叫 Machines 的資料表
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    }
}