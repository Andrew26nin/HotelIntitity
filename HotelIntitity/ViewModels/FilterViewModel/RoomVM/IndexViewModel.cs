using Data.Models;
using System.Collections.Generic;

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
