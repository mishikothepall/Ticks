using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Place
    {
        public int Id { get; set; }

        public bool IsVerified { get; set; }

        public int TrainNumber { get; set; }

        public int Wagon { get; set; }

        public int PassengerPlace { get; set; }

        public int Price { get; set; }

        public string PassName { get; set; }

        public string DepStation { get; set; }

        public string ArrivalStation { get; set; }

        public DateTime Departure {get;set;}

        public DateTime Arrival { get; set; }

        public string Type { get; set; }

        public Train Train { get; set; }

        public Account Passenger { get; set; }

        public Place() {
            IsVerified = false;
        }

    }
}
