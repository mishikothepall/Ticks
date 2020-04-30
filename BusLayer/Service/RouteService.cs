using AutoMapper;
using BusLayer.DTO;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.Service
{
    public interface IRouteService
    {
        List<TrainDTO> AllRoutes();

        TrainDTO SelectTrain(int id);

        string AddRoute(TrainDTO tr, List<StopoverDTO> stops, string selected);

        IEnumerable<RouteDTO> RouteScaf();

        List<TrainDTO> RouteFilter(string departure, string arrival, DateTime date);

        string EditRoute(TrainDTO tr, List<StopoverDTO> stops, string selected);

        string AddPath(RouteDTO route);

        string EditPath(RouteDTO route, string oldName);
    }

    public class RouteService : IRouteService
    {
        private UnitOfWork Unit { get; set; }

        private IMapper mapper = new Mapper(AutomapperConfig.Config);

        public RouteService(UnitOfWork unit)
        {
            Unit = unit;
        }

        public List<TrainDTO> AllRoutes()
        {
            return mapper.Map<List<Train>, List<TrainDTO>>(Unit.routeFactory.RoutesInfo().AllRoutes().ToList());
        }

        public TrainDTO SelectTrain(int id)
        {
            return AllRoutes().FirstOrDefault(t=>t.Id==id);
        }

        public string AddRoute(TrainDTO tr, List<StopoverDTO> stops, string selected)
        {
            Train train = new Train {
                Number = tr.Number,
                DepartureStation = tr.DepartureStation,
                ArrivalStation = tr.ArrivalStation,
                Departure = tr.Departure,
                Seats =  tr.Seats.Select(s=> new Seat {
                    Type = s.Type,
                    Quantity = s.Quantity,
                    Price = s.Price
                }).ToList()
            };

            var stopovers = stops.Select(s=> new Stopover{
                StopStation = s.StopStation,
                Departure = s.Departure,
                Arrival = s.Arrival
            }).ToList();
            return Unit.routeFactory.CreateRoute().AddRoute(train, stopovers, selected);
        }

        public IEnumerable<RouteDTO> RouteScaf()
        {
            return mapper.Map<IEnumerable<Route>, IEnumerable<RouteDTO>>(Unit.routeFactory.RoutesScaf().RoutesList());
        }

        public List<TrainDTO> RouteFilter(string departure, string arrival, DateTime date)
        {
           var dep = AllRoutes().Where(r => r.Stops.Exists(s => s.StopStation == departure) && r.Stops.Exists(s=>s.StopStation==arrival) && r.Departure.Date == date).ToList();
           return dep;
        }

        public string EditRoute(TrainDTO tr, List<StopoverDTO> stops, string selected) {
            var rt = Unit.routeFactory.RoutesScaf().RoutesList().FirstOrDefault(r => r.RouteName == selected);
            var train = new Train {
                Number = tr.Number,
                DepartureStation = tr.DepartureStation,
                ArrivalStation = tr.ArrivalStation,
                Departure = tr.Departure,
                Seats = tr.Seats.Select(s => new Seat
                {
                    Type = s.Type,
                    Quantity = s.Quantity,
                    Price = s.Price
                }).ToList(),
                CurrentRoute = rt,
                Stops = stops.Select(s => new Stopover {
                    StopStation = s.StopStation,
                    Departure = s.Departure,
                    Arrival = s.Arrival
                }).ToList()
            };
            return Unit.routeFactory.RouteEditor().EditorRoute(train, selected);
        }

        public string AddPath(RouteDTO route) {
            var rt = new Route {
                RouteName = route.RouteName,
                Distance = route.Distance,
                Stops = route.Stops.Select(r => new Station { StationName = r.StationName}).ToList()
            };
            return Unit.routeFactory.CreatePath().CreatePath(rt);
        }

        public string EditPath(RouteDTO route, string oldName)
        {
            var rt = new Route
            {
                RouteName = route.RouteName,
                Distance = route.Distance,
                Stops = route.Stops.Select(r => new Station { StationName = r.StationName }).ToList()
            };
            return Unit.routeFactory.EditPath().EditPath(rt, oldName);
        }
    }
}
