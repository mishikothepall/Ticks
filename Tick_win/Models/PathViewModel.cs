using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tick_win.Models
{
    public class PathViewModel
    {
        public string OldName { get; set; }

        public string RouteName { get; set; }

        public int Distance { get; set; }

        public List<TrainViewModel> Trains { get; set; }

        public List<string> Stops { get; set; }

        public IEnumerable<SelectListItem> Stations { get; }

        public string Station { get; set; }

        public PathViewModel() { }

        public PathViewModel(IEnumerable<string> stations)
        {
            Stations = stations.Select(s => new SelectListItem { Text = s, Value = s });
        }
    }
}