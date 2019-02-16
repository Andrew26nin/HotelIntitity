using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.Models
{
    public class Room
    {

        public int Id { get; set; }


        public int RoomTypeId { get; set; }        
        public virtual RoomType RoomType { get; set; }

        public virtual List<Booking> Booking { get; set; }
    }
}
