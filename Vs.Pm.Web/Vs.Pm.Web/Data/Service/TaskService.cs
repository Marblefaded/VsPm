using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.ViewModel;
using TaskModel = Vs.Pm.Pm.Db.Models.TaskModel;
using Microsoft.AspNetCore.Http;
using Vs.Pm.Web.Pages.Users;

namespace Vs.Pm.Web.Data.Service
{
    public class TaskService
    {
        private static VsPmContext DbContext;
        EFRepository<TaskModel> mRepoTask;
        private string _user;

        public TaskService(VsPmContext context, IHttpContextAccessor httpContextAccessor)
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
            DbContext = context;
            mRepoTask = new EFRepository<TaskModel>(context, _user);
        }
        public List<TaskViewModel> GetAll()
        {
            var list = mRepoTask.Get().ToList();
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
            var model = mRepoTask.Reload(item.TaskId);
            if (model == null)
            {
                return null;
            }
            return Convert(model);
        }

        public void Delete(TaskViewModel item)
        {
            var x = mRepoTask.FindById(item.TaskId);
            mRepoTask.Remove(x);
        }

        public TaskViewModel Update(TaskViewModel item)
        {
            var model = mRepoTask.FindByIdForReload(item.TaskId);

            model.Title = item.Title;
            model.Description = item.Description;
            model.ProjectId = item.ProjectId;
            model.StatusId = item.StatusId;
            model.TaskTypeId = item.TaskTypeId;
            model.Hours = item.Hours;

            return Convert(mRepoTask.Update(model, item.Item.Timestamp));
        }

        public TaskViewModel Create(TaskViewModel item)
        {
            var newItem = mRepoTask.Create(item.Item);

            return Convert(newItem);
        }

        public List<TaskViewModel> FilteringEmploers(string filterValue)
        {
            var filteredListRooms = mRepoTask.GetQuery().Where(x => (x.Title.Contains(filterValue))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
        public List<TaskViewModel> FilteringProject(int filterValue)
        {
            var filteredProjects = mRepoTask.GetQuery().Where(x => (x.ProjectId == filterValue));
            var result = filteredProjects.Select(Convert).ToList();

            return result;
        }
        public List<TaskViewModel> FilteringTaskType(int filterValue)
        {
            var filteredProjects = mRepoTask.GetQuery().Where(x => (x.TaskTypeId == filterValue));
            var result = filteredProjects.Select(Convert).ToList();

            return result;
        }
    }
}
