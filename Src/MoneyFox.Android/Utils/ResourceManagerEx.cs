using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MoneyFox.Droid.Utils
{
    internal static class ResourceManagerEx
    {
        internal static int IdFromTitle(string title, Type type)
        {
            string name = Path.GetFileNameWithoutExtension(title);
            int id = GetId(type, name);
            return id; // Resources.System.GetDrawable (Resource.Drawable.dashboard);
        }

        static int GetId(Type type, string propertyName)
        {
            FieldInfo[] props = type.GetFields();
            FieldInfo prop = props.Select(p => p).FirstOrDefault(p => p.Name == propertyName);
            if (prop != null)
                return (int) prop.GetValue(type);
            return 0;
        }
    }
}