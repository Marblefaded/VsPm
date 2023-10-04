using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.WorkingStatus.InfoTaskType
{
    public class TaskTypeInfoView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public TaskTypeViewModel TaskTypeViewModel { get; set; }
        [Parameter]
        public string Title { get; set; }
        public List<ChangeLog> ChangeLogModel = new();
        [Inject] public TaskTypeService TaskTypeService { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void AddInfo()
        {
            ChangeLogModel = JsonSerializer.Deserialize<List<ChangeLog>>(TaskTypeViewModel.ChangeLogJson);
        }

        protected override async Task OnInitializedAsync()
        {
            AddInfo();
            await InvokeAsync(StateHasChanged);
        }

    }
}