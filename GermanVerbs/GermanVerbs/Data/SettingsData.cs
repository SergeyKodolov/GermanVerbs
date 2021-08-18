using GermanVerbs.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GermanVerbs.Data
{
    public static class SettingsData
    {
        public static Dictionary<string, PropertyInfo> Settings { get; set; }

        static SettingsData()
        {
            Settings = new Dictionary<string, PropertyInfo>();

            var propertiesList = typeof(Conjugation).GetProperties().Skip(1).Take(4).ToList();

            foreach (var prop in propertiesList)
            {
                var name = ((DescriptionAttribute)prop.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;

                Settings[name] = prop;
            }
        }
    }
}
