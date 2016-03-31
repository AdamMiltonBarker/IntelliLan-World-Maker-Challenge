using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TIA.Classes;
using TIA.Pages;

namespace TIA
{
    /// <summary>
    /// Hackster/Microsoft/Arduino World Maker Challenge Entry By Adam Milton-Barker
    /// TIA, the TechBubble Intelligent Assistant for the IntelliLan IoT Network
    /// </summary>

    public sealed partial class MainPage : Page
    {

        TechBubbleIoTConfigs TechBubbleIoTConfigs = new TechBubbleIoTConfigs();
        TechBubbleSpeech TechBubbleSpeech = new TechBubbleSpeech();
        TIASiml TIASiml = new TIASiml();

        public MainPage()
        {
            this.InitializeComponent();
            TIASiml.initiateSIML();
            TechBubbleSpeech.Speak(TIASiml.simlCommunicate("APPLICATIONWELCOME"));

        }

        private void nfcAuthRedirect(object sender, RoutedEventArgs e)
        {
            TechBubbleSpeech.Speak(TIASiml.simlCommunicate("AUTHWAYNFCRESPONSE"));
            this.Frame.Navigate(typeof(nfcLogin));

        }

        private void fingerprintAutRedirect(object sender, RoutedEventArgs e)
        {
            TechBubbleSpeech.Speak(TIASiml.simlCommunicate("AUTHWAYFPRESPONSE"));
            this.Frame.Navigate(typeof(fingerprintLogin));

        }

        private void passwordAutRedirect(object sender, RoutedEventArgs e)
        {
            TechBubbleSpeech.Speak(TIASiml.simlCommunicate("PASSWORDRESPONSE"));
            this.Frame.Navigate(typeof(passwordLogin));

        }
    }

}
