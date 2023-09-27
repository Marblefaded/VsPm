using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.ViewModel;
using TaskModel = Vs.Pm.Pm.Db.Models.TaskModel;

namespace Vs.Pm.Web.Data.Service
{
    public class TaskService
    {
        private static VsPmContext DbContext;
        EFRepository<TaskModel> repos;

        public TaskService(VsPmContext context)
        {
            DbContext = context;
            repos = new EFRepository<TaskModel>(context);
        }
        public List<TaskViewModel> GetAll()
        {
            var list = repos.Get().ToList();
            var result = list.Select(x => Convert(x)).ToList();
           
            return result;
        }

        private static TaskViewModel Convert(TaskModel Model)
        {
            var item = new TaskViewModel(Model);
            return item;
        }

        public TaskViewModel ReloadItem(TaskViewModel item)
        {
            var x = repos.Reload(item.TaskId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(TaskViewModel item)
        {
            var x = repos.FindById(item.TaskId);
            repos.Remove(x);
        }

        public TaskViewModel Update(TaskViewModel item)
        {
            var x = repos.FindByIdForReload(item.TaskId);

            x.Title = item.Title;
            x.Description = item.Description;
            x.ProjectId = item.ProjectId;
            x.StatusId = item.StatusId;
            x.TaskTypeId = item.TaskTypeId;

            return Convert(repos.Update(x, item.Item.Timestamp));
        }

        public TaskViewModel Create(TaskViewModel item)
        {
            var newItem = repos.Create(item.Item);

            return Convert(newItem);
        }

        public List<TaskViewModel> FilteringEmploers(string y)
        {
            var filteredListRooms = repos.GetQuery().Where(x => (x.Title.Contains(y))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
        public List<TaskViewModel> FilteringProject(int y)
        {
            var filteredProjects = repos.GetQuery().Where(x => (x.ProjectId == y));
            var result = filteredProjects.Select(Convert).ToList();

            return result;
        }
        public List<TaskViewModel> FilteringTaskType(int y)
        {
            var filteredProjects = repos.GetQuery().Where(x => (x.TaskTypeId == y));
            var result = filteredProjects.Select(Convert).ToList();

            return result;
        }
    }
}
