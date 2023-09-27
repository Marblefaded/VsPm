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

        public string Description
        {
            get => _item.Description;
            set => _item.Description = value;
        }

        public int ProjectId
        {
            get => _item.ProjectId;
            set => _item.ProjectId = value;
        }
        public int StatusId
        {
            get => _item.StatusId;
            set => _item.StatusId = value;
        }
        public int TaskTypeId
        {
            get => _item.TaskTypeId;
            set => _item.TaskTypeId = value;
        }
    }
}
