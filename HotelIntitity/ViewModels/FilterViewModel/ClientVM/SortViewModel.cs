using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.ViewModels.FilterViewModel.ClientVM
{
    public class SortViewModel
    {
        public SortState NameSort { get; private set; } // значение для сортировки по имени
        public SortState EmailSort { get; private set; }    // значение для сортировки по адресу

        public SortState Current { get; private set; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            EmailSort = sortOrder == SortState.EmailAsc ? SortState.EmailDesc : SortState.EmailAsc;
            
            Current = sortOrder;
        }
    }
}
