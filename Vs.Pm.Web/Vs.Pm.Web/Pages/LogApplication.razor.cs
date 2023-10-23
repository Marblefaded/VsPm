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
        [Inject] protected IServiceScopeFactory ScopeFactory { get; set; }
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
            for(int i = 0; i < 1000; i++)
            {
                try
                {
                    try
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
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

        public async Task Generator1k()
        {
            var GeneratorList = new List<LogApplicationViewModel>();
            var date = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                var GeneratorItem = new LogApplicationViewModel();
                {
                    GeneratorItem.LogApplicationId = i;
                    GeneratorItem.ErrorMessage = "Generated";
                    GeneratorItem.ErrorContext = "Generated";
                    GeneratorItem.ErrorInnerException = "Generated";
                    GeneratorItem.Date= date;
                };
                if (i % 1000 == 0)
                {
                    date = date.AddDays(-1);
                }
                GeneratorList.Add(GeneratorItem);
            }
            using (var scope = this.ScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<LogApplicationService>();
                var newList = service.CreateDate(GeneratorList);
                ListModel.AddRange(newList);
            }
            await InvokeAsync(StateHasChanged);
        }
        

        public void Clear()
        {
            ListModel.Clear();
            LogService.ClearAll();
        }

        /*DECLARE @CounterX INT = 1
DECLARE @CounterY Int = 1
DECLARE @Date DATE = '2023-10-18' 

WHILE @CounterY <= 10
BEGIN


WHILE @CounterX <= 1000
BEGIN
    INSERT INTO[dbo].LogApplicationError(InsertDate, ErrorContext, ErrorMessage, ErrorInnerException)
    VALUES(@Date, 'Qqqqqq', 'Qqqqqq', 'Qqqqqq' );

        SET @CounterX = @CounterX + 1
END

SET @Date = DATEADD(day, 1, @Date);
        SET @CounterY = @CounterY + 1
END


SELECT[l].[LogApplicationErrorId], [l].[ErrorContext], [l].[ErrorInnerException], [l].[ErrorMessage], [l].[InsertDate]
        FROM[LogApplicationError] AS[l] where ErrorContext like '%Q%'-- and(InsertDate >= '2023-10-01' and InsertDate < '2023-10-20')

TRUNCATE TABLE[dbo].LogApplicationError*/


    }
}
