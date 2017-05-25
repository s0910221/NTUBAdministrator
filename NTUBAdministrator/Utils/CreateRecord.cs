using System;
//沒用到
namespace NTUBAdministrator.Utils
{
    public class CreateRecord<T> where T : class
    {
        
        public T AddRecord(T t, string accountID)
        {
            if (accountID != null)
            {
                t.GetType().GetProperty("CreateID").SetValue(t, accountID);
                t.GetType().GetProperty("ModifyID").SetValue(t, accountID);
            }
            DateTime time = DateTime.Now;
            t.GetType().GetProperty("CreateTime").SetValue(t, time);
            t.GetType().GetProperty("ModifyTime").SetValue(t, time);
            return t;
        }
    }
}