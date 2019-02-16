using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        //public virtual List<Client> Client { get; set; }
        //public virtual List<Room> Room { get; set; }

        public virtual Client Client { get; set; }
        public virtual Room Room { get; set; }



    }
}
