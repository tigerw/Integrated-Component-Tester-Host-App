using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Integrated_Component_Tester
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        RemoteArduino Ra = new RemoteArduino();

        public MainPage()
        {
            var formattableTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Black;
            formattableTitleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            formattableTitleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.White;
            formattableTitleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Black;
            formattableTitleBar.ForegroundColor = Windows.UI.Colors.White;

            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            this.InitializeComponent();

            Ra.Initialise();
            Ra.JSONMessageReceived += new RemoteArduino.JSONMessageReceivedCallback(Message);
        }

        private static readonly Dictionary<string, string> BMPMappings = new Dictionary<string, string>
        {
            { "NPN Transistor", "BJTNPN" },
            { "Diode", "DIODE" },
            { "Capacitor", "POLARISED_CAPACITOR" },
            { "Resistor", "RESISTOR" },
            { "N/A", "NONE" }
        };

        private async void Message(Windows.Data.Json.JsonObject JSON)
        {
            await Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    var Component = JSON["Component"].GetString();
                    ComponentImage.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx://Integrated_Component_Tester/Assets/" + BMPMappings[Component] + ".bmp"));

                    ComponentLabel.Text = Component;
                    JSON.Remove("Component");

                    ComponentCharacteristicsList.Items.Clear();
                    foreach (var Entry in JSON)
                    {
                        ComponentCharacteristicsList.Items.Add(Entry.Key + ": " + Entry.Value);
                    }
                }
            );
        }
    }
}
