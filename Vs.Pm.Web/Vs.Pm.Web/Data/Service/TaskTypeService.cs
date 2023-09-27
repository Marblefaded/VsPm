using Vs.Pm.Pm.Db;
using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.Service
{
    public class TaskTypeService
    {
        private static VsPmContext dbContext;
        EFRepository<TaskType> repos;

        public TaskTypeService(VsPmContext context)
        {
            dbContext = context;
            repos = new EFRepository<TaskType>(context);
        }
        public List<TaskTypeViewModel> GetAll()
        {
            var list = repos.Get().ToList();
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
            var x = repos.Reload(item.TaskTypeId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(TaskTypeViewModel item)
        {
            var x = repos.FindById(item.TaskTypeId);
            repos.Remove(x);
        }

        public TaskTypeViewModel Update(TaskTypeViewModel item)
        {
            var x = repos.FindByIdForReload(item.TaskTypeId);

            x.Title = item.Title;
            
            return Convert(repos.Update(x, item.Item.Timestamp));
        }

        public TaskTypeViewModel Create(TaskTypeViewModel item)
        {
            var newItem = repos.Create(item.Item);

            return Convert(newItem);
        }

        public List<TaskTypeViewModel> FilteringEmploers(string y)
        {
            var filteredListRooms = repos.GetQuery().Where(x => (x.Title.StartsWith(y))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
        public string GetName(int id)
        {
            var x = repos.FindById(id);
            return x.Title;
        }
    }
}
