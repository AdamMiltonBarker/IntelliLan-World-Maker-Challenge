using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TIA.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TIA.Pages
{
    /// <summary>
    /// Hackster/Microsoft/Arduino World Maker Challenge Entry By Adam Milton-Barker
    /// TIA, the TechBubble Intelligent Assistant for the IntelliLan IoT Network
    /// </summary>
    
    public sealed partial class passwordLogin : Page
    {
        TechBubbleIoTConfigs TechBubbleIoTConfigs = new TechBubbleIoTConfigs();
        TechBubbleIoT TechBubbleIoT = new TechBubbleIoT();
        TechBubbleSpeech TechBubbleSpeech = new TechBubbleSpeech();
        TechBubbleIoTHelpers TechBubbleIoTHelpers = new TechBubbleIoTHelpers();
        TIASiml TIASiml = new TIASiml();

        public passwordLogin()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));

        }


        private void passwordContinueClick(object sender, RoutedEventArgs e)
        {

            TechBubbleIoTHelpers.iniateLoader(LoadingBar);
            TechBubbleIoTHelpers.collapseButton(loginContinue);

            if (password.Text=="")
            {
                TechBubbleIoTHelpers.collapseLoader(LoadingBar);
                TechBubbleSpeech.Speak(TIASiml.simlCommunicate("AUTHWAYNFCRESPONSE"));
                TechBubbleIoTHelpers.initiateButton(loginContinue);
            }
            else
            {

                TechBubbleIoTHelpers.iniateLoader(LoadingBar);
                TechBubbleIoTHelpers.collapseButton(loginContinue);

                    if (password.Text == TechBubbleIoTConfigs.userPassword)
                    {

                        TechBubbleIoTConfigs.userSession.status = "loggedIn";
                        TechBubbleSpeech.Speak("Thank you, redirecting you to application homepage.");
                        this.Frame.Navigate(typeof(appHome));
                    }
                    else
                    {

                        TechBubbleSpeech.Speak("Sorry password not valid");
                        this.Frame.Navigate(typeof(passwordLogin));
                    }
                }

            }
    }
}
