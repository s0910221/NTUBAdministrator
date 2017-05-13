using System;
using System.Reflection;

namespace NTUBAdministrator.Utils
{
    public static class CreateRecord
    {
        private static string CreateID;
        private static string ModifyID;
        private static DateTime? CreateTime;
        private static DateTime? ModifyTime;
        

        public static T AddRecord<T>(this T t,string accountID)
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