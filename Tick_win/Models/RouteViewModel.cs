using BusLayer.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tick_win.Models
{
    public class RouteViewModel
    {
        
        public int Number { get; set; }
        
        public string DepartureStation { get; set; }
        
        public string ArrivalStation { get; set; }

        public string SelectedRoute { get; set; }

        public IEnumerable<SelectListItem> CurrentRoute { get; set; }//SelectList

        //public int Speed { get; set; }

        
        public DateTime Departure { get; set; }

        //[Required]
        //public DateTime Arrival { get; set; }

        
        public List<SeatsViewModel> Seats { get; set; }


        
        public List<TransitionalViewModel> Stops { get; set; }//from CurrRoute => List of stop stations 

        public List<SelectListItem> routeSet { get; set; }

        public RouteViewModel() {
        }


        public RouteViewModel(IEnumerable<string> stations )
        {
            CurrentRoute = stations.Select(s => new SelectListItem { Text=s});

            routeSet = stations.Select(s => new SelectListItem { Text = s }).ToList();

            routeSet.Remove(routeSet.FirstOrDefault(r => r.Text == SelectedRoute));
            routeSet.Insert(0, new SelectListItem { Text = SelectedRoute });

            Seats = new List<SeatsViewModel> {
                    new SeatsViewModel { Type = "Купе"},
                    new SeatsViewModel { Type = "Плацкарт"},
                    new SeatsViewModel { Type = "Общий"}
                };
        }

        public RouteViewModel(IEnumerable<string> stations, List<SeatsViewModel> seats)
        {
            CurrentRoute = stations.Select(s => new SelectListItem { Text = s });

            routeSet = stations.Select(s => new SelectListItem { Text = s }).ToList();

            routeSet.Remove(routeSet.FirstOrDefault(r => r.Text == SelectedRoute));
            routeSet.Insert(0, new SelectListItem { Text = SelectedRoute });

            Seats = NoSeats(seats);
        }

        private List<SeatsViewModel> NoSeats(List<SeatsViewModel> mod) {
            if (mod.Count <= 0) {

                mod = new List<SeatsViewModel> {
                    new SeatsViewModel { Type = "Купе"},
                    new SeatsViewModel { Type = "Плацкарт"},
                    new SeatsViewModel { Type = "Общий"}
                };
                
            }
            return mod;
        }
    }
}