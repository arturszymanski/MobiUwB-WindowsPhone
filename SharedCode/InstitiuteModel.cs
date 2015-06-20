#region

using System;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace SharedCode
{
    public class InstitiuteModel
    {
        public readonly Website Website;

        public String Name
        {
            get
            {
                return Website.Name;
            }
        }
        
        public String Page 
        { 
            get 
            {
                return Website.Url;
            }
        }

        public String IconPath { get; private set; }

        public InstitiuteModel(Website website, 
            String iconPath)
        {
            Website = website;
            IconPath = iconPath;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InstitiuteModel))
            {
                return false;
            }
            InstitiuteModel concreteObj = (InstitiuteModel) obj;
            return concreteObj.Website.Id.Equals(Website.Id);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
