using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class SearchViewModel
    {
        [Required]
        public string DepartureStation { get; set; }
        [Required]
        public string ArrivalStation { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        public List<TrainViewModel> Trains { get; set; }

        public SearchViewModel() { }

        public SearchViewModel(List<TrainViewModel> trains)
        {
            Trains = trains;
        }
    }
}