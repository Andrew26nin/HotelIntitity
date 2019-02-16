using HotelIntitity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.ViewModels.FilterViewModel.RoomVM
{
    public class IndexViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
