using DataLayer.Context;
using DataLayer.Models;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Factories
{
    //Поление полного списка билетов

    public interface ITicketsInfo
    {
        IEnumerable<Place> AllTickets();
    }

    class TicketsInfo : ITicketsInfo {
        AppDbContext db = new AppDbContext();

        public IEnumerable<Place> AllTickets()
        {
            return db.Places.Include("Passenger").Include("Train");
        }
    }
    
    //Добавление билета
    public interface IAddTicket {
        string AddTicket(Place place, Account pass, Train train);
    }

    class AddTicket : IAddTicket {
        AppDbContext db;

        public AddTicket(AppDbContext context){
            db = context;
        }

        string IAddTicket.AddTicket(Place place, Account pass, Train train)
        {
            //Получение списка сущностей
            var ps = db.Accounts.Include("Tickets").FirstOrDefault(p=>p.UserName == pass.UserName);
            var tr = db.Trains.Include("Stops").Include("Stations").Include("Seats").FirstOrDefault(t=>t.Id == train.Id);
            var tk = db.Places.FirstOrDefault(p=>p.PassengerPlace == place.PassengerPlace && p.Wagon == place.Wagon);
            var st = tr.Seats.FirstOrDefault(s => s.Type == place.Type).Id;
            var sts = db.Seats.FirstOrDefault(s => s.Id == st);

            if (ps == null)
            {
                return "Пассажир не зарегестрирован";
            }
            else
            if (tr == null) {
                return "Поезд не существует";
            }
            else
           {
                //Создание билета

                place.Passenger = ps;
                place.PassName = ps.UserName;
                place.Train = tr;
                place.TrainNumber = tr.Number;
                place.Price = tr.Seats.FirstOrDefault(s => s.Type == place.Type).Price;
                sts.Quantity=sts.Quantity-1;
                
                //Редактирование соответствующих сущностей

                db.Entry(sts).State = EntityState.Modified;
                db.Entry(ps).State = EntityState.Modified;
                db.Entry(tr).State = EntityState.Modified;

                //Добавление в контекст

                tr.Passengers.Add(place);
                ps.Tickets.Add(place);
                db.Places.Add(place);

                return null;
            }
            
        }
    }

    //Удаление билета

    public interface ITicketRemover {
        void RemoveTicket(Place ticket);
    }

    class TicketRemover : ITicketRemover {
        AppDbContext db;

        public TicketRemover(AppDbContext context) {
            db = context;
        }

        public void RemoveTicket(Place ticket)
        {
            //Поиск соответствующих сущностей

            var t = db.Places.Include("Passenger").Include("Train").FirstOrDefault(tk => tk.Id == ticket.Id);
            var pas = db.Accounts.FirstOrDefault(a => a.Id == t.Passenger.Id);
            var tr = db.Trains.Include("Seats").FirstOrDefault(rt=>rt.Number==t.TrainNumber).Seats;
            var st = tr.FirstOrDefault(s => s.Type == t.Type);

            st.Quantity = st.Quantity+1; 
            pas.Tickets.Remove(t); 

            //Редактирование сущнстей и удаление билета

            db.Entry(pas).State = EntityState.Modified;
            db.Entry(st).State = EntityState.Modified;
            db.Places.Remove(t);

            
        }
    }

    //Редактирование билета

    public interface ITicketEditor {
        string EditTicket(Place place, int id);
    }

    class TicketEditor : ITicketEditor {
        AppDbContext db;

        public TicketEditor(AppDbContext context) {
            db = context;
        }

        public string EditTicket(Place place, int id)
        {
            
          
            var tk = db.Places.FirstOrDefault(p => p.Id == id);

            var ver = db.Trains.FirstOrDefault(t=> t.Number == tk.TrainNumber).
                Passengers.FirstOrDefault(p => p.PassengerPlace == place.PassengerPlace && p.Wagon == place.Wagon);


            if (tk != null && ver != null) {
                return "Такой билет был уже куплен";
            }else
            if (tk != null && ver==null)
            {
         
                tk.IsVerified = true;
                tk.Wagon = place.Wagon;
                tk.PassengerPlace = place.PassengerPlace;
                db.Entry(tk).State = EntityState.Modified;
                return null;
            }
            return "Такой билет не найден";
        }
    }
    public interface ITicketManager {
        ITicketsInfo TicketsList();
        IAddTicket CreateTicket();
        ITicketRemover DeleteTicket();
        ITicketEditor EditTicket();
    }

    public class TicketFactory : ITicketManager
    {
        AppDbContext db;

        public TicketFactory(AppDbContext context) {
            db = context;
        }

        public IAddTicket CreateTicket()
        {
            return new AddTicket(db);
        }

        public ITicketRemover DeleteTicket()
        {
            return new TicketRemover(db);
        }

        public ITicketEditor EditTicket()
        {
            return new TicketEditor(db);
        }

        public ITicketsInfo TicketsList()
        {
            return new TicketsInfo();
        }
    }
}
