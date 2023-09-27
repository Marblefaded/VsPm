using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Shared;
using System.Threading.Tasks;

namespace Vs.Pm.Web.Pages.WorkingStatus
{
    public class TaskTypeView : ComponentBase
    {
        [Inject] protected TaskTypeService Service { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();
        protected List<TaskTypeViewModel> Model { get; set; }
        public TaskTypeViewModel mCurrentItem;
        public EditTaskTypeViewModel mEditViewModel = new EditTaskTypeViewModel();


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
                var newItem = new TaskTypeViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTaskType.EditTaskType> { { x => x.TaskTypeViewModel, newItem } };
                parameters.Add(x => x.Title, "Создание проживающего");
                var dialog = DialogService.Show<EditTaskType.EditTaskType>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskTypeViewModel returnModel = new TaskTypeViewModel();
                    //returnModel = (UserViewModel)result.Data;
                    returnModel = newItem;
                    var newUser = Service.Create(returnModel);
                    Model.Add(newItem);
                    Service.GetAll();
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        public async void EditItemDialog(TaskTypeViewModel item)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var parameters = new DialogParameters<EditTaskType.EditTaskType> { { x => x.TaskTypeViewModel, item } };
            parameters.Add(x => x.Title, "Изменение проживающего");
            var dialog = DialogService.Show<EditTaskType.EditTaskType>("", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                TaskTypeViewModel returnModel = new TaskTypeViewModel();
                returnModel = (TaskTypeViewModel)result.Data;
                var newItem = Service.Update(returnModel);
                var index = Model.FindIndex(x => x.TaskTypeId == newItem.TaskTypeId);
                Model[index] = newItem;
                Model = Service.GetAll();
                StateHasChanged();
            }
            else
            {
                var oldItem = Service.ReloadItem(item);
                var index = Model.FindIndex(x => x.TaskTypeId == oldItem.TaskTypeId);
                Model[index] = oldItem;
                Model = Service.GetAll();
                StateHasChanged();
            }

        }

        public async Task DeleteItemAsync(TaskTypeViewModel mCurrentItem)
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
