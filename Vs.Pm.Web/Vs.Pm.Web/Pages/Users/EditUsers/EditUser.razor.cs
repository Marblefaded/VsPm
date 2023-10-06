using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Pages.Users.EditUsers
{
    public class EditUserView:ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();
        [Parameter]
        public string mTitle { get; set; }
        [Inject] protected UserService Service { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(UserViewModel));
            
        }
    }
}
