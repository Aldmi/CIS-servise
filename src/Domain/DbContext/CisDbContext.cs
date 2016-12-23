using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Domain.Entities;

namespace Domain.DbContext
{
    public class CisDbContext : System.Data.Entity.DbContext
    {
        public CisDbContext() : base("CisDbContext")  //"CisDbContext"
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<CisDbContext>());
        }




        #region Reps

        public DbSet<Station> Stations { get; set; }

        public DbSet<OperativeSchedule> OperativeSchedules { get; set; }

        public DbSet<RegulatorySchedule> RegulatorySchedules { get; set; }

        public DbSet<RailwayStation> RailwayStations { get; set; }

        public DbSet<Info> Infos { get; set; }

        public DbSet<Diagnostic> Diagnostics { get; set; }

        #endregion




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
           .HasMany(c => c.OperativeSchedulesListOfStops)
           .WithMany(p => p.ListOfStops)
           .Map(m =>
              {
                  // Ссылка на промежуточную таблицу
                  m.ToTable("OperativeSchedulesListOfStops");

                  // Настройка внешних ключей промежуточной таблицы
                  m.MapLeftKey("StationId");
                  m.MapRightKey("OperativeSchedulesId");
              });


            modelBuilder.Entity<Station>()
           .HasMany(c => c.OperativeSchedulesListWithoutStops)
           .WithMany(p => p.ListWithoutStops)
           .Map(m =>
           {
               // Ссылка на промежуточную таблицу
               m.ToTable("OperativeSchedulesListWithoutStops");

               // Настройка внешних ключей промежуточной таблицы
               m.MapLeftKey("StationId");
               m.MapRightKey("OperativeSchedulesId");
           });


            modelBuilder.Entity<Station>()
           .HasMany(c => c.RegulatorySchedulesListOfStops)
           .WithMany(p => p.ListOfStops)
           .Map(m =>
           {
               // Ссылка на промежуточную таблицу
               m.ToTable("RegulatorySchedulesListOfStops");

               // Настройка внешних ключей промежуточной таблицы
               m.MapLeftKey("StationId");
               m.MapRightKey("RegulatorySchedulesId");
           });


            modelBuilder.Entity<Station>()
           .HasMany(c => c.RegulatorySchedulesListWithoutStops)
           .WithMany(p => p.ListWithoutStops)
           .Map(m =>
           {
               // Ссылка на промежуточную таблицу
               m.ToTable("RegulatorySchedulesListWithoutStops");

               // Настройка внешних ключей промежуточной таблицы
               m.MapLeftKey("StationId");
               m.MapRightKey("RegulatorySchedulesId");
           });
        }
    }
}