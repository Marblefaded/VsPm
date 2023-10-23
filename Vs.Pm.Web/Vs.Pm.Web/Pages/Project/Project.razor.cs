using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using NuGet.Configuration;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.Project.EditProject;
using Vs.Pm.Web.Pages.Project.Info;
using Vs.Pm.Web.Shared;


namespace Vs.Pm.Web.Pages
{
    public class ProjectView : ComponentBase
    {
        [Inject] protected ProjectService Service { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
        [Inject] protected IServiceScopeFactory ScopeFactory { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();
        protected List<ProjectViewModel> Model { get; set; }
        public ProjectViewModel mCurrentItem;
        public EditProjectViewModel mEditViewModel = new EditProjectViewModel();


        public bool isRemove;
        public string mFilterValue;

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
            get => mFilterValue;

            set
            {
                mFilterValue = value;
                Filter();
            }
        }
        protected void Filter()
        {
            Model = Service.FilteringEmploers(mFilterValue);
            StateHasChanged();
        }
        public void ClearInput()
        {
            mFilterValue = "";
            Model = Service.GetAll();
            StateHasChanged();
        }

        public async Task AddItemDialog()
        {
            try
            {

                var newItem = new ProjectViewModel();
                EditProjectViewModel editProject = new();
                editProject.ProjectViewModel = newItem;

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditProject> { { x => x.EditProjectViewModel, editProject },{x=>x.AddItem, true } };
                var dialog = DialogService.Show<EditProject>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    ProjectViewModel returnModel = new ProjectViewModel();
                    //returnModel = (UserViewModel)result.Data;
                    returnModel = newItem;
                    var newUser = Service.Create(returnModel);
                    Model.Add(newItem);
                    Service.GetAll();
                }
                Service.GetAll();

            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }
        public Task<ProjectViewModel> Refresh(ProjectViewModel item)
        {

            var refreshItem = Service.RefreshItem(item);
            if (refreshItem == null)
            {
                Model.Remove(item);
            }
            else
            {
                if (mEditViewModel.IsConcurency)
                {
                    mEditViewModel.ProjectViewModel = refreshItem;
                }
                var index = Model.FindIndex(x => x.ProjectId == item.ProjectId);
                if (refreshItem.Item == null)
                {
                    /*mEditViewModel.DialogIsOpen = false;*/
                    Model.RemoveAt(index);
                }
                else
                {
                    mEditViewModel.IsConcurency = false;
                    Model[index] = refreshItem;
                }
            }
            StateHasChanged();
            return Task.FromResult(item);
        }

        public async void EditItemDialog(ProjectViewModel item)
        {
            /*try
            {*/
            EditProjectViewModel editProject = new();
            editProject.ProjectViewModel = item;

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var parameters = new DialogParameters<EditProject> { { x => x.EditProjectViewModel, editProject },{x=>x.AddItem,false} };
            /*parameters.Add(x => x.Refresh, Refresh(item));*/
            var dialog = DialogService.Show<EditProject>("", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var index = Model.FindIndex(x => x.ProjectId == editProject.ProjectViewModel.ProjectId);
                Model[index] = editProject.ProjectViewModel;
                StateHasChanged();
            }
            else
            {
                var oldItem = Service.ReloadItem(item);
                var index = Model.FindIndex(x => x.ProjectId == oldItem.ProjectId);
                Model[index] = oldItem;
                Model = Service.GetAll();
                StateHasChanged();
            }
            
        }

        public async void ChangeLogInfo(ProjectViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<ProjectInfo> { { x => x.ProjectViewModel, item } };
                parameters.Add(x => x.Title, "ProjectInfo");
                var dialog = DialogService.Show<ProjectInfo>("", parameters, options);
                Model = Service.GetAll();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        public async Task DeleteItemAsync(ProjectViewModel mCurrentItem)
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

        public async Task Generator1k()
        {
            var GeneratorList = new List<ProjectViewModel>();
            for (int i = 0; i < 1000; i++)
            {
                var GeneratorItem = new ProjectViewModel();
                {
                    GeneratorItem.Title = "Privet";
                };
                GeneratorList.Add(GeneratorItem);
            }
            using (var scope = this.ScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<ProjectService>();
                var newList = service.CreateDate(GeneratorList);
                Model.AddRange(newList);
            }
            await InvokeAsync(StateHasChanged);
        }
        public void Generator10k()
        {
            for (int i = 0; i < 10000; i++)
            {
                var GenerateModel = new ProjectViewModel()
                {
                    Title = "By generator"
                };
                Service.Create(GenerateModel);
            }
            /*Model = Service.GetAll();*/
            StateHasChanged();
        }
        public async Task Generator100k()
        {
            var GeneratorList = new List<ProjectViewModel>();
            for (int i = 0; i < 100000; i++)
            {
                var GeneratorItem = new ProjectViewModel();
                {
                    GeneratorItem.Title = "Privet";
                };
                GeneratorList.Add(GeneratorItem);
            }
            using (var scope = this.ScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<ProjectService>();
                var newList = service.CreateDate(GeneratorList);
                Model.AddRange(newList);
            }
            await InvokeAsync(StateHasChanged);
        }

        public void Clear()
        {
            Model.Clear();
            Service.ClearAll();
        }
    }
}
