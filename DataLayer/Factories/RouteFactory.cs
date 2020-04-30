using DataLayer.Context;
using DataLayer.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{

    //Редактирование маршрута

    public interface IPathEditor {
        string EditPath(Route route, string oldName);
    }
    class PathEditor : IPathEditor {
        AppDbContext db;

        public PathEditor(AppDbContext context) {
            db = context;
        }

        public string EditPath(Route route, string oldName)
        {
            var chk = db.Routes.FirstOrDefault(r => r.RouteName == oldName);
            
            if (chk == null)
            {
                return "Маршрут с таким именем не существует.";
            }
            else
            {

                db.Entry(chk).State = EntityState.Modified;
                chk.RouteName = route.RouteName;
                chk.Distance = route.Distance;

            }
            return null;
        }
    }

    //Создание маршрута

    public interface IPathCreator {
        string CreatePath(Route route);
    }

    class PathCreator : IPathCreator {
        AppDbContext db;

        public PathCreator(AppDbContext context) {
            db = context;
        }

        public string CreatePath(Route route)
        {
            var chk = db.Routes.FirstOrDefault(r=>r.RouteName == route.RouteName);

            if (chk != null)
            {
                return "Маршрут с таким именем уже существует.";
            }
            else {
                
                List<Station> sts = new List<Station>();
                route.Stops.ForEach(s => sts.Add(db.Stations.FirstOrDefault(st => st.StationName.Equals(s.StationName))));
                route.Stops.Clear();
                route.Stops = sts;
                route.Stops.ForEach(s => db.Entry(db.Stations.FirstOrDefault(st => st.StationName == s.StationName)).State = EntityState.Modified);
                route.Stops.ForEach(s => db.Stations.FirstOrDefault(st => st.StationName.Equals(s.StationName)).Routes.Add(route));


                db.Routes.Add(route);
                
            }
            return null ;
        }
    }

    //Редактирование поезда

    public interface IEditRoute
    {
        string EditorRoute(Train tr, string selected);
    }

    class EditRoute : IEditRoute {
        AppDbContext db;

        public EditRoute(AppDbContext context) {
            db = context;
        }

       public string EditorRoute(Train tr, string selected)
        {
            //Получение необходимых сущностей

            var train = db.Trains.Include("Seats").FirstOrDefault(t=>t.Number == tr.Number);
            var rt = db.Routes.FirstOrDefault(r => r.RouteName == selected);
            var departure = db.Stations.FirstOrDefault(s => s.StationName == tr.DepartureStation);
            var arrival = db.Stations.FirstOrDefault(s => s.StationName == tr.ArrivalStation);
            var sts = db.Stations;

            if (train == null)
            {
                return "Поезд не найден";
            }
            else
                if(rt==null)
            {
                return "Маршрут не найден";
            }
            else {
                db.Trains.Remove(train);
                
                tr.CurrentRoute = rt;

                var t = db.Trains.Add(tr);
                t.Seats.ForEach(s => s.Train = t);
                train.Stops.ForEach(s => sts.FirstOrDefault(st => st.StationName == s.StopStation).Trains.Add(t)); //Поиск станций и добавление к ним поезда
                rt.Trains.Add(t);
                t.Seats.ForEach(s => db.Seats.Add(s));
                departure.Trains.Add(t);
                arrival.Trains.Add(t);

                //Обозначение сущностей состоянием Modified для внесения изменений в таблицу
                t.Stops.ForEach(s => db.Entry(sts.FirstOrDefault(st => st.StationName == s.StopStation)).State = EntityState.Modified);
                db.Entry(rt).State = EntityState.Modified;
                db.Entry(departure).State = EntityState.Modified;
                db.Entry(arrival).State = EntityState.Modified;
                return null;
            }
        }
    }
    //Получение списка маршрутов

    public interface IRouteScaf
    {
        IEnumerable<Route> RoutesList();
    }

    class RoutesList : IRouteScaf
    {
        AppDbContext db = new AppDbContext();

        IEnumerable<Route> IRouteScaf.RoutesList()
        {
            return db.Routes.Include("Trains").Include("Stops");
        }
    }

    //Создание поезда

    public interface IAddRoute
    {
        string AddRoute(Train train, List<Stopover> stops, string selected);
    }

    class RouteAdd : IAddRoute
    {
        AppDbContext db;

        public RouteAdd(AppDbContext context)
        {
            db = context;
        }
       
       public string AddRoute(Train train, List<Stopover> stops, string selected)
        {
            //Получение необходимых сущностей

            var rt = db.Routes.FirstOrDefault(r => r.RouteName == selected);                       
            var departure = db.Stations.FirstOrDefault(s=>s.StationName==train.DepartureStation);  
            var arrival = db.Stations.FirstOrDefault(s => s.StationName == train.ArrivalStation);   
            var sts = db.Stations; 
            
            
            train.CurrentRoute = rt;
            train.Stops = stops;
            
            var t = db.Trains.Add(train);
            t.Seats.ForEach(s=>s.Train=t);
            train.Stops.ForEach(s => sts.FirstOrDefault(st => st.StationName == s.StopStation).Trains.Add(t)); //Поиск станций и добавление к ним поезда
            rt.Trains.Add(t);     //Добавление поезда к маршруту
            t.Seats.ForEach(s=> db.Seats.Add(s));  //Добавление мест к поезду
            departure.Trains.Add(t);
            arrival.Trains.Add(t);
           
            //Обозначение сущностей состоянием Modified для внесения изменений в таблицу

            t.Stops.ForEach(s => db.Entry(sts.FirstOrDefault(st => st.StationName == s.StopStation)).State = EntityState.Modified);
            db.Entry(rt).State = EntityState.Modified;
            db.Entry(departure).State = EntityState.Modified;
            db.Entry(arrival).State = EntityState.Modified;

            return null;
           
        }
    }

    //Получение списка всех поездов

    public interface IRoutes
    {
        IEnumerable<Train> AllRoutes();
    }
    class ShowRoutes : IRoutes
    {
        AppDbContext db = new AppDbContext();
        public IEnumerable<Train> AllRoutes()
        {
            return db.Trains.Include("Stops").Include("Passengers").Include("CurrentRoute")
                .Include("Stations").Include("Seats");
        }
    }

    public interface IRouteManager
    {
        IRoutes RoutesInfo();
        IAddRoute CreateRoute();
        IRouteScaf RoutesScaf();
        IEditRoute RouteEditor();
        IPathCreator CreatePath();
        IPathEditor EditPath();
       
    }
    public class RouteFactory : IRouteManager
    {
        private AppDbContext db;

        public RouteFactory(AppDbContext context)
        {
            db = context;
        }

        public IPathCreator CreatePath()
        {
            return new PathCreator(db);
        }

        public IAddRoute CreateRoute()
        {
           return new RouteAdd(db);
        }

        public IPathEditor EditPath()
        {
            return new PathEditor(db);
        }

        public IEditRoute RouteEditor()
        {
            return new EditRoute(db);
        }

        public IRoutes RoutesInfo()
        {
            return new ShowRoutes();
        }

        public IRouteScaf RoutesScaf()
        {
            return new RoutesList();
        }
    }
}
