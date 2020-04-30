using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class PlaceVerificationModel
    {
        public int Id {get; set;}

        public int TrainNumber { get; set; }

        [Required(ErrorMessage ="Укажите вагон")]
        public int Wagon { get; set; }

        [Required(ErrorMessage ="Укажите номер места")]
        public int Place { get; set; }

        public PlaceVerificationModel() { }

        public PlaceVerificationModel(int id, int wagon, int place, int train) {

            Id = id;

            Wagon = wagon;

            Place = place;

            TrainNumber = train;

        }
    }
}