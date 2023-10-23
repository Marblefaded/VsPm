using Vs.Pm.Pm.Db.Models;
using System.Reflection;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.ViewModel;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

namespace Vs.Pm.Web.Data.Service
{
    public class LogApplicationService
    {
        private static VsPmContext DbContext;
        EFRepository<LogApplicationError> mRepoLog;

        public LogApplicationService(VsPmContext context)
        {
            mRepoLog = new EFRepository<LogApplicationError>(context);
            DbContext = context;
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

        public List<LogApplicationViewModel> CreateDate(List<LogApplicationViewModel> list)
        {
            
            var newList = mRepoLog.CreateBulk(list.Select(x => x.Item).ToList());
            var resultList = newList.Select(Convert).ToList();
            return resultList;
            /*var listItems = mRepoLog.Get();
            var result = listItems.Select(Convert).ToList();
            result.Reverse();
            return await System.Threading.Tasks.Task.FromResult(result);*/
        }
        public void ClearAll()
        {
            DbContext.dbSetLog.RemoveRange(DbContext.dbSetLog);
            DbContext.SaveChanges();
        }

        /*DECLARE @Counter INT = 1

WHILE @Counter <= 100000
BEGIN
    INSERT INTO[dbo].LogApplicationError(InsertDate, ErrorContext, ErrorMessage, ErrorInnerException)
    VALUES(2022-01-01, 'Qqqqqq', 'Qqqqqq', 'Qqqqqq' );

        SET @Counter = @Counter + 1
END*/
    }   
}
