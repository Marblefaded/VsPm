using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.TaskView.InfoTask
{
    public class TaskInfoView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public TaskViewModel TaskViewModel { get; set; } = new();
        [Parameter]
        public string Title { get; set; }
        public List<ChangeLog> ChangeLogModel = new();
        [Inject] public TaskService TaskService { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void AddInfo()
        {
            ChangeLogModel = JsonSerializer.Deserialize<List<ChangeLog>>(TaskViewModel.ChangeLogJson);
        }

        protected override async Task OnInitializedAsync()
        {
            AddInfo();
            await InvokeAsync(StateHasChanged);
        }
    }
}
