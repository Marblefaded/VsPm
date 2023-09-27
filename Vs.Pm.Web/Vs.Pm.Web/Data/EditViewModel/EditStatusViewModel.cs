using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.EditViewModel
{
    public class EditStatusViewModel
    {
        public bool IsConcurency { get; set; }
        public StatusViewModel Item { get; set; }
        public List<StatusViewModel> Models { get; set; }
        public string ConcurencyErrorText { get; set; }
    }
}
