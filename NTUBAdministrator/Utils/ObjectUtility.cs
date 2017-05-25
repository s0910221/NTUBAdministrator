using System.Reflection;
//沒用到
namespace NTUBAdministrator.Utils
{
    public class ObjectUtility
    {
        public static void CloneObject(object dest, object src)
        {
            foreach (PropertyInfo p in dest.GetType().GetProperties())
            {
                p.SetValue(dest, p.GetValue(src, null), null);
            }
        }

    }
}