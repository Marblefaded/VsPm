using Vs.Pm.Pm.Db;
using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Web.Data.ViewModel;

namespace Vs.Pm.Web.Data.Service
{
    public class UserService
    {
        private static VsPmContext DbContext;
        EFRepository<User> mRepoUser;

        public UserService(VsPmContext context)
        {
            DbContext = context;
            mRepoUser = new EFRepository<User>(context);
        }
        public List<UserViewModel> GetAll()
        {
            var list = mRepoUser.Get().ToList();
            var result = list.Select(Convert).ToList();
            return result;
        }

        private static UserViewModel Convert(User Model)
        {
            var item = new UserViewModel(Model);
            return item;
        }

        public UserViewModel ReloadItem(UserViewModel item)
        {
            var x = mRepoUser.Reload(item.UserId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(UserViewModel item)
        {
            var x = mRepoUser.FindById(item.UserId);
            mRepoUser.Remove(x);
        }

        public UserViewModel Update(UserViewModel item)
        {
            var x = mRepoUser.FindByIdForReload(item.UserId);
            x.PersonName = item.PersonName;
            x.PersonSurname = item.PersonSurname;
            x.Password = item.Password;
            x.RoleId = item.RoleId;
            x.Login = item.Login;

            return Convert(mRepoUser.Update(x, item.Item.Timestamp));
        }

        public UserViewModel Create(UserViewModel item)
        {
            var newItem = mRepoUser.Create(item.Item);

            return Convert(newItem);
        }

    }
}
