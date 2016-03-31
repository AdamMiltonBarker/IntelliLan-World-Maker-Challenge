using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TIA.Classes;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TIA
{
    /// <summary>
    /// Hackster/Microsoft/Arduino World Maker Challenge Entry By Adam Milton-Barker
    /// TIA, the TechBubble Intelligent Assistant for the IntelliLan IoT Network
    /// </summary>
    
    public sealed partial class nfcLogin : Page
    {
        TechBubbleIoTConfigs TechBubbleIoTConfigs = new TechBubbleIoTConfigs();
        TechBubbleIoT TechBubbleIoT = new TechBubbleIoT();
        TechBubbleSpeech TechBubbleSpeech = new TechBubbleSpeech();
        TechBubbleIoTHelpers TechBubbleIoTHelpers = new TechBubbleIoTHelpers();
        TIASiml TIASiml = new TIASiml();

        public nfcLogin()
        {
            this.InitializeComponent();
            TechBubbleIoT.listOfDevices = new ObservableCollection<DeviceInformation>();
            TechBubbleIoT.findSerialDevices(DeviceListSource);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));

        }


        private async void loginContinueClick(object sender, RoutedEventArgs e)
        {

            TechBubbleIoTHelpers.iniateLoader(LoadingBar);
            TechBubbleIoTHelpers.collapseButton(loginContinue);
            var selection = ComInput.SelectedItems;

            if (selection.Count <= 0)
            {
                TechBubbleIoTHelpers.collapseLoader(LoadingBar);
                TechBubbleSpeech.Speak(TIASiml.simlCommunicate("AUTHWAYNFCRESPONSE"));
                TechBubbleIoTHelpers.initiateButton(loginContinue);
            }
            else
            {

                TechBubbleIoTHelpers.iniateLoader(LoadingBar);
                TechBubbleIoTHelpers.collapseButton(loginContinue);
                DeviceInformation entry = (DeviceInformation)selection[0];

                SerialDevice device = await TechBubbleIoT.findSerialDevice(entry.Id);
                String keyAuth;

                if (device != null)
                {

                    TechBubbleIoT.setDataReader(device);
                    TechBubbleIoT.setDataWriter(device);

                    char[] buffer = new char[100];
                    "S".CopyTo(0, buffer, 0, 1);
                    String InputString = new string(buffer);
                    TechBubbleIoT.dataWriter.WriteString(InputString);
                    await TechBubbleIoT.dataWriter.StoreAsync();

                    while (true)
                    {
                        var bytesRecieved = await TechBubbleIoT.dataReader.LoadAsync(124);
                        if (bytesRecieved > 0)
                        {
                            keyAuth = TechBubbleIoT.dataReader.ReadString(bytesRecieved);
                            System.Diagnostics.Debug.WriteLine(keyAuth);
                            if (keyAuth.Contains("FOUND"))
                            {
                                TechBubbleSpeech.Speak("Thank you, please wait");
                                await Task.Delay(2000);
                                keyAuth = keyAuth.Replace("FOUND", "");
                                keyAuth = keyAuth.Replace(" ", "");
                                keyAuth = keyAuth.Replace("Error.Failedreadpage4\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed20\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed19\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed18\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed17\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed16\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed15\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed14\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed13\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed12\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed11\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed10\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed9\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed8\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed7\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed6\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed5\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed4\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed3\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed2\r\n", "");
                                keyAuth = keyAuth.Replace("Readfailed1\r\n", "");
                                await Task.Delay(2000);
                                break;
                            }
                            else
                            {
                                TechBubbleSpeech.Speak(keyAuth);
                            }
                        }
                    }

                    keyAuth = keyAuth.TrimEnd(null);
                    if (keyAuth == TechBubbleIoTConfigs.userNFC)
                    {

                        TechBubbleIoTConfigs.userSession.status = "loggedIn";

                        char[] rbuffer = new char[100];
                        "K".CopyTo(0, rbuffer, 0, 1);
                        String RInputString = new string(rbuffer);
                        TechBubbleIoT.dataWriter.WriteString(RInputString);
                        await TechBubbleIoT.dataWriter.StoreAsync();

                        TechBubbleSpeech.Speak("Thank you, redirecting you to application homepage.");
                        await Task.Delay(2000);
                        TechBubbleIoT.dataReader.Dispose();
                        TechBubbleIoT.dataWriter.Dispose();
                        this.Frame.Navigate(typeof(appHome));
                    }
                    else
                    {

                        char[] rbuffer = new char[100];
                        "F".CopyTo(0, rbuffer, 0, 1);
                        String RInputString = new string(rbuffer);
                        TechBubbleIoT.dataWriter.WriteString(RInputString);
                        await TechBubbleIoT.dataWriter.StoreAsync();
                        TechBubbleSpeech.Speak("Sorry NFC key not valid");
                        await Task.Delay(2000);
                        TechBubbleIoT.dataReader.Dispose();
                        TechBubbleIoT.dataWriter.Dispose();
                        this.Frame.Navigate(typeof(nfcLogin));
                    }
                }
                else
                {
                    TechBubbleSpeech.Speak("Sorry I could not find any devices, please try again");
                    this.Frame.Navigate(typeof(nfcLogin));

                }

            }
        }
    }
}
