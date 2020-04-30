using BusLayer;
using BusLayer.DTO;
using BusLayer.Service;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Tick_win.Filters;
using Tick_win.Models;

namespace Tick_win.Controllers
{
    [ExceptionLogAttribute]
    [UserActionLogger]
    [Authorize(Roles = "user, cashier")]
    public class UserController : Controller
    {
        StationService ss;
        UserService us;
        RouteService rs;
        TicketService ts;
        UnitOfWork unit;

        public UserController() { }

        //Инъекция зависимостей в конструктор

        public UserController(UnitOfWork _unit) {
            unit = _unit;
            ts = new TicketService(_unit);
            rs = new RouteService(_unit);
            us = new UserService(_unit);
            ss = new StationService(_unit);
        }

        //Данные пользователя 
        public ActionResult UserIndex()
        {
            try { 
            var model = us.AllUsers().FirstOrDefault(u=>u.UserName== User.Identity.Name);
            var user = new UserViewModel {
                UserId = model.Id,
                Name =model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                MyTickets = model.Tickets.Select(t=> new PlaceViewModel(t.Wagon, t.PassengerPlace, t.Price, t.TrainNumber, t.IsVerified) {
                    Id = t.Id,
                    DepStation = t.DepStation,
                    ArrivalStation = t.ArrivalStation,
                    Departure = t.Departure,
                    Arrival = t.Arrival,
                    Type = t.Type,
                    Wagon = t.Wagon,
                    Place = t.PassengerPlace
                }).ToList()
            };
            return View(user);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Частичное представление со списком с билетами
        public ActionResult PartTickets(string depStat) {
            try { 
            var model = us.AllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name).Tickets.Where(t=>t.DepStation == depStat);
            if (model == null || string.IsNullOrEmpty(depStat)) {
                model = us.AllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name).Tickets;
            }
            var tickets = model.Select(t => new PlaceViewModel(t.Wagon, t.PassengerPlace, t.Price, t.TrainNumber, t.IsVerified)
            {
                Id = t.Id,
                DepStation = t.DepStation,
                ArrivalStation = t.ArrivalStation,
                Departure = t.Departure,
                Arrival = t.Arrival
            }).ToList();
            return PartialView(tickets);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Редактирование пользователя

        [HttpPost]
        public ActionResult UserIndex(UserViewModel model) {
            try { 
            var res = us.AllUsers().FirstOrDefault(u => u.Id == model.UserId);
            if (res != null) {

               var fres = us.EditUser(new UserDTO {
                    Id = model.UserId,
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                });
                if (fres != null) {
                    ModelState.AddModelError(string.Empty, fres);
                }

             

                unit.Save();
                unit.Dispose();

                AuthManager.SignOut();
                Session.Abandon();
                return RedirectToAction("HomePage", "Home");
            }
            ModelState.AddModelError(string.Empty, "Такой пользователь не найден");
            return RedirectToAction("UserIndex");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Окно покупку билета

        public ActionResult PurchaseWindow(int trainId)
        {
            try { 
            var seats = rs.AllRoutes().FirstOrDefault(t => t.Number == trainId).Seats.Where(s=>s.Quantity>0).Select(s => new SeatsViewModel(s.Type, s.Quantity, s.Price)).ToList();
            var train = rs.AllRoutes().FirstOrDefault(t => t.Number == trainId);
            var model = new PlaceViewModel(train.Number, train.Stops.Select(t => t.StopStation).ToList(), seats);
            return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Покупка билета

        [HttpPost]
        public ActionResult MakePurchase(PlaceViewModel tk)
        {
            try { 
            var tr = rs.AllRoutes().FirstOrDefault(t => t.Number == tk.Train).Stops;
            var ticket = new TicketDTO
            {
                PassName = AuthManager.User.Identity.Name,
                Type = tk.Type,
                DepStation = tk.DepStation,
                ArrivalStation = tk.ArrivalStation,
                Departure = tr.FirstOrDefault(s => s.StopStation == tk.DepStation).Departure,
                Arrival = tr.FirstOrDefault(s => s.StopStation == tk.ArrivalStation).Arrival,

            };

            var res = ts.AddTickets(ticket, tk.Train);
            unit.Save();
            unit.Dispose();
            return RedirectToAction("HomePage", "Home");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Удаление билета

        public ActionResult RemoveTicket(int tickId) {
            try { 
           var res = ts.RemoveTicket(tickId);
            if (res != null) {
                ModelState.AddModelError("", res);
            }
            unit.Save();
            unit.Dispose();
            return RedirectToAction("UserIndex");
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