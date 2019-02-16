using Data.Models;
using System.Collections.Generic;

namespace HotelIntitity.ViewModels.FilterViewModel.ClientVM
{
    public class IndexViewModel
    {
        public IEnumerable<Client> Clients { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
