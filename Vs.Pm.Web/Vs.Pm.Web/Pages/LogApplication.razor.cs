﻿using Microsoft.AspNetCore.Components;
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
        protected List<LogApplicationViewModel> ListModel { get; set; }
        public LogApplicationViewModel LogModel = new LogApplicationViewModel();

        public LogApplicationViewModel mCurrentItem;
        public DateTime mFilterDate;
        public string mFilterError = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                FilterDate = DateTime.Now;
                ListModel = await LogService.GetAll();
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
                    if (ListModel.Contains(LogModel) == false)
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
            get => mFilterDate;

            set
            {
                mFilterDate = value;
                FilterDateTime();
            }
        }
        public string FilterError
        {
            get => mFilterError;

            set
            {
                mFilterError = value;
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
                    ListModel.Remove(item);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }


        public async Task ClearInput()
        {
            mFilterError = "";
            ListModel = await LogService.GetAll();
            StateHasChanged();
        }

        protected void FilterDateTime()
        {
            ListModel = LogService.FilteringDate(mFilterDate);
            StateHasChanged();
        }
        protected void FiltersError()
        {
            ListModel = LogService.FilteringError(mFilterError);
            StateHasChanged();
        }

    }
}
