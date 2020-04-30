using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DTO
{
    public class TrainDTO
    {
        public int Id { get; set; }

        public int Number { get; set; }// Name

        public string DepartureStation { get; set; }//Name

        public string ArrivalStation { get; set; }//Name

        public RouteDTO CurrentRoute { get; set; }

        public int Speed { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public List<SeatDTO> Seats { get; set; }

        public List<StationDTO> Stations { get; set; }

        public List<StopoverDTO> Stops { get; set; }//Поиск по нему

        public List<TicketDTO> Passengers { get; set; }


        public TrainDTO()
        {
            Seats = new List<SeatDTO>();

            Stations = new List<StationDTO>();

            Passengers = new List<TicketDTO>();

            Stops = new List<StopoverDTO>();


        }
    }
}
