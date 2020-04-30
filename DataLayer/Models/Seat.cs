using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
   public class Seat
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public Train Train { get; set; }

    }
}
