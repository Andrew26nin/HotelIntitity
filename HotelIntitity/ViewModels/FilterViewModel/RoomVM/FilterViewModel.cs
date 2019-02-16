using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HotelIntitity.ViewModels.FilterViewModel.RoomVM
{
    public class FilterViewModel
    {
        public FilterViewModel(List<RoomType> types, int? roomtype, int id)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            types.Insert(0, new RoomType { Type = "Все", Id = 0, Cost=0 });
            RoomTypes = new SelectList(types, "Id", "Type", roomtype);
            SelectedType = roomtype;
            SelectedId = id;
        }
        public SelectList RoomTypes { get; private set; } // список типов комнат
        public int? SelectedType { get; private set; }   // выбранный тип
        public int SelectedId { get; private set; }    // Введенный номер

    }
}
