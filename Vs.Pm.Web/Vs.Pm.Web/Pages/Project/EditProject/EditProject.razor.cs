using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.Project.EditProject
{
    public class EditProjectView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public ProjectViewModel ProjectViewModel { get; set; } = new ProjectViewModel();
        [Parameter]
        public string mTitle { get; set; }
        [Inject] protected ProjectService Service { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(ProjectViewModel));
        }
    }
}
