using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIA.Classes
{
    class TechBubbleIoTConfigs
    {

        public static class userSession
        {
            public static string status { get; set; }
        }

        // USER DETAILS
        public string userName = "YOURUSERNAME";
        public string userNFC = "YOURNFCCHIPID";
        public string userPassword = "YOURPASSWORD";

        // DEVICE DETAILS
        public string deviceAddress = "YOURDOORGUARDIP";

    }
}
