using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.DashBoardStatus.InfoStatus
{
    public class StatusInfoView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public StatusViewModel StatusViewModel { get; set; } = new();
        [Parameter]
        public string Title { get; set; }
        public List<ChangeLog> ChangeLogModel = new();
        [Inject] public StatusService StatusService { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void AddInfo()
        {
            ChangeLogModel = JsonSerializer.Deserialize<List<ChangeLog>>(StatusViewModel.ChangeLogJson);
        }

        protected override async Task OnInitializedAsync()
        {
            AddInfo();
            await InvokeAsync(StateHasChanged);
        }
    }
}
