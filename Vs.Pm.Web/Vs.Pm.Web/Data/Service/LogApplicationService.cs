using Vs.Pm.Pm.Db.Models;
using System.Reflection;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.ViewModel;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Vs.Pm.Web.Data.Service
{
    public class LogApplicationService
    {
        EFRepository<LogApplicationError> mRepoLog;


        public LogApplicationService(VsPmContext context)
        {
            mRepoLog = new EFRepository<LogApplicationError>(context);
            /*dbContext = context;//Исправить*/
        }
        public async Task<List<LogApplicationViewModel>> GetAll()
        {
            var listItems = mRepoLog.Get();
            var result = listItems.Select(Convert).ToList();
            result.Reverse();
            return await System.Threading.Tasks.Task.FromResult(result);
        }

        private static LogApplicationViewModel Convert(LogApplicationError model)
        {
            var item = new LogApplicationViewModel(model);
            return item;
        }

        public LogApplicationViewModel ReloadItem(LogApplicationViewModel item)
        {
            var model = mRepoLog.Reload(item.LogApplicationId);
            if (model == null)
            {
                return null;
            }
            return Convert(model);
        }

        /*public void DeleteSelected()
        {
            var itemsToDelete = DbContext.DbSetLogApplication.Where(x => x.IsEnable == true);
            DbContext.DbSetLogApplication.RemoveRange(itemsToDelete);
            DbContext.SaveChanges();
        }*/

        public void Delete(LogApplicationViewModel item)
        {
            var resualt = mRepoLog.FindById(item.LogApplicationId);
            mRepoLog.Remove(resualt);
        }

        public LogApplicationViewModel Create(LogApplicationViewModel item, string msg, string stackTrace, string? innerEx, DateTime date)
        {
            if (innerEx != null)
            {
                item = new LogApplicationViewModel
                {
                    Date = date,
                    ErrorMessage = msg,
                    ErrorContext = stackTrace,
                    ErrorInnerException = innerEx
                };
                Console.WriteLine($"\n{msg}   {stackTrace}  {date}");
                var newItem = mRepoLog.Create(item.Item);
                return Convert(newItem);
            }
            else
            {
                item = new LogApplicationViewModel
                {
                    Date = date,
                    ErrorMessage = msg,
                    ErrorContext = stackTrace,

                };
                Console.WriteLine($"\n{msg}   {stackTrace}  {date}");
                var newItem = mRepoLog.Create(item.Item);
                return Convert(newItem);
            }

        }



        public List<LogApplicationViewModel> FilteringDate(DateTime? date)
        {
            var filteredListLogs = mRepoLog.GetQuery().Where(x => x.InsertDate.Date == date.GetValueOrDefault().Date).ToList();
            var result = filteredListLogs.Select(Convert).ToList();
            result.Reverse();
            return result;
        }
        public List<LogApplicationViewModel> FilteringError(string message)
        {
            var filteredListLogs = mRepoLog.GetQuery().Where(x => (x.ErrorMessage.StartsWith(message)) || x.ErrorContext.StartsWith(message)).ToList();
            var result = filteredListLogs.Select(Convert).ToList();
            result.Reverse();
            return result;
        }
    }
}
