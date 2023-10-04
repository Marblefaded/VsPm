using Vs.Pm.Pm.Db;
using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.Service
{
    public class ProjectService
    {
        private static VsPmContext DbContext;
        EFRepository<Project> mRepoProject;

        public ProjectService(VsPmContext context)
        {
            DbContext = context;
            mRepoProject = new EFRepository<Project>(context);
        }
        public List<ProjectViewModel> GetAll()
        {
            var list = mRepoProject.Get().ToList();
            var result = list.Select(Convert).ToList();
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
            var x = mRepoProject.Reload(item.ProjectId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(ProjectViewModel item)
        {
            var x = mRepoProject.FindById(item.ProjectId);
            mRepoProject.Remove(x);
        }

        public ProjectViewModel Update(ProjectViewModel item)
        {
            var x = mRepoProject.FindByIdForReload(item.ProjectId);
            x.Title = item.Title;
            

            return Convert(mRepoProject.Update(x, item.Item.Timestamp));
        }

        public ProjectViewModel Create(ProjectViewModel item)
        {
            var newItem = mRepoProject.Create(item.Item);

            return Convert(newItem);
        }

        public List<ProjectViewModel> FilteringEmploers(string filterValue)
        {
            var filteredListRooms = mRepoProject.GetQuery().Where(x => (x.Title.StartsWith(filterValue))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }

        public string GetName(int id)
        {
            var listProject = mRepoProject.FindById(id);
            return listProject.Title;
        }
    }
}
