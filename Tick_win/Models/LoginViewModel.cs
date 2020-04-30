using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tick_win.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "!!Имя прользователя не указано.!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "!!Пароль не указан.!!")]
        public string Password { get; set; }

    }
}