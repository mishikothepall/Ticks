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
    //Получения списка всех станций из контекста

    public interface IStationsList
    {
        IEnumerable<Station> AllStations();
    }
    class StationsList : IStationsList
    {
        AppDbContext db = new AppDbContext();

        public IEnumerable<Station> AllStations()
        {
            return db.Stations.Include("Trains").Include("Routes");
        }
    }

    //Создание станции

    public interface ICreateStation
    {
        void CreateStation(string name);
    }

    

    class StationCreator : ICreateStation
    {

        AppDbContext db;
        public StationCreator(AppDbContext context)
        {
            db = context;
        }

        public void CreateStation(string name)
        {
            db.Stations.Add(new Station { StationName = name});
        }
    }
    
    //Добавление поезда к станции

    public interface IAddTrains
    {
        string AddTrain(Station station, Train train);
    }

    class TrainToStation : IAddTrains
    {
        AppDbContext db;

        public TrainToStation(AppDbContext context)
        {
            db = context;
        }

        public string AddTrain(Station station, Train train)
        {
            var st = db.Stations.FirstOrDefault(s=>s.Id==station.Id);
            var tr = db.Trains.FirstOrDefault(t=>t.Id==train.Id);
            if (st != null && tr != null)
            {
                st.Trains.Add(tr);
                db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                return null;
            }

            return "Поезд или станция не были обнаруженны";
        }
    }

    //Редактирование станции

    public interface IEditStation
    {
        string EditStation(Station station);
    }

    class StationEditor : IEditStation
    {
        AppDbContext db;

        public StationEditor(AppDbContext context)
        {
            db = context;
        }

        public string EditStation(Station station)
        {
            var st = db.Stations.FirstOrDefault(s => s.Id == station.Id);
            if (st != null)
            {
                st=station;
                db.Entry(st).State = EntityState.Modified;
                return null;
            }
            return "Такой станции не существует";
        }
    }
   public interface IStationManager
    {
        IStationsList GetStations();
        ICreateStation Create();
        IAddTrains AddTrain();
        IEditStation Edit();
    }
    public class StationFactory : IStationManager
    {
        AppDbContext db;
        public StationFactory(AppDbContext context)
        {
            db = context;
        }

        public IAddTrains AddTrain()
        {
            return new TrainToStation(db);
        }

        public ICreateStation Create()
        {
            return new StationCreator(db);
        }

        public IEditStation Edit()
        {
            return new StationEditor(db);
        }

        public IStationsList GetStations()
        {
            return new StationsList();
        }
    }
}
