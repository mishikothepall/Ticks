using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tick_win.Models;

namespace Tick_win.Filters
{
    //Логирование исключений пользователя
    public class ExceptionLogAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string name = "Guest";

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                name = filterContext.HttpContext.User.Identity.Name; //Получение имени пользователя
            }
            if (!filterContext.ExceptionHandled)
            {
                ExViewModel logger = new ExViewModel()
                {
                    UserName = name,
                    ExMessage = filterContext.Exception.Message,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    ExTrace = filterContext.Exception.StackTrace,
                    LogTime = DateTime.Now
                };

                //Вызов класса для записи в XML файл
                XmlFileManager manager = new XmlFileManager();
                manager.XmlSave(logger);
              
            }
        }
    }
}