using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<TicketDTO> Tickets { get; set; }

        public UserDTO()
        {
            Tickets = new List<TicketDTO>();
        }
    }
}
