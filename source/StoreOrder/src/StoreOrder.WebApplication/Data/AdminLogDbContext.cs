using Microsoft.EntityFrameworkCore;
using StoreOrder.WebApplication.Data.Models.Loging;

namespace StoreOrder.WebApplication.Data
{
    public class AdminLogDbContext : DbContext, IAdminLogDbContext
    {
        public AdminLogDbContext(DbContextOptions<AdminLogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureLogContext(builder);
        }

        private void ConfigureLogContext(ModelBuilder builder)
        {
            builder.Entity<Log>(c =>
            {
                c.ToTable("logs", "logging");
                c.HasKey(x => x.Id);
                c.Property(x => x.Id).ValueGeneratedOnAdd();
                c.Property(x => x.Message).HasColumnType("text").HasColumnName("message");
                c.Property(x => x.MessageTemplate).HasColumnType("text").HasColumnName("message_template");
                c.Property(x => x.Level).HasMaxLength(128).HasColumnName("level");
                c.Property(x => x.TimeStamp).HasColumnName("time_stamp");
                c.Property(x => x.Exception).HasColumnType("text").HasColumnName("exception");
                c.Property(x => x.LogEvent).HasColumnType("jsonb").HasColumnName("log_event");
                c.Property(x => x.Properties).HasColumnType("jsonb").HasColumnName("properties");
            });
        }

        public DbSet<Log> Logs { get; set; }
    }
}
