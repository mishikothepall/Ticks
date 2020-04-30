using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class TransitionalViewModel
    {
        public string StopStation { get; set; }

        public int Position = 0;

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public TimeSpan Pause { get; set; }

        public TransitionalViewModel() {
            Position++;
        }

        public TransitionalViewModel(string station)
        {
            StopStation = station;

        }

        public TransitionalViewModel(string station, DateTime arrival, DateTime departure)
        {
            StopStation = station;
            Arrival = arrival;
            Departure = departure;
            Pause = arrival.Subtract(departure);

        }
    }
}