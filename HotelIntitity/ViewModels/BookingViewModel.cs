using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.ViewModels
{
    public class BookingViewModel
    {

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string RoomType { get; set; }
 
    }
}
