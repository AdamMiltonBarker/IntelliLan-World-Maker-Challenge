using System;
using System.Text;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TIA.Classes
{
    class TechBubbleIoTHelpers
    {
        public void iniateLoader(ProgressBar LoadingBar)
        {
            LoadingBar.IsEnabled = true;
            LoadingBar.Visibility = Visibility.Visible;
        }

        public void collapseLoader(ProgressBar LoadingBar)
        {
            LoadingBar.IsEnabled = false;
            LoadingBar.Visibility = Visibility.Collapsed;
        }

        public void initiateButton(Button item)
        {
            item.Visibility = Visibility.Visible;
        }

        public void collapseButton(Button item)
        {
            item.Visibility = Visibility.Collapsed;
        }

        public void initiateTextBox(TextBox item)
        {
            item.Visibility = Visibility.Visible;
        }

        public void collapseTextBox(TextBox item)
        {
            item.Visibility = Visibility.Collapsed;
        }
    }
}
