using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.ViewModels.FilterViewModel.RoomVM
{
   
        public class SortViewModel
        {
            public SortState IdSort { get; private set; } // значение для сортировки по номеру
            
            public SortState TypeSort { get; private set; }   // значение для сортировки по типу
            public SortState Current { get; private set; }     // текущее значение сортировки

            public SortViewModel(SortState sortOrder)
            {
                IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
                
                TypeSort = sortOrder == SortState.TypeAsc ? SortState.TypeDesc : SortState.TypeAsc;
                Current = sortOrder;
            }
        }
    }

