using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Vs.Pm.Web.Shared
{
    public class RedirectLogin:ComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IJSRuntime Js { get; set; }

        protected readonly string EmpfehlungenView = "EmpfehlungenView";
        protected readonly string TeilnehmerView = "TeilnehmerView";

        protected override void OnAfterRender(bool firstRender)
        {
            if (NavigationManager.Uri.Contains(EmpfehlungenView) || NavigationManager.Uri.Contains(TeilnehmerView))
            {
                Js.InvokeVoidAsync("SetUrlInLocalStorage", NavigationManager.Uri);
            }
            NavigationManager.NavigateTo(NavigationManager.BaseUri + "login");
        }

    }
}
