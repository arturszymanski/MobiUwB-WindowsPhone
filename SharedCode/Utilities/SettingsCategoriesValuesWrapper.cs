using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Utilities
{
    public class SettingsCategoriesValuesWrapper
    {
        private Dictionary<String, object> _settingsCategoriesValues;

        public Dictionary<String, object> SettingsCategoriesValues
        {
            get { return _settingsCategoriesValues; }
            set { _settingsCategoriesValues = value; }
        }

        public void AddValue(String key, object value)
        {
            _settingsCategoriesValues.Add(key, value);
        }

        public object GetValue(String key)
        {
            return _settingsCategoriesValues[key];
        }

        public SettingsCategoriesValuesWrapper()
        {
            _settingsCategoriesValues = new Dictionary<String, object>();
        }
    }
}
