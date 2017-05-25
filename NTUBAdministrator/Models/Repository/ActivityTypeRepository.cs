using System.Linq;
using NTUBAdministrator.Models.Interface;

namespace NTUBAdministrator.Models.Repository
{
    public class ActivityTypeRepository : GenericRepository<ActivityType>, IActivityTypeRepository
    {
        public IQueryable GetActivityTypeList()
        {
            return this.GetAll().Select(x => new { x.ActivityTypeID, x.TypeName });
        }
    }
}