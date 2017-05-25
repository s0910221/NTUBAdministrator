using System.Collections.Generic;
using System.Linq;
using NTUBAdministrator.Models.Interface;
using NTUBAdministrator.ViewModels;

namespace NTUBAdministrator.Models.Repository
{
    public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
    {
        public Activity GetActivityByID(int activityID)
        {
            return this.Get(x => x.ActivityID == activityID);
        }

        public IEnumerable<ActivityCalendarViewModel> GetActivityCalendarEvent()
        {
            return this.GetAll().Where(x => x.Status == "1").Select(c => new ActivityCalendarViewModel()
            {
                ActivityName = c.ActivityName,
                ActivityStartTime = c.ActivityStartTime,
                ActivityEndTime = c.ActivityEndTime,
                Url = "/Activity/ActivityCalendarDetail/" + c.ActivityID
            });
        }

        public IEnumerable<ActivityManagementViewModel> GetAllActivityManagementView()
        {
            return this.GetAll().Select(x => new ActivityManagementViewModel()
            {
                ActivityID = x.ActivityID,
                ActivityName = x.ActivityName,
                Status = x.Status
            });
        }

        public IEnumerable<ActivityManagementViewModel> GetActivityManagementViewByCreateID(string accountID)
        {
            return this.GetAll().Where(x => x.CreateID == accountID).Select(c => new ActivityManagementViewModel()
            {
                ActivityID = c.ActivityID,
                ActivityName = c.ActivityName,
                Status = c.Status
            });
        }
    }
}