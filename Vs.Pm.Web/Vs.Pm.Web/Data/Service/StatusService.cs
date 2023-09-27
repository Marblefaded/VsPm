using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.Service
{
    public class StatusService
    {
        private static VsPmContext DbContext;
        EFRepository<Status> repos;

        public StatusService(VsPmContext context)
        {
            DbContext = context;
            repos = new EFRepository<Status>(context);
        }
        public List<StatusViewModel> GetAll()
        {
            var list = repos.Get().ToList();
            var result = list.Select(x => Convert(x)).ToList();
            foreach (var item in result)
            {
                item.IsDeleteEnabled = DbContext.IsClaimDeleteEnabled(item.StatusId);
            }
            return result;
        }

        private static StatusViewModel Convert(Status Model)
        {
            var item = new StatusViewModel(Model);
            return item;
        }

        public StatusViewModel ReloadItem(StatusViewModel item)
        {
            var model = repos.Reload(item.StatusId);
            if (model == null)
            {
                return null;
            }
            return Convert(model);
        }

        public void Delete(StatusViewModel item)
        {
            var x = repos.FindById(item.StatusId);
            repos.Remove(x);
        }

        public StatusViewModel Update(StatusViewModel item)
        {
            var x = repos.FindByIdForReload(item.StatusId);

            x.Title = item.Title;
            x.OrderId = item.OrderId;

            return Convert(repos.Update(x, item.Item.Timestamp));
        }

        public StatusViewModel Create(StatusViewModel item)
        {
            var newItem = repos.Create(item.Item);

            return Convert(newItem);
        }

        public List<StatusViewModel> FilteringEmploers(string y)
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
