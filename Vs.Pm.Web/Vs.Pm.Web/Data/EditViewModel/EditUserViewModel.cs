using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.EditViewModel
{
    public class EditUserViewModel
    {
        public bool IsConcurency { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public List<UserViewModel> Models { get; set; }
        public string ConcurencyErrorText { get; set; }
    }
}
