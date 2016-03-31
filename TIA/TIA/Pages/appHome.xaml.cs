using TIA.Classes;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Core;
using Windows.Media.SpeechRecognition;

namespace TIA
{
    /// <summary>
    /// Hackster/Microsoft/Arduino World Maker Challenge Entry By Adam Milton-Barker
    /// TIA, the TechBubble Intelligent Assistant for the IntelliLan IoT Network
    /// </summary>

    public sealed partial class appHome : Page
    {
        TechBubbleSpeech TechBubbleSpeech = new TechBubbleSpeech();
        TechBubbleIoTConfigs TechBubbleIoTConfigs = new TechBubbleIoTConfigs();
        TechBubbleWeb TechBubbleWeb = new TechBubbleWeb();
        TIASiml TIASiml = new TIASiml();

        private CoreDispatcher dispatcher;
        private SpeechRecognizer speechRecognizer;

        public appHome()
        {
            this.InitializeComponent();
            TIASiml.initiateSIML();
            readDeviceData();
            initializeSpeechRec();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(appHome));

        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {

            TIASiml.simlCommunicate("RELOADDATA");
            DoorLoadingBar.IsEnabled = true;
            DoorLoadingBar.Visibility = Visibility.Visible;
            Door.Visibility = Visibility.Collapsed;

            ProximityLoadingBar.IsEnabled = true;
            ProximityLoadingBar.Visibility = Visibility.Visible;
            Proximity.Visibility = Visibility.Collapsed;

            TemperatureLoadingBar.IsEnabled = true;
            TemperatureLoadingBar.Visibility = Visibility.Visible;
            Temperature.Visibility = Visibility.Collapsed;

            readDeviceData();

        }

        public async void readDeviceData()
        {
            string[] apiParams = {
                    TechBubbleIoTConfigs.deviceAddress,
                    "/?ReadData",
                    "",
                    "application/json",
                    ""
            };

            try
            {

                JObject apiResponse = await TechBubbleWeb.jsonApiCall(apiParams);
                System.Diagnostics.Debug.WriteLine(apiResponse);

                if ((string)apiResponse["Response"] == "OK")
                {

                    Door.Text = (string)apiResponse["ResponseData"]["DoorSense"]["Status"];

                    Proximity.Text = (string)apiResponse["ResponseData"]["Proximity"]["Status"];
                    ProximityMax.Text = (string)apiResponse["ResponseData"]["Proximity"]["Max"];

                    Temperature.Text = (string)apiResponse["ResponseData"]["Temperature"]["Status"];
                    TemperatureMax.Text = (string)apiResponse["ResponseData"]["Temperature"]["Max"];

                    DoorLoadingBar.IsEnabled = false;
                    DoorLoadingBar.Visibility = Visibility.Collapsed;
                    Door.Visibility = Visibility.Visible;

                    ProximityLoadingBar.IsEnabled = false;
                    ProximityLoadingBar.Visibility = Visibility.Collapsed;
                    Proximity.Visibility = Visibility.Visible;

                    TemperatureLoadingBar.IsEnabled = false;
                    TemperatureLoadingBar.Visibility = Visibility.Collapsed;
                    Temperature.Visibility = Visibility.Visible;

                    if ((string)apiResponse["ResponseData"]["DoorSense"]["Status"] == "OPEN")
                    {

                        Door.Text = "OPEN";
                        FirstGrid.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Door.Text = "CLOSED";
                        FirstGrid.Background = new SolidColorBrush(Colors.SteelBlue);

                    }

                    if ((string)apiResponse["ResponseData"]["Proximity"]["Status"] == "WARNING")
                    {

                        Proximity.Text = "WARNING";
                        SecondGrid.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Proximity.Text = "OK";
                        SecondGrid.Background = new SolidColorBrush(Colors.SteelBlue);

                    }

                    if ((string)apiResponse["ResponseData"]["Temperature"]["Status"] == "WARNING")
                    {
                        Temperature.Text = "WARNING";
                        ThirdGrid.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Temperature.Text = "OK";
                        ThirdGrid.Background = new SolidColorBrush(Colors.SteelBlue);

                    }


                }

            }
            catch (Exception)
            {
                readDeviceData();
            }
        }

        private void Proximity_Click(object sender, RoutedEventArgs e)
        {

            ProximityLoadingBar.IsEnabled = true;
            ProximityLoadingBar.Visibility = Visibility.Visible;
            Proximity.Visibility = Visibility.Collapsed;

            updateProximity(ProximityMax.Text);

        }

        async void updateProximity(String maxSetting)
        {
            string[] apiParams = {
                    TechBubbleIoTConfigs.deviceAddress,
                    "/?MaxProximity="+maxSetting+"S",
                    "",
                    "application/json",
                    ""
            };

            try
            {

                JObject apiResponse = await TechBubbleWeb.jsonApiCall(apiParams);
                System.Diagnostics.Debug.WriteLine(apiResponse);

                ProximityMax.Text = (string)apiResponse["NewValue"];

                ProximityLoadingBar.IsEnabled = false;
                ProximityLoadingBar.Visibility = Visibility.Collapsed;
                Proximity.Visibility = Visibility.Visible;

                if ((string)apiResponse["Warn"] == "WARNING")
                {
                    Proximity.Text = "WARNING";
                    SecondGrid.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Proximity.Text = "OK";
                    SecondGrid.Background = new SolidColorBrush(Colors.SteelBlue);

                }

            }
            catch (Exception)
            {
                updateProximity(maxSetting);
            }
        }

        private void Temperature_Click(object sender, RoutedEventArgs e)
        {

            TIASiml.simlCommunicate("SETMAXTEMP");
            TemperatureLoadingBar.IsEnabled = true;
            TemperatureLoadingBar.Visibility = Visibility.Visible;
            Temperature.Visibility = Visibility.Collapsed;

            updateTemperature(TemperatureMax.Text);

        }

        async void updateTemperature(String maxSetting)
        {
            string[] apiParams = {
                    TechBubbleIoTConfigs.deviceAddress,
                    "/?MaxTemp="+maxSetting+"S",
                    "",
                    "application/json",
                    ""
            };

            try
            {

                JObject apiResponse = await TechBubbleWeb.jsonApiCall(apiParams);
                System.Diagnostics.Debug.WriteLine(apiResponse);

                TemperatureMax.Text = (string)apiResponse["NewValue"];

                TemperatureLoadingBar.IsEnabled = false;
                TemperatureLoadingBar.Visibility = Visibility.Collapsed;
                Temperature.Visibility = Visibility.Visible;

                if ((string)apiResponse["Warn"] == "WARNING")
                {
                    Temperature.Text = "WARNING";
                    ThirdGrid.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Temperature.Text = "OK";
                    ThirdGrid.Background = new SolidColorBrush(Colors.SteelBlue);

                }

            }
            catch (Exception)
            {
                updateTemperature(maxSetting);
            }
        }
        public void commandCheck(string text)
        {
            if (text.Contains("reload data"))
            {
                readDeviceData();

            }
            else if (text.Contains("update proximity"))
            {
                string command = text.Replace("update proximity ", "");
                updateProximity(command);

            }
            else if (text.Contains("update temperature"))
            {
                string command = text.Replace("update temperature ", "");
                updateTemperature(command);

            }
        }

        public async void initializeSpeechRec()
        {
            TIASiml.initiateSIML();
            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
            if (!permissionGained)
            {
                MessageDialog("Permission to access capture resources was not given by the user, reset the application setting in Settings->Privacy->Microphone.");
            }

            this.dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            this.speechRecognizer = new SpeechRecognizer();
            speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;
            SpeechRecognitionCompilationResult result = await speechRecognizer.CompileConstraintsAsync();
            speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
            speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
            await speechRecognizer.ContinuousRecognitionSession.StartAsync();

        }

        public async void startSpeechRec()
        {
            await speechRecognizer.ContinuousRecognitionSession.StartAsync();
        }

        public async void cancelSpeechRec()
        {
            await speechRecognizer.ContinuousRecognitionSession.CancelAsync();
        }

        private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {

            if (args.Result.Confidence == SpeechRecognitionConfidence.Medium || args.Result.Confidence == SpeechRecognitionConfidence.High)
            {

                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    System.Diagnostics.Debug.WriteLine("OUTPUTTING REC");
                    TechBubbleSpeech.Speak(TIASiml.simlCommunicate(args.Result.Text));
                    commandCheck(args.Result.Text);

                });
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    System.Diagnostics.Debug.WriteLine("OUTPUTTING REC2");
                    TechBubbleSpeech.Speak(TIASiml.simlCommunicate(args.Result.Text));
                    commandCheck(args.Result.Text);
                });
            }
        }
        private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            if (args.Status != SpeechRecognitionResultStatus.Success)
            {
                if (args.Status == SpeechRecognitionResultStatus.TimeoutExceeded)
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        initializeSpeechRec();
                    });
                }
                else
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        initializeSpeechRec();
                    });
                }
            }
        }
        private async void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                System.Diagnostics.Debug.WriteLine(args.State.ToString());
            });
        }

        private async void MessageDialog(string message)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(message);
            await messageDialog.ShowAsync();
        }

    }
}