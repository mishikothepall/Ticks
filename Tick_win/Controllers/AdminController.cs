using BusLayer;
using BusLayer.DTO;
using BusLayer.Service;
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
    [Authorize(Roles = "admin")]

    public class AdminController : Controller
    {
        RouteService rs;
        StationService ss;
        UserService us;
        UnitOfWork Unit;
        XmlFileManager xmlData = new XmlFileManager();

        public AdminController() { }

        //Инъекция зависимостей в конструктор
        public AdminController(UnitOfWork unit)
        {
            rs = new RouteService(unit);
            ss = new StationService(unit);
            us = new UserService(unit);
            Unit = unit;
        }

        //Список поездов

        public ActionResult AvailableTrains() {
            try
            {
                var routes = rs.AllRoutes();

                //Временная переменная для инициализации частичного представления
                TempData["Trains"] = routes.Select(r => new TrainViewModel
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
                        )).OrderBy(d => d.Departure).ToList(),
                    r.Seats.Select(s => new SeatsViewModel(
                        s.Type,
                        s.Quantity,
                        s.Price
                        )).ToList(),
                    r.Passengers.Select(p => new PlaceViewModel(p.Id, p.Wagon, p.PassengerPlace, p.Price, p.TrainNumber, p.PassName, p.Type)).ToList(),
                    r.CurrentRoute.Distance
                )

                ).ToList();

                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Частичное представление со списком поездов

        [HttpPost]
        public ActionResult PartTrains(SearchViewModel svm)
        {
            try
            {
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

        //Форма для редактирования поезда

            public ActionResult EditTrain( int trainNumber)
        {
            try
            {
                var data = rs.AllRoutes().FirstOrDefault(t => t.Number == trainNumber);

                var model = new RouteViewModel(rs.RouteScaf().Select(r => r.RouteName), data.Seats.Select(s => new SeatsViewModel(s.Type, s.Quantity, s.Price)).ToList())
                {
                    Number = data.Number,
                    DepartureStation = data.DepartureStation,
                    ArrivalStation = data.ArrivalStation,
                    Departure = data.Departure,
                    SelectedRoute = data.CurrentRoute.RouteName,
                    Stops = data.Stops.Select(s => new TransitionalViewModel(s.StopStation, s.Departure, s.Arrival)).ToList(),
                    Seats = data.Seats.Select(s => new SeatsViewModel(s.Type, s.Quantity, s.Price)).ToList(),

                };

                return View(model);
            }
            catch (Exception ex) {
                return View(ex.Message);
            }
        }

        //Редактирование поезда
        [HttpPost]
        public ActionResult ChangeRoute(RouteViewModel model, List<TransitionalViewModel> modelItem) {
            try
            {
                var train = new TrainDTO
                {
                    Number = model.Number,
                    DepartureStation = model.DepartureStation,
                    ArrivalStation = model.ArrivalStation,
                    Departure = model.Departure,
                    Seats = model.Seats.Select(s => new SeatDTO
                    {
                        Type = s.Type,
                        Quantity = s.Quantity,
                        Price = s.Price
                    }).ToList(),
                    Stops = modelItem.Select(m => new StopoverDTO
                    {
                        StopStation = m.StopStation,
                        Departure = m.Departure,
                        Arrival = m.Arrival

                    }).ToList()

                };

                rs.EditRoute(train, train.Stops, model.SelectedRoute);
                Unit.Save();
                Unit.Dispose();
                return RedirectToAction("AvailableTrains");
                }
                catch (Exception ex) {
                    return View(ex.Message);
                }
        }
        [HttpPost]
        public ActionResult InitStations(string routeName, int train)
        {
            try
            {
                var mod = rs.AllRoutes().FirstOrDefault(t => t.Number == train);
                List<TransitionalViewModel> ls = mod.Stops.Select(s => new TransitionalViewModel(s.StopStation)
                {
                    Departure = s.Departure,
                    Arrival = s.Arrival
                }).OrderBy(d => d.Departure).ToList();

                return PartialView(ls);
            }
            catch (Exception ex) {
                return View(ex.Message);
            }
        }

        //Форма для создания маршрута

        public ActionResult CreatePath() {
            try
            {
                var sts = ss.AllStations().ToList();
                var model = new PathViewModel(sts.Select(s => s.StationName));
                TempData["PathList"] = rs.RouteScaf().Select(p => new PathViewModel { RouteName = p.RouteName, Distance = p.Distance }).ToList();
                return View(model);
            }
            catch (Exception ex){
                return View(ex.Message);
            }
        }

        //Создание маршрута
        [HttpPost]
        public ActionResult PathAdd(PathViewModel model)
        {
            try { 
            var rt = new RouteDTO
            {
                RouteName = model.RouteName,
                Distance = model.Distance,
                Stops = model.Stops.Select(s => new StationDTO { StationName = s}).ToList()
            };
            rs.AddPath(rt);
            Unit.Save();
            Unit.Dispose();
            var data = rs.RouteScaf().Select(p => new PathViewModel { RouteName = p.RouteName, Distance = p.Distance }).ToList();
            return PartialView(data);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Форма для редактирования маршрута
        public ActionResult EditPath(string pathName) {
            try { 
            var data = rs.RouteScaf().FirstOrDefault(p=>p.RouteName == pathName);
            var model = new PathViewModel(data.Stops.Select(s => s.StationName))
            {
                OldName = data.RouteName,
                RouteName = data.RouteName,
                Distance = data.Distance,
                Stops = data.Stops.Select(s=>s.StationName).ToList()
            };
            return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Редактирование маршрута
        [HttpPost]
        public ActionResult EditPath(PathViewModel model)
        {
            try { 
            var rt = new RouteDTO
            {
                RouteName = model.RouteName,
                Distance = model.Distance,
            };
            rs.EditPath(rt, model.OldName);
            Unit.Save();
            Unit.Dispose();
            var data = rs.RouteScaf().Select(p => new PathViewModel { RouteName = p.RouteName, Distance = p.Distance }).ToList();
            return RedirectToAction("CreatePath");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        
        //Форма для создания поезда

        public ActionResult CreateRoute()
        {
            try {
               
            var routes = rs.AllRoutes();
            var mod = rs.RouteScaf();
            var ls = mod.ToList();
            var data = new RouteViewModel(mod.Select(m => m.RouteName));
            
         
            return View(data);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Создание поезда

        [HttpPost]
        public ActionResult CreateRoute(RouteViewModel model, List<TransitionalViewModel> modelItem)
        {
            try { 
            var train = new TrainDTO
            {
                Number = model.Number,
                DepartureStation = model.DepartureStation,
                ArrivalStation = model.ArrivalStation,
                Departure = model.Departure,
                Seats = model.Seats.Select(s=> new SeatDTO {
                    Type = s.Type,
                    Quantity = s.Quantity,
                    Price = s.Price
                }).Where(q=>q.Quantity>0).ToList()
            };
            var stopovers = modelItem.Select(m=> new StopoverDTO {
                StopStation =  m.StopStation,
                Departure = m.Departure,
                Arrival = m.Arrival

            }).ToList();
            rs.AddRoute(train, stopovers, model.SelectedRoute);
            Unit.Save();
            Unit.Dispose();
            
            return RedirectToAction("AvailableTrains");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }


        //Список остановок

        [HttpPost]
        public ActionResult StopList(string routeName)
        {
            try { 
            var mod = rs.RouteScaf().FirstOrDefault(s=>s.RouteName==routeName);
            List<TransitionalViewModel> ls = mod.Stops.Select(s => new TransitionalViewModel(s.StationName)).ToList();

            return PartialView(ls);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        //Список пользователей
        public ActionResult AdminIndex() {
            try { 
            var data = us.AllUsers();

            var mod = data.Select(d=>new UserViewModel {
                UserId = d.Id,
                Name =d.UserName,
                Email =d.Email,
                PhoneNumber = d.PhoneNumber
            }).ToList() ;
            TempData["Users"] = mod;
            return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult UsersPartial(UserViewModel user) {
            try { 
            var res = us.AllUsers().Where(u=>u.UserName==user.Name).ToList();
            var mod = new List<UserViewModel>();
            if (res.Count() <= 0)
            {
                res = us.AllUsers();
            }
           mod = res.Select(d=>new UserViewModel {
                UserId = d.Id,
                Name = d.UserName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber
            }).ToList();

           
            return PartialView(mod);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Создание кассира
        public ActionResult CreateCashier(string userId) {
            try { 
            var res = us.CreateCashier(userId);
            if (res != null) {
                ModelState.AddModelError("", res);
            }
            return RedirectToAction("AdminIndex");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        
        //Создание станции
        [HttpPost]
        public ActionResult CreateStation(PathViewModel model) {
            try { 
            ss.CreateStation(model.Station);
            Unit.Save();
            Unit.Dispose();
            return RedirectToAction("CreatePath");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
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