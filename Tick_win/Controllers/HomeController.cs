using BusLayer;
using BusLayer.DTO;
using BusLayer.Service;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tick_win.Filters;
using Tick_win.Models;

namespace Tick_win.Controllers
{

    [ExceptionLogAttribute]
    [UserActionLogger]
    public class HomeController : Controller
    {
        StationService ss;
        UserService us;
        RouteService rs;
        TicketService ts;
        UnitOfWork unit;


        public HomeController() { }
        
        //Инъекция зависимостей в конструктор

        public HomeController(UnitOfWork _unit)
        {
            unit = _unit;
            ts = new TicketService(_unit);
            rs = new RouteService(_unit);
            us = new UserService(_unit);
            ss = new StationService(_unit);
        }


        public ActionResult HomePage() {
            try { 
            var routes = rs.AllRoutes();

            var trains = routes.Select(r => new TrainViewModel
            (
                r.Number,
                r.DepartureStation,
                r.ArrivalStation,
                r.Arrival,
                r.Departure,
                r.Stops.Select(t => new TransitionalViewModel(
                    t.StopStation,
                    t.Arrival,
                    t.Departure
                    )).ToList(),
                r.Seats.Select(s => new SeatsViewModel(
                    s.Type,
                    s.Quantity,
                    s.Price
                    )).ToList(),
                 r.Passengers.Select(p => new PlaceViewModel(p.Id, p.Wagon, p.PassengerPlace, p.Price, p.TrainNumber, p.PassName, p.Type)).ToList(),
                r.CurrentRoute.Distance
            )

            ).ToList();

            var model = new SearchViewModel { Trains = trains };

            return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Поиск поездов

        public ActionResult HomePartial(SearchViewModel svm) {
            try { 
            List<TrainViewModel> model = new List<TrainViewModel>();
            var res = rs.RouteFilter(svm.DepartureStation, svm.ArrivalStation, svm.DepartureDate).ToList();
            if (res.Count() <= 0)
            {
                res = rs.AllRoutes();

                ModelState.AddModelError(string.Empty, "По Вашему запросу ничего не найдено.");
            }

            model = res.Select(r => new TrainViewModel
            (
                r.Number,
                r.DepartureStation,
                r.ArrivalStation,
                r.Arrival,
                r.Departure,
                r.Stops.Select(t => new TransitionalViewModel(
                    t.StopStation,
                    t.Arrival,
                    t.Departure
                    )).ToList(),
                r.Seats.Select(s => new SeatsViewModel(
                    s.Type,
                    s.Quantity,
                    s.Price
                    )).ToList(),
                r.Passengers.Select(p => new PlaceViewModel(p.Id, p.Wagon, p.PassengerPlace, p.Price, p.TrainNumber, p.PassName, p.Type)).ToList(),
                r.CurrentRoute.Distance
            )).ToList();

            return PartialView(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Выход из системы

        public ActionResult LogOut()
        {
            try { 
            AuthManager.SignOut();
            Session.Abandon();
            return RedirectToAction("HomePage");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Форма для входа в систему

        public ActionResult Login(string returnUrl)
        {
            try { 
            TempData["returnUrl"] = returnUrl;
            return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Вход в систему

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try { 
            var isCreated = us.LoginUser(model.UserName, AuthManager, model.Password);

            if (isCreated.Equals(string.Empty))
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("HomePage");
                }
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", isCreated);
            }

            return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }

        //Форма для регистрации пользователя

        public ActionResult Create() {
            return View();
        }

        //Регистрация пользователя

        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {

            if (ModelState.IsValid)
            {
                try { 
                UserDTO user = new UserDTO { UserName = model.Name, Email = model.Email };
                var res = us.CreateUser(user, model.Password, AuthManager);
                if (res == null)
                {
                    unit.Save();
                    return RedirectToAction("HomePage");
                }
                else
                {
                    AddErrorsFromResult(res);
                }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }

            return View(model);
        }

        private void AddErrorsFromResult(IEnumerable<string> result)
        {
            foreach (string error in result)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

 
    }
}