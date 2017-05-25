using System.Linq;

namespace NTUBAdministrator.Models.Interface
{
    public interface IActivityTypeRepository: IRepository<ActivityType>
    {
        IQueryable GetActivityTypeList();
    }
}
