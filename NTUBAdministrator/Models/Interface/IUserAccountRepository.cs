namespace NTUBAdministrator.Models.Interface
{
    public interface IUserAccountRepository : IRepository<UserAccount>
    {
        UserAccount getUserAccountByID(string accountID);
    }
}
