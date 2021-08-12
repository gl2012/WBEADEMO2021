/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */

namespace WBEADMS
{
    public static class ModelsCommon
    {
        public static string FetchConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["ASIDEV11"].ToString();
        }

        public static string FetchMysqlConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["mysqlconnectionString"].ToString();
        }


        // TODO: if this is not referenced anywhere, then remove it
        public static string ProcessStringInput(string input)
        {
            if (input != null)
            {
                input = input.Trim();
                if (input == "")
                {
                    input = null;
                }
            }

            return input;
        }
    }
}