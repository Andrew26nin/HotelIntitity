using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
    }
}
