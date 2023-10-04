using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.NetworkInformation;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Pm.Db;
using System.Text.Json;

namespace Vs.Pm.Web.Pages.Project.Info
{
    public class ProjectInfoView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public ProjectViewModel ProjectViewModel { get; set; } = new ProjectViewModel();
        [Parameter]
        public string Title { get; set; }
        public List<ChangeLog> ChangeLogModel = new();
        [Inject] public ProjectService ProjectService { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void AddInfo()
        {
            ChangeLogModel = JsonSerializer.Deserialize<List<ChangeLog>>(ProjectViewModel.ChangeLogJson);
        }

        protected override async Task OnInitializedAsync()
        {
            AddInfo();
            await InvokeAsync(StateHasChanged);
        }
    }
}
