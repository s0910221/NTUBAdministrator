using NTUBAdministrator.Models.Interface;

namespace NTUBAdministrator.Models.Repository
{
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccount getUserAccountByID(string accountID)
        {
            return this.Get(x => x.AccountID == accountID);
        }
    }
}