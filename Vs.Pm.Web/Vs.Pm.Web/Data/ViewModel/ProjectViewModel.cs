using System.ComponentModel.DataAnnotations;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web.Data.ViewModel
{
    public class ProjectViewModel
    {

        private Project _item;
        public Project Item => _item;

        public ProjectViewModel()
        {
            _item = new Project();

        }

        public ProjectViewModel(Project item)
        {
            _item = item;
        }

        [Key]
        public int ProjectId
        {
            get => _item.ProjectId;
            set => _item.ProjectId = value;
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
        /*public bool IsConcurency { get; set; }

        public string ConcurencyErrorText { get; set; }
*/
        public bool IsDeleteEnabled { get; set; } = true;
    }
}
