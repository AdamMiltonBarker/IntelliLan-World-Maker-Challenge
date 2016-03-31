using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace TIA.Classes
{

    class TechBubbleIoT
    {
        public SerialDevice device;
        public DataReader dataReader;
        public DataWriter dataWriter;
        public ObservableCollection<DeviceInformation> listOfDevices;

        public async Task<SerialDevice> findSerialDevice(String comDevice)
        {
            System.Diagnostics.Debug.WriteLine("FOUND COM " + comDevice);
            device = await SerialDevice.FromIdAsync(comDevice);
            if (device != null)
            {
                System.Diagnostics.Debug.WriteLine("FOUND DEVICE " + device);
                return device;

            }
            else {

                System.Diagnostics.Debug.WriteLine("null 1 ");
                return null;

            }
        }

        public async void findSerialDevices(CollectionViewSource DeviceListSource)
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);

                for (int i = 0; i < dis.Count; i++)
                {
                    listOfDevices.Add(dis[i]);
                }

                DeviceListSource.Source = listOfDevices;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            System.Diagnostics.Debug.WriteLine(listOfDevices);
        }

        public void setDataReader(SerialDevice device)
        {
            device.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            device.BaudRate = 9600;
            device.StopBits = SerialStopBitCount.One;
            device.DataBits = 8;
            device.Parity = SerialParity.None;
            device.Handshake = SerialHandshake.None;
            dataReader = new DataReader(device.InputStream);
        }

        public void setDataWriter(SerialDevice device)
        {
            dataWriter = new DataWriter(device.OutputStream);
        }
    }
}
