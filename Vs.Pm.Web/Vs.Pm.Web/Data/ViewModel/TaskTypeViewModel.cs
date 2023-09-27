using System.ComponentModel.DataAnnotations;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web.Data.ViewModel
{
    public class TaskTypeViewModel
    {
        private TaskType _item;
        public TaskType Item => _item;

        public TaskTypeViewModel()
        {
            _item = new TaskType();

        }

        public TaskTypeViewModel(TaskType item)
        {
            _item = item;
        }

        [Key]
        public int TaskTypeId
        {
            get => _item.TaskTypeId;
            set => _item.TaskTypeId = value;
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

        public bool IsDeleteEnabled { get; set; } = true;
    }
}
