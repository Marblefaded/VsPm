using Vs.Pm.Pm.Db;
using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.Users;

namespace Vs.Pm.Web.Data.Service
{
    public class TaskTypeService
    {
        private static VsPmContext dbContext;
        EFRepository<TaskType> mRepoTaskType;
        private string _user;

        public TaskTypeService(VsPmContext context, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User != null)
            {
                string user = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "";
                var index = user.IndexOf("@");
                if (index > 0)
                {
                    _user = user.Substring(0, index);
                }
            }
            dbContext = context;
            mRepoTaskType = new EFRepository<TaskType>(context, _user);
        }
        public List<TaskTypeViewModel> GetAll()
        {
            var list = mRepoTaskType.Get().ToList();
            var result = list.Select(Convert).ToList();
            foreach (var item in result)
            {
                item.IsDeleteEnabled = dbContext.IsTaskTypeDeleteEnabled(item.TaskTypeId);
            }
            return result;
        }

        private static TaskTypeViewModel Convert(TaskType Model)
        {
            var item = new TaskTypeViewModel(Model);
            return item;
        }

        public TaskTypeViewModel ReloadItem(TaskTypeViewModel item)
        {
            var model = mRepoTaskType.Reload(item.TaskTypeId);
            if (model == null)
            {
                return null;
            }
            return Convert(model);
        }

        public void Delete(TaskTypeViewModel item)
        {
            var model = mRepoTaskType.FindById(item.TaskTypeId);
            mRepoTaskType.Remove(model);
        }

        public TaskTypeViewModel Update(TaskTypeViewModel item)
        {
            var model = mRepoTaskType.FindByIdForReload(item.TaskTypeId);

            model.Title = item.Title;
            
            return Convert(mRepoTaskType.Update(model, item.Item.Timestamp));
        }

        public TaskTypeViewModel Create(TaskTypeViewModel item)
        {
            var newItem = mRepoTaskType.Create(item.Item);

            return Convert(newItem);
        }

        public List<TaskTypeViewModel> FilteringEmploers(string filterValue)
        {
            var filteredListRooms = mRepoTaskType.GetQuery().Where(x => (x.Title.StartsWith(filterValue))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
        public string GetName(int id)
        {
            var model = mRepoTaskType.FindById(id);
            return model.Title;
        }
    }
}
