using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Shared;

namespace Vs.Pm.Web.Pages.TaskView
{
    public class TaskViewCs:ComponentBase
    {
        [Inject] protected TaskService Service { get; set; }
        [Inject] protected ProjectService ProjectService { get; set; }
        [Inject] protected StatusService StatusService { get; set; }
        [Inject] protected TaskTypeService TaskTypeService { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();
        protected List<TaskViewModel> Model { get; set; }
        public TaskViewModel mCurrentItem;
        public EditTaskViewModel mEditViewModel = new EditTaskViewModel();


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
                var newItem = new TaskViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTask.EditTask> { { x => x.TaskViewModel, newItem } };
                parameters.Add(x => x.Title, "Создание проживающего");
                var dialog = DialogService.Show<EditTask.EditTask>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskViewModel returnModel = new TaskViewModel();
                    //returnModel = (UserViewModel)result.Data;
                    returnModel = newItem;
                    var newUser = Service.Create(returnModel);
                    Model.Add(newItem);
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        public async void EditItemDialog(TaskViewModel item)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var parameters = new DialogParameters<EditTask.EditTask> { { x => x.TaskViewModel, item } };
            parameters.Add(x => x.Title, "Изменение проживающего");
            var dialog = DialogService.Show<EditTask.EditTask>("", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                TaskViewModel returnModel = new TaskViewModel();
                returnModel = (TaskViewModel)result.Data;
                var newItem = Service.Update(returnModel);
                var index = Model.FindIndex(x => x.TaskId == newItem.TaskId);
                Model[index] = newItem;
                StateHasChanged();
            }
            else
            {
                var oldItem = Service.ReloadItem(item);
                var index = Model.FindIndex(x => x.TaskId == oldItem.TaskId);
                Model[index] = oldItem;
                StateHasChanged();
            }

        }

        public async Task DeleteItemAsync(TaskViewModel mCurrentItem)
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
        public string GetStatusNameP(int titleId)
        {
            return ProjectService.GetName(titleId);
        }
        public string GetStatusNameS(int titleId)
        {
            return StatusService.GetName(titleId);
        }
        public string GetStatusNameT(int titleId)
        {
            return TaskTypeService.GetName(titleId);
        }


    }
}
