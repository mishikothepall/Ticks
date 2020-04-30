using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DTO
{
    public class RouteDTO
    {
        public int Id { get; set; }

        public string RouteName { get; set; }

        public int Distance { get; set; }

        public List<TrainDTO> Trains { get; set; }

        public List<StationDTO> Stops { get; set; }

        public RouteDTO()
        {
            Stops = new List<StationDTO>();
        }
    }
}
