using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.WorkingStatus.EditTaskType;
using Vs.Pm.Web.Shared;

namespace Vs.Pm.Web.Pages.DashBoardStatus
{
    public class StatusView:ComponentBase
    {
        [Inject] protected StatusService Service { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();
        protected List<StatusViewModel> Model { get; set; }
        public StatusViewModel mCurrentItem;
        public EditStatusViewModel mEditViewModel = new EditStatusViewModel();


        public bool isRemove;
        public string filterValue = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Model = Service.GetAll();
                await InvokeAsync(StateHasChanged);
            }
        }

        public string FilterValue
        {
            get => filterValue;

            set
            {
                filterValue = value;
                Filter();
            }
        }
        protected void Filter()
        {
            Model = Service.FilteringEmploers(filterValue);
            StateHasChanged();
        }
        public void ClearInput()
        {
            filterValue = "";
            Model = Service.GetAll();
            StateHasChanged();
        }

        public async Task AddItemDialog()
        {
            try
            {
                var newItem = new StatusViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditStatus.EditStatus> { { x => x.StatusViewModel, newItem } };
                parameters.Add(x => x.Title, "Создание проживающего");
                var dialog = DialogService.Show<EditStatus.EditStatus>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    StatusViewModel returnModel = new StatusViewModel();
                    //returnModel = (UserViewModel)result.Data;
                    returnModel = newItem;
                    var newUser = Service.Create(returnModel);
                    Model.Add(newItem);
                    Model = Service.GetAll();
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        public async void EditItemDialog(StatusViewModel item)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var parameters = new DialogParameters<EditStatus.EditStatus> { { x => x.StatusViewModel, item } };
            parameters.Add(x => x.Title, "Изменение проживающего");
            var dialog = DialogService.Show<EditStatus.EditStatus>("", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                StatusViewModel returnModel = new StatusViewModel();
                returnModel = (StatusViewModel)result.Data;
                var newItem = Service.Update(returnModel);
                var index = Model.FindIndex(x => x.StatusId == newItem.StatusId);
                Model[index] = newItem;
                Service.GetAll();
                StateHasChanged();
            }
            else
            {
                var oldItem = Service.ReloadItem(item);
                var index = Model.FindIndex(x => x.StatusId == oldItem.StatusId);
                Model[index] = oldItem;
                Model = Service.GetAll();
                StateHasChanged();
            }

        }

        public async Task DeleteItemAsync(StatusViewModel mCurrentItem)
        {
            try
            {
                var dialog = DialogService.Show<DeleteComponent>("Do you really want to delete this item ? ");
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    Service.Delete(mCurrentItem);
                    Model.Remove(mCurrentItem);
                    isRemove = false;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }
    }
}
