using NTUBAdministrator.ViewModels;
using System.Collections.Generic;

namespace NTUBAdministrator.Models.Interface
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Activity GetActivityByID(int activityID);
        IEnumerable<ActivityCalendarViewModel> GetActivityCalendarEvent();
        IEnumerable<ActivityManagementViewModel> GetAllActivityManagementView();
        IEnumerable<ActivityManagementViewModel> GetActivityManagementViewByCreateID(string accountID);
    }
}
