using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
   public class Station
    {
        public int Id { get; set; }

        public string StationName { get; set; }

        public List<Train> Trains { get; set; }

        public List<Route> Routes { get; set; }


        public Station()
        {
            Routes = new List<Route>();
            Trains = new List<Train>();

        }
    }
}
