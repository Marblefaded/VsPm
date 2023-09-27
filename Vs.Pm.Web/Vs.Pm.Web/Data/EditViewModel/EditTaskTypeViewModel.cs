using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.EditViewModel
{
    public class EditTaskTypeViewModel
    {
        public bool IsConcurency { get; set; }
        public TaskTypeViewModel TaskTypeViewModel { get; set; }
        public List<TaskTypeViewModel> Models { get; set; }
        public string ConcurencyErrorText { get; set; }
    }
}
