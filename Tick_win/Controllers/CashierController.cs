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
    [Authorize(Roles = "cashier")]
    public class CashierController : Controller
    {
        StationService ss;
        UserService us;
        RouteService rs;
        TicketService ts;
        UnitOfWork unit;

        public CashierController() { }

        //Инъекция зависимостей в конструктор

        public CashierController(UnitOfWork _unit) {
            unit = _unit;
            ts = new TicketService(_unit);
            rs = new RouteService(_unit);
            us = new UserService(_unit);
            ss = new StationService(_unit);
        }

        //Список пользователей

        public ActionResult UserList()
        {
            try { 
            TempData["Users"] = us.AllUsers().Select(u => new UserViewModel
            {
                UserId = u.Id,
                Name = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                MyTickets = u.Tickets.Where(t => t.IsVerified == false).Select(p => new PlaceViewModel(

                        p.Id,
                        p.Wagon,
                        p.PassengerPlace,
                        p.Price,
                        p.TrainNumber,
                        p.PassName,
                        p.Type

                        )).ToList()
            }).ToList();
            return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult ULPartial(UserViewModel user) {
            try { 
            var data = us.AllUsers().Where(u => u.UserName == user.Name).ToList();
            var mod = new List<UserViewModel>();
            if (data.Count() <= 0) {

                data = us.AllUsers();
            }
            mod = data.Select(u => new UserViewModel
            {
                UserId = u.Id,
                Name = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                MyTickets = u.Tickets.Where(t => t.IsVerified == false).Select(p => new PlaceViewModel(

                        p.Id,
                        p.Wagon,
                        p.PassengerPlace,
                        p.Price,
                        p.TrainNumber,
                        p.PassName,
                        p.Type

                        )).ToList()
            }).ToList();
            return PartialView(mod);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Список билетов конкретного пользователя

        public ActionResult UserInfo(string name) {
            try { 
            var data = us.AllUsers().FirstOrDefault(u => u.UserName == name);
            var model = new UserViewModel {
                UserId = data.Id,
                Name = data.UserName,
                Email = data.Email,
                MyTickets = data.Tickets.Select(d => new PlaceViewModel(
                    d.Id,
                    d.Wagon,
                    d.PassengerPlace,
                    d.Price,
                    d.TrainNumber,
                    d.PassName,
                    d.Type
                    )
                {
                    DepStation = d.DepStation,
                    Departure = d.Departure,
                    ArrivalStation =d.ArrivalStation,
                    Arrival = d.Arrival
                }).ToList()
            };
            return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //Подтверждение билета кассиром

        public ActionResult TicketInfo( int ticketId) {
            try { 
            var data = ts.AllTickets().FirstOrDefault(t => t.Id == ticketId);

            var model = new PlaceVerificationModel(data.Id, data.Wagon, data.PassengerPlace, data.TrainNumber);

            return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult TicketInfo(PlaceViewModel pl, int id) {
            try { 
            var ticket = new TicketDTO
            {
                TrainNumber = pl.Train,
                PassengerPlace = pl.Place,
                Wagon = pl.Wagon,

            };

            var res = ts.EditTickets(ticket, id);
            if (res == null)
            {
                unit.Save();
                unit.Dispose();

            
            }
            else
            {
                ModelState.AddModelError(string.Empty, res);
                return View();
            }
            
            return RedirectToAction("UserList");
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