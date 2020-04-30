using AutoMapper;
using BusLayer.DTO;
using DataLayer.Context;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.Service
{
    public interface ITicketService {
        List<TicketDTO> AllTickets();
        string AddTickets(TicketDTO place, int number);
        string EditTickets(TicketDTO place, int ticketId);
        string RemoveTicket(int id);
    }
    public class TicketService : ITicketService
    {
        private UnitOfWork Unit { get; set; }

        private IMapper mapper = new Mapper(AutomapperConfig.Config);


        public TicketService(UnitOfWork unit) {
            Unit = unit;
        }

        public List<TicketDTO> AllTickets()
        {
            return mapper.Map<List<Place>, List<TicketDTO>>(Unit.ticketFactory.TicketsList().AllTickets().ToList());
        }

        public string AddTickets(TicketDTO place, int number )
        {
            var pas = Unit.userFactory.GetUsers().AllUsers().FirstOrDefault(u=>u.UserName == place.PassName);
            var tr = Unit.routeFactory.RoutesInfo().AllRoutes().FirstOrDefault(t=>t.Number == number);
            string type = tr.Seats.FirstOrDefault(s => s.Type == place.Type).Type;
            var pl = new Place
            {
                Passenger = pas,
                Train = tr,
                TrainNumber = tr.Number,
                Type = type,
                PassName = place.PassName,
                DepStation = place.DepStation,
                ArrivalStation = place.ArrivalStation,
                Departure = place.Departure,
                Arrival = place.Arrival
            };
            return Unit.ticketFactory.CreateTicket().AddTicket(pl, pas, tr);
        }

        public string EditTickets(TicketDTO place, int ticketId)
        {
            var tk = new Place {
                PassengerPlace = place.PassengerPlace,
                Wagon = place.Wagon,
            };
            
            return Unit.ticketFactory.EditTicket().EditTicket(tk, ticketId);
        }

        public string RemoveTicket(int id)
        {
            var tk = Unit.ticketFactory.TicketsList().AllTickets().FirstOrDefault(t=>t.Id==id);
            if (tk != null) {
                Unit.ticketFactory.DeleteTicket().RemoveTicket(tk);
                return null;
            }
            return "Такой билет не был найден";
        }
    }
}
