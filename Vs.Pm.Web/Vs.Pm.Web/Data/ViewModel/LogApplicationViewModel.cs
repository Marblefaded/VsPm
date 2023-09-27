using Vs.Pm.Pm.Db.Models;
using System.ComponentModel.DataAnnotations;

namespace Vs.Pm.Web.Data.ViewModel
{
    public class LogApplicationViewModel
    {
        private LogApplicationError _item;
        public LogApplicationError Item => _item;

        public LogApplicationViewModel()
        {
            _item = new LogApplicationError();

        }

        public LogApplicationViewModel(LogApplicationError item)
        {
            _item = item;
        }

        [Key]
        public int LogApplicationId
        {
            get => _item.LogApplicationErrorId;
            set => _item.LogApplicationErrorId = value;
        }

        public string ErrorMessage
        {
            get => _item.ErrorMessage;
            set => _item.ErrorMessage = value;
        }
        public string ErrorContext
        {
            get => _item.ErrorContext;
            set => _item.ErrorContext = value;
        }
        public string? ErrorInnerException
        {
            get => _item.ErrorInnerException;
            set => _item.ErrorInnerException = value;
        }
        public DateTime Date
        {
            get => _item.InsertDate;
            set => _item.InsertDate = value;
        }

    }
}
