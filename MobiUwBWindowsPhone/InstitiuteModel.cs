#region

using System;
using System.Windows.Media;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace MobiUwB
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

        public ImageSource Icon { get; private set; }

        public InstitiuteModel(Website website, 
            ImageSource icon)
        {
            Website = website;
            Icon = icon;
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
