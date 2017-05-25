using NTUBAdministrator.Models.Interface;

namespace NTUBAdministrator.Models.Repository
{
    public class ActivityApplicantRepository : GenericRepository<ActivityApplicant>, IActivityApplicantRepository
    {
        public ActivityApplicant GetActivityApplicant(string accountID, int activityID)
        {
            return this.Get(x => x.ApplyID == accountID && x.ActivityID == activityID);
        }
    }
}