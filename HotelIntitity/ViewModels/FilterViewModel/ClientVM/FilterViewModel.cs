namespace HotelIntitity.ViewModels.FilterViewModel.ClientVM
{
    public class FilterViewModel
    {
        public FilterViewModel(string name,string email)
        {
            SelectedEmail = email;
            SelectedName = name;
        }
    
        public string SelectedEmail { get; private set; }   // выбранный email
        public string SelectedName { get; private set; }    // введенное имя

    }
}
