using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class ExViewModel
    {
        public string UserName { get; set; }
        public string ExMessage { get; set; }
        public string ControllerName { get; set; }
        public string ExTrace { get; set; }
        [DataType(DataType.Date)]
        public DateTime LogTime { get; set; }
    }
}