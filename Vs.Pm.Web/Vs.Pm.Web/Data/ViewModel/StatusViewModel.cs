using System.ComponentModel.DataAnnotations;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web.Data.ViewModel
{
    public class StatusViewModel
    {
        private Status _item;
        public Status Item => _item;

        public StatusViewModel()
        {
            _item = new Status();

        }

        public StatusViewModel(Status item)
        {
            _item = item;
        }

        [Key]
        public int StatusId
        {
            get => _item.StatusId;
            set => _item.StatusId = value;
        }

        public string Title
        {
            get => _item.Title;
            set => _item.Title = value;
        }

        public string? ChangeLogJson
        {
            get => _item.ChangeLogJson;
            set => _item.ChangeLogJson = value;
        }

        public int OrderId
        {
            get => _item.OrderId;
            set => _item.OrderId = value;
        }

        public bool IsDeleteEnabled { get; set; } = true;
    }
}
