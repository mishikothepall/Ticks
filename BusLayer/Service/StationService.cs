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
    public interface IStationService
    {
        List<StationDTO> AllStations();
        string CreateStation(string name);
    }
    public class StationService : IStationService
    {
        private UnitOfWork Unit { get; set; }

        private IMapper mapper = new Mapper(AutomapperConfig.Config);

        public StationService(UnitOfWork unit)
        {
            Unit = unit;
        }

        public List<StationDTO> AllStations()
        {
            return mapper.Map<List<Station>, List<StationDTO>>(Unit.stationFactory.GetStations()
                .AllStations().ToList());
        }

        public string CreateStation(string name) {
            var res = AllStations().FirstOrDefault(s=>s.StationName==name);
            if (res == null)
            {
                Unit.stationFactory.Create().CreateStation(name);
                return null;
            }
            return "Такая станция уже существует.";
        }
    }
}
