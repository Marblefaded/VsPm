using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web
{
    public static class Globals
    {
        public static List<UserRole> UserList => InitUnterrichtsmodellList();
        public static List<UserRole> InitUnterrichtsmodellList()
        {
            var list = new List<UserRole>
            {
                new UserRole {RoleId = 1, RoleName = "User"},
                new UserRole {RoleId = 2, RoleName = "Admin"},
            };

            return list;
        }
    }
}
