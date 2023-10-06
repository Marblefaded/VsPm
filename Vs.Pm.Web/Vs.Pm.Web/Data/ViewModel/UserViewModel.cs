using System.ComponentModel.DataAnnotations;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web.Data.ViewModel
{
    public class UserViewModel
    {
        private User _item;
        public User Item => _item;

        public UserViewModel()
        {
            _item = new User();

        }

        public UserViewModel(User item)
        {
            _item = item;
        }

        [Key]
        public int UserId
        {
            get => _item.UserId;
            set => _item.UserId = value;
        }
        [Required]
        [MinLength(2)]
        public string PersonName
        {
            get => _item.PersonName;
            set => _item.PersonName = value;
        }
        [Required]
        [MinLength(2)]
        public string PersonSurname
        {
            get => _item.PersonSurname;
            set => _item.PersonSurname = value;
        }
        [Required]
        [MinLength(2)]
        public string Password
        {
            get => _item.Password;
            set => _item.Password = value;
        }
        [Required]
        public int RoleId
        {
            get => _item.RoleId;
            set => _item.RoleId = value;
        }
        public string? ChangeLogJson
        {
            get => _item.ChangeLogJson;
            set => _item.ChangeLogJson = value;
        }
        public string Login
        {
            get => _item.Login;
            set => _item.Login = value;
        }
    }
}
