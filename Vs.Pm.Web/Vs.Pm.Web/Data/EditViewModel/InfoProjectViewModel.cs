using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.EditViewModel
{
    public class InfoProjectViewModel
    {
        public bool IsConcurency { get; set; }
        public ProjectViewModel ProjectViewModel { get; set; }
        public List<ProjectViewModel> Models { get; set; }
        public string ConcurencyErrorText { get; set; }
    }
}
