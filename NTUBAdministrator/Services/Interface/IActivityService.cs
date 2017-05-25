using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NTUBAdministrator.ViewModels;

namespace NTUBAdministrator.Services.Interface
{
    public interface IActivityService
    {
        JsonResult GetActivityCalendarEvent();
        ActivityCalendarDetailViewModel GetActivityCalendarDetail(string accountID, int id);
        IResult ApplyActivity(string accountID, ActivityCalendarDetailViewModel viewModel);
        IResult CancelApplyActivity(string accountID, ActivityCalendarDetailViewModel viewModel);
        IEnumerable<ActivityManagementViewModel> GetActivityManagement(string accountID);
        IResult AddActivityType(string accountID, ActivityTypeViewModel viewModel);
        IResult AddActivity(string accountID, ActivityViewModel viewModel);
        IQueryable GetActivityTypeList();
        ActivityCheckViewModel GetActivityCheckView(string accountID, int id);
        IResult CheckActivity(string accountID, ActivityCheckViewModel viewModel);
    }
}
