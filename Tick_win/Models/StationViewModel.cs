using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tick_win.Models
{
    public class StationViewModel
    {
        public int Id { get;}

        public string StationName { get; }

        public List<string> SelectedStation { get; set; }

        public IEnumerable<SelectListItem> Stations { get; }

        public List<string> TrainRoute { get; }

        public StationViewModel() { }

        public StationViewModel(IEnumerable<string> stations)
        {
            Stations = stations.Select(s=> new SelectListItem { Text = s, Value=s});
        }

        public StationViewModel(int id, string name, List<string> route)
        {

            Id = id;

            StationName = name;

            TrainRoute = route;

        }
    }
}