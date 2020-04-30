using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tick_win.Models
{
    public class PlaceViewModel
    {
        public int Id { get; set; }

        public int Wagon { get; set; }

        public int Place { get; set; }

        public int Price { get; set; }

        public int Train { get; set; }

        public string Status { get; }

        [Required(ErrorMessage = "Станция отправления не указана.")]
        public string DepStation { get; set; }

        [Required(ErrorMessage = "Станция прибытия не указана.")]
        public string ArrivalStation { get; set; }

        public DateTime Departure { get; set; } 

        public DateTime Arrival { get; set; }

        public IEnumerable<SelectListItem> Departures { get; set; } 

        public IEnumerable<SelectListItem> Arrivals { get; set; } 

        [Required(ErrorMessage = "Место не выбрано.")]
        public string Type { get; set; }

        public List<SeatsViewModel> Seats { get; set; }

        public string Passenger { get; set; }

        public PlaceViewModel() { }

        public PlaceViewModel(int train, List<string> stations, List<SeatsViewModel> seats){
            Train = train;
            stations.Insert(0, string.Empty);
            Departures = stations.Select(d=>new SelectListItem { Text=d});
            Arrivals = stations.Select(a=> new SelectListItem { Text = a});
            Seats = seats;
        }

        public PlaceViewModel(int wagon, int place, int price, int train, bool isVerified) {
            Wagon = wagon;

            Place = place;

            Price = price;

            Train = train;

            Status = IsVerified(isVerified);

        }

        public PlaceViewModel(int id, int wagon, int place, int price, int train, string passenger, string type)
        {
            Id = id;

            Wagon = wagon;

            Place = place;

            Price = price;

            Train = train;

            Passenger = passenger;

            Type = type;
        }

        private string IsVerified(bool stat) {
            if (stat == false) {
                return "Ждет подтверждения у кассира.";
            } else {
                return "Подтвержден.";
            }
        }
    }
}