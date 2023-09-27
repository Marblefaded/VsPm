using Vs.Pm.Pm.Db;
using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.Service
{
    public class ProjectService
    {
        private static VsPmContext DbContext;
        EFRepository<Project> repos;

        public ProjectService(VsPmContext context)
        {
            DbContext = context;
            repos = new EFRepository<Project>(context);
        }
        public List<ProjectViewModel> GetAll()
        {
            var list = repos.Get().ToList();
            var result = list.Select(x => Convert(x)).ToList();
            foreach (var item in result)
            {
                item.IsDeleteEnabled = DbContext.IsProjectEnabled(item.ProjectId);
            }
            return result;
        }

        private static ProjectViewModel Convert(Project Model)
        {
            var item = new ProjectViewModel(Model);
            return item;
        }

        public ProjectViewModel ReloadItem(ProjectViewModel item)
        {
            var x = repos.Reload(item.ProjectId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(ProjectViewModel item)
        {
            var x = repos.FindById(item.ProjectId);
            repos.Remove(x);
        }

        public ProjectViewModel Update(ProjectViewModel item)
        {
            var x = repos.FindByIdForReload(item.ProjectId);
            x.Title = item.Title;

            return Convert(repos.Update(x, item.Item.Timestamp));
        }

        public ProjectViewModel Create(ProjectViewModel item)
        {
            var newItem = repos.Create(item.Item);

            return Convert(newItem);
        }

        public List<ProjectViewModel> FilteringEmploers(string y)
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
