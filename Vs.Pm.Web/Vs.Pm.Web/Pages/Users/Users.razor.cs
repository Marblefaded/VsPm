using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Web.Data.EditViewModel;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.Project.Info;
using Vs.Pm.Web.Pages.Users.EditUsers;
using Vs.Pm.Web.Pages.Users.InfoUsers;
using Vs.Pm.Web.Shared;

namespace Vs.Pm.Web.Pages.Users
{
    public class UsersView:ComponentBase
    {
        [Inject] protected UserService Service { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationService LogService { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();
        protected List<UserViewModel> Model { get; set; }
        public UserViewModel mCurrentItem;
        public EditUserViewModel mEditViewModel = new EditUserViewModel();


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

        public async void ChangeLogInfo(UserViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<UsersInfo> { { x => x.UserViewModel, item } };
                parameters.Add(x => x.Title, "UserInfo");
                var dialog = DialogService.Show<UsersInfo>("", parameters, options);
                Model = Service.GetAll();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        public async Task AddItemDialog()
        {
            try
            {
                var newItem = new UserViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditUser> { { x => x.UserViewModel, newItem } };
                var dialog = DialogService.Show<EditUser>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    UserViewModel returnModel = new UserViewModel();
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
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }

        public async void EditItemDialog(UserViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditUser> { { x => x.UserViewModel, item } };
                parameters.Add(x => x.mTitle, "Изменение проживающего");
                var dialog = DialogService.Show<EditUser>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    UserViewModel returnModel = new UserViewModel();
                    returnModel = (UserViewModel)result.Data;


                    var newItem = Service.Update(returnModel);
                    var index = Model.FindIndex(x => x.UserId == newItem.UserId);
                    Model[index] = newItem;
                    Model = Service.GetAll();
                    StateHasChanged();
                }
                else
                {
                    var oldItem = Service.ReloadItem(item);
                    var index = Model.FindIndex(x => x.UserId == oldItem.UserId);
                    Model[index] = oldItem;
                    Model = Service.GetAll();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                LogService.Create(LogModel, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }
        }


        public async Task DeleteItemAsync(UserViewModel mCurrentItem)
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
