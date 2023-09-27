using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.WorkingStatus.EditTaskType
{
    public class EditTaskTypeView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public TaskTypeViewModel TaskTypeViewModel { get; set; } = new TaskTypeViewModel();
        [Parameter]
        public string Title { get; set; }
        [Inject] protected TaskTypeService Service { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(TaskTypeViewModel));
        }
    }
}
