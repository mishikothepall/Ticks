using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DTO
{
   public class StopoverDTO
    {
        public int Id { get; set; }

        public string StopStation { get; set; }

        public TrainDTO CurrentTrain { get; set; }

        public DateTime Arrival { get; set; }

        public DateTime Departure { get; set; }
    }
}
