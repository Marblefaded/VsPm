using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Shared;

namespace Vs.Pm.Web.Pages
{
    public class LogApplicationView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
        protected List<LogApplicationViewModel> Model { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();

        public LogApplicationViewModel CurrentItem;
        public DateTime filterDate;
        public string filterError = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                FilterDate = DateTime.Now;
                Model = await LogService.GetAll();
                //ModelForClearing = await LogService.GetAll();
                //Model.Reverse();
                await InvokeAsync(StateHasChanged);
            }
        }

        public void PushError()
        {
            try
            {
                try
                {
                    throw new ArgumentOutOfRangeException();
                }

                catch (ArgumentOutOfRangeException e)
                {
                    //make sure this path does not exist
                    if (Model.Contains(LogModel) == false)
                    {
                        throw new FileNotFoundException("Error project. Destroy pc", e);
                    }
                }
            }

            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogService.Create(LogModel, e.Message, e.StackTrace, e.InnerException.StackTrace, DateTime.Now);
                }
            }
        }

        public DateTime FilterDate
        {
            get => filterDate;

            set
            {
                filterDate = value;
                FilterDateTime();
            }
        }
        public string FilterError
        {
            get => filterError;

            set
            {
                filterError = value;
                FiltersError();
            }
        }
        public async Task DeleteItemForeverAsync(LogApplicationViewModel item)
        {
            try
            {
                var dialog = DialogService.Show<DeleteComponent>("Вы точно хотите удалить данную запись?");
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    LogService.Delete(item);
                    Model.Remove(item);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }


        public async Task ClearInput()
        {
            filterError = "";
            Model = await LogService.GetAll();
            StateHasChanged();
        }

        protected void FilterDateTime()
        {
            Model = LogService.FilteringDate(filterDate);
            StateHasChanged();
        }
        protected void FiltersError()
        {
            Model = LogService.FilteringError(filterError);
            StateHasChanged();
        }

    }
}
