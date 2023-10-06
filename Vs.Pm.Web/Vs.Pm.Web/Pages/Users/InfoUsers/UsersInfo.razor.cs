using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.Users.InfoUsers
{
    public class UsersInfoView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();
        [Parameter]
        public string Title { get; set; }
        public List<ChangeLog> ChangeLogModel = new();
        [Inject] public UserService UserService { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void AddInfo()
        {
            ChangeLogModel = JsonSerializer.Deserialize<List<ChangeLog>>(UserViewModel.ChangeLogJson);
        }

        protected override async Task OnInitializedAsync()
        {
            AddInfo();
            await InvokeAsync(StateHasChanged);
        }
    }
}
