using Microsoft.EntityFrameworkCore;
using Mission.Entities.Entities;

namespace Mission.Entities.context
{
    public class MissionDbContext : DbContext
    {
        public MissionDbContext(DbContextOptions<MissionDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<MissionTheme> MissionTheme { get; set; }
        public DbSet<MissionSkill> MissionSkill { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Missions> Missions { get; set; }  
        public DbSet<MissionApplication> MissionApplications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegancyTimestampBehavior", true);
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FirstName = "Dhaval",
                LastName = "Dangar",
                EmailAddress = "admin@gmail.com",
                UserType = "admin",
                Password = "413311",
                PhoneNumber = "9327665313",
                CreatedDate = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

            modelBuilder.Entity<Country>().HasData(new Country()
            {
               Id = 1,
               CountryName = "India",
            });

            modelBuilder.Entity<City>().HasData(new City()
            {
                Id = 1,
                CityName = "Ahmedabad",
                CountryId = 1
            });
            modelBuilder.Entity<MissionTheme>().HasData(new MissionTheme()
            {
                Id = 1,
                MissionThemeName = "Environment",
            });
            modelBuilder.Entity<MissionSkill>().HasData(new MissionSkill()
            {
                Id = 1,
                MissionSkillName = "Gardening",
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
