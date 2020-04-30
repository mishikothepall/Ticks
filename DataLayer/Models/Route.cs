using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Route
    {
        public int Id { get; set; }

        public string RouteName { get; set; }

        public int Distance { get; set; }

        public List<Train> Trains { get; set; }

        public List<Station> Stops { get; set; }

        public Route() {
            Stops = new List<Station>();
            Trains = new List<Train>();
        }
    }
}
