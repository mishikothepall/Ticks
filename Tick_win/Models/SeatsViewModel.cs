using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class SeatsViewModel
    {
        public string Type { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public SeatsViewModel() { }

        public SeatsViewModel(string type, int quantity, int price) {

            Type = type;

            Quantity = quantity;

            Price = price;

        }
    }
}