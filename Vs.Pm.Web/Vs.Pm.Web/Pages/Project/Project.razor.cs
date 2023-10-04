using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.Project.EditProject;
using Vs.Pm.Web.Pages.Project.Info;
using Vs.Pm.Web.Shared;

namespace Vs.Pm.Web.Pages
{
    public class ProjectView:ComponentBase
    {
        [Inject] protected ProjectService Service { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
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

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditProject> { { x => x.ProjectViewModel, newItem } };
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
                    StateHasChanged();
                }
                Service.GetAll();

            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace,ex.InnerException.Message, DateTime.Now);
            }
        }

        public async void EditItemDialog(ProjectViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditProject> { { x => x.ProjectViewModel, item } };
                parameters.Add(x => x.mTitle, "Изменение проживающего");
                var dialog = DialogService.Show<EditProject>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    ProjectViewModel returnModel = new ProjectViewModel();
                    returnModel = (ProjectViewModel)result.Data;
                    
                    
                    var newItem = Service.Update(returnModel);
                    var index = Model.FindIndex(x => x.ProjectId == newItem.ProjectId);
                    Model[index] = newItem;
                    Model = Service.GetAll();
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
            catch(Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
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
                LogService.Create(LogModel, ex.Message, ex.StackTrace,ex.InnerException.Message, DateTime.Now);
            }
        }
    }
}
