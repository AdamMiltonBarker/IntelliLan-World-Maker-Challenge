using System;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace TIA.Classes
{
    class TechBubbleSpeech
    {
        MediaElement mediaElement = new MediaElement();

        public async void Speak(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            SpeechSynthesizer synth = new SpeechSynthesizer();
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
            mediaElement.Stop();
            synth.Dispose();
        }
    }
}
