using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.EditViewModel
{
    public class EditTaskViewModel
    {
        public bool IsConcurency { get; set; }
        public TaskViewModel TaskViewModel { get; set; }
        public List<TaskViewModel> Models { get; set; }
        public string ConcurencyErrorText { get; set; }

        public List<ProjectViewModel> ProjectViews { get; set; } = new List<ProjectViewModel>();
        public List<StatusViewModel> StatusViews { get; set; } = new List<StatusViewModel>();
        public List<TaskTypeViewModel> TaskTypeViews { get; set; } = new List<TaskTypeViewModel>();
    }
}
