using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    
   public class AppDbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            var hk = context.Stations.Add(new Station { StationName = "Харьков" });
            var ky = context.Stations.Add(new Station { StationName = "Киев" });
            var lv = context.Stations.Add(new Station { StationName = "Львов" });
            var zd = context.Stations.Add(new Station { StationName = "Здолбунов" });
            var pen = context.Stations.Add(new Station { StationName = "Малые Пеньки"});
            context.SaveChanges();

            var khlv = context.Routes.Add(new Route { RouteName = "Харьков - Львов", Stops = new List<Station> {hk, ky, zd, lv }, Distance = 1017 });
            hk.Routes.Add(khlv);
            ky.Routes.Add(khlv);
            zd.Routes.Add(khlv);
            lv.Routes.Add(khlv);
            context.SaveChanges();
            var kyzd = context.Routes.Add(new Route { RouteName = "Киев - Здолбунов", Stops = new List<Station> { ky, pen,zd }, Distance = 500 });
            ky.Routes.Add(kyzd);
            pen.Routes.Add(kyzd);
            zd.Routes.Add(kyzd);
            context.SaveChanges();

            var pc = context.Seats.Add(new Seat { Type = "плацкарт", Quantity = 25, Price = 100 });
            var coup = context.Seats.Add(new Seat { Type = "купе", Quantity = 15, Price = 300 });

            var train = context.Trains.Add(new Train
            {
                Number = 56,
                DepartureStation = context.Stations.FirstOrDefault(t => t.Id == 1).StationName,
                ArrivalStation = context.Stations.FirstOrDefault(t => t.Id == 3).StationName,
                Departure = new DateTime(2020, 03, 05, 14, 10, 00),
                Arrival = new DateTime(2020, 03, 07, 18, 30, 00),
                CurrentRoute = khlv,
                Speed = 30,
                Stations = khlv.Stops,
                Seats = new List<Seat> { pc, coup}
            });

            pc.Train = train;
            coup.Train = train;
           


            hk.Trains.Add(train);
            ky.Trains.Add(train);
            zd.Trains.Add(train);
            lv.Trains.Add(train);

           

            khlv.Trains.Add(train);
            khlv.Stops.Add(ky);
            khlv.Stops.Add(zd);

            

            train.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = hk.StationName,
                Departure = new DateTime(2020, 03, 05, 14, 10, 00),
                Arrival= new DateTime(2020, 03, 05, 14, 10, 00)
            }));

            train.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = ky.StationName,
                Departure = new DateTime(2020, 03, 06, 12, 10, 00),
                Arrival = new DateTime(2020, 03, 06, 12, 30, 00),
            }));

            train.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = zd.StationName,
                Departure = new DateTime(2020, 03, 06, 15, 10, 00),
                Arrival = new DateTime(2020, 03, 06, 15, 30, 00),
            }));

            train.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = lv.StationName,
                Arrival = new DateTime(2020, 03, 07, 18, 30, 00),
                Departure= new DateTime(2020, 03, 07, 18, 30, 00)
            }));

            context.SaveChanges();

            var pc2 = context.Seats.Add(new Seat { Type = "плацкарт", Quantity = 25, Price=50 });
            var coup2 = context.Seats.Add(new Seat { Type = "купе", Quantity = 15 });

            var train2 = context.Trains.Add(new Train
            {
                Number = 46,
                DepartureStation = context.Stations.FirstOrDefault(t => t.Id == 2).StationName,
                ArrivalStation = context.Stations.FirstOrDefault(t => t.Id == 4).StationName,
                Departure = new DateTime(2020, 04, 05, 14, 10, 00),
                Arrival = new DateTime(2020, 04, 07, 18, 30, 00),
                CurrentRoute = kyzd,
                Speed = 30,
                Stations = kyzd.Stops,
                Seats = new List<Seat> { pc2, coup2 }

            });

            pc2.Train = train2;
            coup2.Train = train2;


            ky.Trains.Add(train2);
            pen.Trains.Add(train2);
            zd.Trains.Add(train2);
            kyzd.Trains.Add(train2);

            train2.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = ky.StationName,
                Departure = new DateTime(2020, 04, 05, 16, 10, 00),
                Arrival = new DateTime(2020, 04, 05, 16, 30, 00),
            }));
            train2.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = pen.StationName,
                Departure = new DateTime(2020, 04, 05, 16, 10, 00),
                Arrival = new DateTime(2020, 04, 05, 16, 30, 00),
            }));
            train2.Stops.Add(context.Stopovers.Add(new Stopover
            {
                StopStation = zd.StationName,
                Departure = new DateTime(2020, 04, 05, 16, 10, 00),
                Arrival = new DateTime(2020, 04, 05, 16, 30, 00),
            }));
            var acc = InitData(context);
            var ticket = context.Places.Add(new Place {
                DepStation = train2.DepartureStation,
                ArrivalStation = train2.ArrivalStation,
                Departure = train2.Departure,
                Arrival = train2.Arrival,
                PassengerPlace = 32,
                Price = pc2.Price,
                Train = train2,
                Wagon =6,
                PassName = acc.UserName,
                TrainNumber = train2.Number,
                Type = pc2.Type});
            
            acc.Tickets.Add(ticket);
            ticket.Passenger = acc;
            train2.Passengers.Add(ticket);
            pc2.Quantity -= 1;
           
            context.SaveChanges();

            

            base.Seed(context);
        }
        private Account InitData(AppDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<UserRoles>(context));

            string userName = "Admin";
            string password = "mypassword";
            string role = "admin";
            string email = "admin@mail.com";

            roleMgr.Create(new UserRoles("cashier"));
            roleMgr.Create(new UserRoles("admin"));
            roleMgr.Create(new UserRoles("user"));

            AppUser user = userMgr.FindByName(userName);

            if (!roleMgr.RoleExists(role))
            {
                roleMgr.Create(new UserRoles(role));
            }

            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email, PhoneNumber = "623362" },
                    password);
                user = userMgr.FindByName(userName);
                
            }


            if (!userMgr.IsInRole(user.Id, role))
            {

                userMgr.AddToRole(user.Id, role);
            }
            var account = context.Accounts.Add(new Account {
                    Id = user.Id,
                    UserName= user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    
                });
                
              
            return account;
        }
    }
}
