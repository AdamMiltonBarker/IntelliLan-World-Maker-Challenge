using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIA.Models
{
    public class Device
     {
        public int deviceID { get; set; }
        public string deviceTitle { get; set; }
        public string deviceType { get; set; }
     }

    public class DeviceManager
    {
        public static List<Device> GetDeviceData()
        {
            var devices = new List<Device>();

            devices.Add(new Device { deviceID = 1, deviceTitle = "Vulpate", deviceType = "Futurum" });
            devices.Add(new Device { deviceID = 2, deviceTitle = "Mazim", deviceType = "Sequiter Que" });
            devices.Add(new Device { deviceID = 3, deviceTitle = "Elit", deviceType = "Tempor" });

            return devices;
        }
    }
}
