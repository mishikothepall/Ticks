using DataLayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext() : base("name=TicketsDb") { }

        static AppDbContext()
        {
            Database.SetInitializer<AppDbContext>(new AppDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(a=>a.Id);
            modelBuilder.Entity<Station>().HasKey(s => s.Id);
            modelBuilder.Entity<Train>().HasKey(t => t.Id);
            modelBuilder.Entity<Seat>().HasKey(p => p.Id);
            modelBuilder.Entity<Stopover>().HasKey(so => so.Id);
            modelBuilder.Entity<Route>().HasKey(r=>r.Id);
            

            modelBuilder.Entity<Train>().Property(t => t.Departure).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Train>().Property(t => t.Arrival).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Stopover>().Property(s => s.Departure).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Stopover>().Property(s => s.Arrival).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Place>().Property(s => s.Departure).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Place>().Property(s => s.Arrival).HasColumnType("datetime2").HasPrecision(0);

            modelBuilder.Entity<Train>().HasMany(t => t.Seats).WithRequired(s => s.Train);

            modelBuilder.Entity<Train>().HasMany(t => t.Stops).WithRequired(so => so.CurrentTrain);

            modelBuilder.Entity<Route>().HasMany(r => r.Trains).WithRequired(t=>t.CurrentRoute);

            modelBuilder.Entity<Route>().HasMany(r => r.Stops).WithMany(s => s.Routes).Map(rs=> 
            {
                rs.MapLeftKey("RouteStationId");
                rs.MapRightKey("StationRouteId");
                rs.ToTable("RouteStation");

            });

            modelBuilder.Entity<Account>().HasMany(u => u.Tickets).WithRequired(t => t.Passenger);
            modelBuilder.Entity<Train>().HasMany(p => p.Passengers).WithRequired(t=>t.Train);

            modelBuilder.Entity<Train>().HasMany(t => t.Stations).WithMany(s => s.Trains).Map(st => {
                st.MapLeftKey("TrainStationId");
                st.MapRightKey("StationTrainId");
                st.ToTable("TrainStation");
            });

           

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Stopover> Stopovers { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Place> Places { get; set; }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}
