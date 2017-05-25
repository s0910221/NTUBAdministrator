namespace NTUBAdministrator.Models.Interface
{
    public interface IActivityApplicantRepository : IRepository<ActivityApplicant>
    {
        ActivityApplicant GetActivityApplicant(string accountID, int activityID);
    }
}
