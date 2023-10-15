using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Vs.Pm.Web.Pages.Users;

namespace Vs.Pm.Web.Data.Service
{
    public class StatusService
    {
        private static VsPmContext DbContext;
        EFRepository<Status> mRepoStatus;
        private string _user;

        public StatusService(VsPmContext context, IHttpContextAccessor httpContextAccessor)
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
            mRepoStatus = new EFRepository<Status>(context,_user);
        }
        public List<StatusViewModel> GetAll()
        {
            var list = mRepoStatus.Get().ToList();
            var result = list.Select(Convert).ToList();
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
            var model = mRepoStatus.Reload(item.StatusId);
            if (model == null)
            {
                return null;
            }
            return Convert(model);
        }

        public void Delete(StatusViewModel item)
        {
            var x = mRepoStatus.FindById(item.StatusId);
            mRepoStatus.Remove(x);
        }

        public StatusViewModel Update(StatusViewModel item)
        {
            var model = mRepoStatus.FindByIdForReload(item.StatusId);

            model.Title = item.Title;
            model.OrderId = item.OrderId;

            return Convert(mRepoStatus.Update(model, item.Item.Timestamp));
        }

        public StatusViewModel Create(StatusViewModel item)
        {
            var newItem = mRepoStatus.Create(item.Item);

            return Convert(newItem);
        }

        public List<StatusViewModel> FilteringEmploers(string filterValue)
        {
            var filteredListRooms = mRepoStatus.GetQuery().Where(x => (x.Title.StartsWith(filterValue))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
        public string GetName(int id)
        {
            var model = mRepoStatus.FindById(id);
            return model.Title;
        }
    }
}
