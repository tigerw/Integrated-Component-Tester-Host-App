using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;


namespace Integrated_Component_Tester
{
    class RemoteArduino
    {
        public async void Initialise()
        {
            Serial = new UsbSerial((await UsbSerial.listAvailableDevicesAsync())[0]);
            Arduino = new RemoteDevice(Serial);

            Arduino.StringMessageReceived += new StringMessageReceivedCallback(MessageReceived);
            Serial.begin(9600, SerialConfig.SERIAL_8N1);
        }

        public void MessageReceived(string Message)
        {
            var Root = Windows.Data.Json.JsonValue.Parse(Message).GetObject();
            JSONMessageReceived(Root);
        }

        public delegate void JSONMessageReceivedCallback(Windows.Data.Json.JsonObject JSON);
        public event JSONMessageReceivedCallback JSONMessageReceived;

        private IStream Serial;
        private RemoteDevice Arduino;
    }
}
