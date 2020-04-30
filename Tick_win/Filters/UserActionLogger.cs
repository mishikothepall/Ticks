using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tick_win.Models;

namespace Tick_win.Filters
{
    //Логирование действий пользователя
    public class UserActionLogger : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            string name = "Guest";

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                name = filterContext.HttpContext.User.Identity.Name; //Получение имени пользователя
            }

            LogData data = new LogData
            {
                UserName = name,
                UserAction = request.RawUrl,
                Date = DateTime.Now
            };

            //Вызов класса для записи в XML файл

            XmlFileManager xmlFileManager = new XmlFileManager();
            xmlFileManager.XmlSave(data);

            base.OnActionExecuting(filterContext);
        }
    }
}