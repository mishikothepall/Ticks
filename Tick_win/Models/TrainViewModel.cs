using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class TrainViewModel
    {
        public int Number { get; set; }

        public string DepartureStation { get; set; }

        public string ArrivalStation { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public string Total { get; set; }

        public int SeatsTotalQuantity { get; set; }

        public List<TransitionalViewModel> Transitional { get; set; }

        public List<SeatsViewModel> Seats { get; set; }

        public List<PlaceViewModel> Passengers { get; set; }

        public TrainViewModel() { }

        public TrainViewModel(int number, string depStat, string arrivalStat, DateTime arrival, DateTime departure,
            List<TransitionalViewModel> trans, List<SeatsViewModel> seats, List<PlaceViewModel> pass, int dist)
        {
            Number = number;

            DepartureStation = depStat;

            ArrivalStation = arrivalStat;

            Arrival = departure.AddHours(dist / 30).AddMinutes(AddPause(trans));

            Departure = departure;

            Transitional = trans;

            Passengers = pass;

            Seats = seats;

            seats.ForEach(s=> SeatsTotalQuantity +=s.Quantity);

            Total = $"{dist/30} часов { AddPause(trans)} минут";

           
        }

        private double AddPause(List<TransitionalViewModel> tr) {
            double ttl = 0;
            foreach (var t in tr) {

                ttl += t.Pause.TotalMinutes;
            }

            return ttl;
        }
    }
}