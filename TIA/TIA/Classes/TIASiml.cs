using Syn.Bot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TIA.Classes
{
    class TIASiml
    {
        public SynBot TIASimlBot;

        public void initiateSIML()
        {
            TIASimlBot = new SynBot();
            TIASimlBot.PackageManager.LoadFromString(File.ReadAllText("TIA.simlpk"));
        }
        public string simlCommunicate(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            var result = TIASimlBot.Chat(text);
            return result.BotMessage;
        }
    }
}
