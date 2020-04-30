using DataLayer.Context;
using DataLayer.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
    public class UnitOfWork
    {
        private AppDbContext db;



        public UnitOfWork(AppDbContext _db)
        {
            db = _db;
        }

        internal RouteFactory routeFactory { get { return new RouteFactory(db); } }

        internal UserFactory userFactory { get { return new UserFactory(db); } }

        internal StationFactory stationFactory { get { return new StationFactory(db); } }

        internal TicketFactory ticketFactory { get { return new TicketFactory(db); } }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose() {
            db.Dispose();
        }
    }
}
