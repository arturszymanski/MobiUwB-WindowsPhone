#region

using System;
using System.Collections.Generic;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace SharedCode.Parsers
{
    public static class ParserFactory
    {
        public static List<InstitiuteModel> GenerateInstituteModels(
            List<Website> websites, String imagePath)
        {
            //TODO odnaleźć jaka to jest jednostka i pobrać z niej string z logiem 
            List<InstitiuteModel> modelsList = new List<InstitiuteModel>();
            foreach (var website in websites)
            {
                modelsList.Add(GenerateInstituteModel(website, imagePath));
            }
            return modelsList;
        }


        public static List<InstitiuteModel> GenerateInstituteModels(
            Websites websites, String imagePath)
        {
            return GenerateInstituteModels(websites.WebsiteList, imagePath);
        }


        public static InstitiuteModel GenerateInstituteModel(
            Website website, String imagePath)
        {
            return new InstitiuteModel(website, imagePath);
        }


        public static InstitiuteModel FindWrapperBy(
            Website website, 
            List<InstitiuteModel> wrappers)
        {
            foreach (var wrapper in wrappers)
            {
                if (wrapper.Website.Id.Equals(website.Id))
                {
                    return wrapper;
                }
            }
            return null;
        }

    
    
    }
}
