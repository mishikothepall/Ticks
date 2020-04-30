using AutoMapper;
using BusLayer.DTO;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
    static class AutomapperConfig
    {
        internal static MapperConfiguration Config
        {
            get
            {
                return new MapperConfiguration(cfg => {

                    cfg.CreateMap<Account, UserDTO>().ForMember(u => u.Tickets, ti => ti.MapFrom(t => t.Tickets));
                    cfg.CreateMap<Train, TrainDTO>().ForMember(t => t.Seats, p => p.MapFrom(s => s.Seats));
                    cfg.CreateMap<Train, TrainDTO>().ForMember(t => t.Stations, st => st.MapFrom(s => s.Stations));
                    cfg.CreateMap<Train, TrainDTO>().ForMember(t => t.Stops, ts => ts.MapFrom(so => so.Stops));
                    cfg.CreateMap<Train, TrainDTO>().ForMember(u => u.Passengers, ti => ti.MapFrom(t => t.Passengers));
                    cfg.CreateMap<Station, StationDTO>().ForMember(s => s.Trains, rt => rt.MapFrom(r => r.Trains));
                    cfg.CreateMap<Station, StationDTO>().ForMember(s=>s.Routes, rt => rt.MapFrom(r=>r.Routes));
                    cfg.CreateMap<Route, RouteDTO>().ForMember(r=>r.Stops, rt=>rt.MapFrom(r=>r.Stops));
                    cfg.CreateMap<Route, RouteDTO>().ForMember(r=>r.Trains, rt=> rt.MapFrom(r=>r.Trains));

                    cfg.CreateMap<Seat, SeatDTO>();
                    cfg.CreateMap<Stopover, StopoverDTO>();
                    cfg.CreateMap<Train, TrainDTO>();
                    cfg.CreateMap<Place, TicketDTO>();

                });
            }
        }
    }
}
