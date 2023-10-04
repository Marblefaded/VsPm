using System.ComponentModel.DataAnnotations;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web.Data.ViewModel
{
    public class TaskViewModel
    {
        private Pm.Db.Models.TaskModel _item;
        public Pm.Db.Models.TaskModel Item => _item;

        public TaskViewModel()
        {
            _item = new Pm.Db.Models.TaskModel();

        }

        public TaskViewModel(Pm.Db.Models.TaskModel item)
        {
            _item = item;
        }

        [Key]
        public int TaskId
        {
            get => _item.TaskId;
            set => _item.TaskId = value;
        }
        [Required]
        [MinLength(2)]
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
        [Required]
        [MinLength(2)]
        public string Description
        {
            get => _item.Description;
            set => _item.Description = value;
        }
        [Required]
        public int ProjectId
        {
            get => _item.ProjectId;
            set => _item.ProjectId = value;
        }
        [Required]
        public int StatusId
        {
            get => _item.StatusId;
            set => _item.StatusId = value;
        }
        [Required]
        public int TaskTypeId
        {
            get => _item.TaskTypeId;
            set => _item.TaskTypeId = value;
        }

        public TimeSpan? Hours
        {
            get => _item.Hours;
            set => _item.Hours = value;
        }
    }
}
