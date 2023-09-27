using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.TaskView.EditTask
{
    public class EditTaskView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public TaskViewModel TaskViewModel { get; set; } = new TaskViewModel();
        public EditTaskViewModel EditTaskViewModel { get; set; } = new EditTaskViewModel();
        public List<StatusViewModel> StatusViewModel { get; set; } = new List<StatusViewModel>();
        public List<ProjectViewModel> ProjectViewModel { get; set; } = new List<ProjectViewModel>();
        public List<TaskTypeViewModel> TaskTypeViewModel { get; set; } = new List<TaskTypeViewModel>();
        [Parameter]
        public string Title { get; set; }
        [Inject] protected TaskService Service { get; set; }
        [Inject] protected StatusService StatusService { get; set; }
        [Inject] protected ProjectService ProjectService { get; set; }
        [Inject] protected TaskTypeService TaskTypeService { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(TaskViewModel));
        }
        
        protected override async void OnInitialized()
        {
            base.OnInitialized();
            StatusViewModel = StatusService.GetAll();
            ProjectViewModel= ProjectService.GetAll();
            TaskTypeViewModel = TaskTypeService.GetAll();
        }
    }
}
