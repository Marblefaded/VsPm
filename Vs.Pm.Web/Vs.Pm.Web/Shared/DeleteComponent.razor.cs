﻿using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Vs.Pm.Web.Shared
{
    public class DeleteComponentView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
                
        public bool Answer { get; set; } = true;

        public void Delete()
        {
            MudDialog.Close(DialogResult.Ok(Answer));
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
