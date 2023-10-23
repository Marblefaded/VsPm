using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using NuGet.Configuration;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.Project.EditProject
{
    public class EditProjectView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public EditProjectViewModel EditProjectViewModel { get; set; } 
        [Parameter]
        public string mTitle { get; set; }

        [Parameter]
        public bool AddItem { get; set; }
        [Inject] ProjectService Service { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            if (AddItem)
            {
                MudDialog.Close(DialogResult.Ok(EditProjectViewModel));
            }
            Service.Update(EditProjectViewModel);
            if (EditProjectViewModel.IsConcurency != true)
            {
                MudDialog.Close(DialogResult.Ok(EditProjectViewModel));
            }
            else
            {
                MudDialog.StateHasChanged();
            }


            /*try
            {
                MudDialog.Close(DialogResult.Ok(ProjectViewModel));
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateConcurrencyException)
                {
                    ProjectViewModel.IsConcurency = true;
                    ProjectViewModel.ConcurencyErrorText = "Reload dialog, u have old data";
                }
            }*/
        }
        public void Reload()
        {
            EditProjectViewModel.ProjectViewModel = Service.ReloadItem(EditProjectViewModel.ProjectViewModel);
            EditProjectViewModel.IsConcurency = false;
            MudDialog.StateHasChanged();
        }
    }
}
