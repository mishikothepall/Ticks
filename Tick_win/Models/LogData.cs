using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class LogData
    {
        public string UserName { get; set; }

        public string UserAction { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

    }
}