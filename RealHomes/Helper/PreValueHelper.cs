using System;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Web.Mvc;
using System.Web.Configuration;

namespace RealHomes.Helper
{
    public class PreValueHelper
    {
        private const string APP_SETTING_ERROR_MESSAGE = "Invalid or missing appSetting, ";

        public List<SelectListItem> GetPreValuesFromDataTypeId(int dataTypeId)
        {
            List<SelectListItem> preValueSelectorList = new List<SelectListItem>();

            XPathNodeIterator iterator = umbraco.library.GetPreValues(dataTypeId);
            iterator.MoveNext();
            XPathNodeIterator preValues = iterator.Current.SelectChildren("preValue", "");

            while (preValues.MoveNext())
            {
                string preValueIdAsString = preValues.Current.GetAttribute("id", "");
                int preValueId = 0;
                int.TryParse(preValueIdAsString, out preValueId);
                string preValue = preValues.Current.Value;
                preValueSelectorList.Add(new SelectListItem { Value = preValueId.ToString(), Text = preValue }); //you could use the preValueId for the value here if you want the value to be a number
            }

            return preValueSelectorList;
        }


        public List<SelectListItem> GetPreValuesFromAppSettingName(string appSettingName)
        {
            int dataTypeId = GetIntFromAppSetting(appSettingName);
            List<SelectListItem> preValues = GetPreValuesFromDataTypeId(dataTypeId);
            return preValues;
        }

        private int GetIntFromAppSetting(string appSettingName)
        {
            int intValue = 0;
            string setting = GetStringFromAppSetting(appSettingName);
            if (!int.TryParse(setting, out intValue))
            {
                throw new Exception(APP_SETTING_ERROR_MESSAGE + appSettingName);
            }
            return intValue;
        }

        private string GetStringFromAppSetting(string appSettingName)
        {
            string setting = WebConfigurationManager.AppSettings[appSettingName] as string;
            if (String.IsNullOrEmpty(setting))
            {
                throw new Exception(APP_SETTING_ERROR_MESSAGE + appSettingName);
            }
            return setting;
        }




    }
}


