//沒用到
namespace NTUBAdministrator.Utils
{
    public class SessionData
    {
        private string AccountID { get; set; }
        private string UserName { get; set; }

        public SessionData(string accountID, string userName)
        {
            this.AccountID = accountID;
            this.UserName = userName;
        }
    }
}