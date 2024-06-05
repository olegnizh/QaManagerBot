using System.Configuration;

namespace QaManagerBot
{
    static class AppSetting
    {

        static public string GetConnectionString()
        {
            string s = "";
            s  = ConfigurationSettings.AppSettings["StringConnection"];
            return s;
        }

        static public string GetQatableName()
        {
            string s = "";
            s = ConfigurationSettings.AppSettings["qatable"];
            return s;
        }

        
    }
}
