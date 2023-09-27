using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using MudBlazor;
using System.Diagnostics;
using System.Net;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.KanbanBoard;
using Vs.Pm.Web.Pages.TaskView.EditTask;
using static System.Collections.Specialized.BitVector32;

namespace Vs.Pm.Web.Pages.KanbanBoard
{
    public class KanbanView : ComponentBase
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    ListStatus = Service.GetAll();
                    ListTask = TaskService.GetAll();

                    foreach (var item in ListTask)
                    {
                        /*var ListEnd = ListStatus.FirstOrDefault(x=>x.StatusId == item.StatusId);*/
                        var value = new DropItem()
                        {
                            taskViewModel = item,
                            Selector = ListStatus.FirstOrDefault(x => x.StatusId == item.StatusId).Title
                            /*Name = ListStatus.FirstOrDefault(x => x.StatusId == item.StatusId).Title*/

                        };
                        serverData.Add(value);
                    }
                    await UpdateData();
                    /*Task.Yield();*/
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] protected StatusService Service { get; set; }
        [Inject] protected TaskTypeService TaskTypeService { get; set; }
        [Inject] protected ProjectService ProjectService { get; set; }

        public List<ProjectViewModel> ListProject = new();

        public List<TaskViewModel> ListTask = new List<TaskViewModel>();

        public List<TaskTypeViewModel> ListTaskType = new List<TaskTypeViewModel>();

        public List<StatusViewModel> ListStatus = new List<StatusViewModel>();

        public List<DropItem> serverData = new List<DropItem>();
        public List<ProjectViewModel> ProjectViewModel { get; set; } = new List<ProjectViewModel>();

        public MudDropContainer<DropItem> container;
        /*[Inject] public KanbanHub KanbanHubContext { get; set; }*/

        [Inject] protected TaskService TaskService { get; set; }

        [Inject] private LogApplicationService LogService { get; set; }

        public LogApplicationViewModel LogModel = new LogApplicationViewModel();

        public List<DropItem> _items = new();

        public class DropItem
        {
            public TaskViewModel taskViewModel { get; init; }


            public string Selector { get; set; }
        }
        public string mFilterTask;
        public int mFilterProject;
        public int mFilterTaskType;
        public string FilterTask
{
            get => mFilterTask;

            set
            {
                mFilterTask = value;
                FilteredTask();
            }
        }

        public int FilterProject
        {
            get => mFilterProject;

            set
            {
                mFilterProject = value;
                FilteredProject();
            }
        }
        public int FilterTaskType
        {
            get => mFilterTaskType;

            set
            {
                mFilterTaskType = value;
                FilteredTaskTypes();
            }
        }

        public async Task ClearFilters()
        {
            mFilterTask = "";
            mFilterProject = 0;
            mFilterTaskType = 0;
            ListTask = TaskService.GetAll();
            foreach (var item in ListTask)
            {
                var oldItem = serverData.FirstOrDefault(x => x.taskViewModel.TaskId == item.TaskId);
                serverData.Remove(oldItem);
                var rez = new DropItem() { taskViewModel = item, Selector = ListStatus.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
            }
            await UpdateData();
            container?.Refresh();
            StateHasChanged();

        }

        public async Task AddItemDialog(string status)
        {
            try
            {
                var newItem = new TaskViewModel();
                var List = ListStatus.FirstOrDefault(x => x.Title == status);
                newItem.StatusId = List.StatusId;
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTask> { { x => x.TaskViewModel, newItem } };
                var dialog = DialogService.Show<EditTask>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var newUser = TaskService.Create(newItem);
                    ListTask.Add(newItem);
                    _items.Add(new DropItem { taskViewModel = newItem, Selector = ListStatus.FirstOrDefault(x => x.StatusId == newItem.StatusId).Title });
                    serverData.Add(new DropItem { taskViewModel = newItem, Selector = ListStatus.FirstOrDefault(x => x.StatusId == newItem.StatusId).Title });
                    container?.Refresh();
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
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTask> { { x => x.TaskViewModel, item } };
                var dialog = DialogService.Show<EditTask>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskViewModel returnModel = new TaskViewModel();    
                    returnModel = (TaskViewModel)result.Data;
                    var newItem = TaskService.Update(returnModel);
                    var index = ListTask.FindIndex(x => x.TaskId == newItem.TaskId);
                    ListTask[index] = newItem;
                    _items.Add(new DropItem { taskViewModel = newItem, Selector = ListStatus.FirstOrDefault(x => x.StatusId == newItem.StatusId).Title });
                    serverData.Add(new DropItem { taskViewModel = newItem, Selector = ListStatus.FirstOrDefault(x => x.StatusId == newItem.StatusId).Title });
                    var oldItem = serverData.FirstOrDefault(x => x.taskViewModel.TaskId == newItem.TaskId);
                    serverData.Remove(oldItem);
                    await UpdateData();
                    container?.Refresh();
                    StateHasChanged();
                }
                else
                {
                    var oldItem = TaskService.ReloadItem(item);
                    var index = ListTask.FindIndex(x => x.TaskId == oldItem.TaskId);
                    ListTask[index] = oldItem;
                    StateHasChanged();
                    container?.Refresh();
                }
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        private async Task UpdateData()
        {
            try
            {
                _items = serverData
                .Select(item => new DropItem()
                {
                    taskViewModel = item.taskViewModel,
                    Selector = item.Selector
                })
                .ToList();
                await InvokeAsync(StateHasChanged);
                await Task.Delay(1);
                container?.Refresh();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }

        }
        
        protected async void FilteredTask()
        {
            ListTask = TaskService.FilteringEmploers(mFilterTask);
            serverData.Clear();
            foreach (var item in ListTask)
            {
                var rez = new DropItem() { taskViewModel = item, Selector = ListStatus.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
            }
            await UpdateData();
            StateHasChanged();
        }
        protected async void FilteredProject()
        {
            ListTask = TaskService.FilteringProject(mFilterProject);
            serverData.Clear();
            foreach (var item in ListTask)
            {
                var rez = new DropItem() { taskViewModel = item, Selector = ListStatus.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
            }
            await UpdateData();
            StateHasChanged();
        }
        protected async void FilteredTaskTypes()
        {
            ListTask = TaskService.FilteringTaskType(mFilterTaskType);
            serverData.Clear();
            foreach (var item in ListTask)
            {
                var rez = new DropItem() { taskViewModel = item, Selector = ListStatus.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
            }
            await UpdateData();
            StateHasChanged();
        }
    }
}
