using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DTO
{
   public class StationDTO
    {
        public int Id { get; set; }

        public string StationName { get; set; }

        public List<TrainDTO> Trains { get; set; }

        public List<RouteDTO> Routes { get; set; }

        public StationDTO()
        {

            Trains = new List<TrainDTO>();

            Routes = new List<RouteDTO>();

        }
    }
}
