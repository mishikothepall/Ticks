using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Train
    {
        public int Id { get; set; }

        public int Number { get; set; }// Name

        public string DepartureStation { get; set; }//Name

        public string ArrivalStation { get; set; }//Name

        public Route CurrentRoute { get; set; }

        public int Speed { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public List<Seat> Seats { get; set; }

        public List<Station> Stations { get; set; }

        public List<Stopover> Stops { get; set; }//Поиск по нему

        public List<Place> Passengers { get; set; }


        public Train()
        {
            Seats = new List<Seat>();

            Stations = new List<Station>();

            Passengers = new List<Place>();

            Stops = new List<Stopover>();


        }
    }
}
