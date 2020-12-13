using System;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using Xamarin.Forms;
using Samsung.Sap;
using System.Linq;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace HelloMessageC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private Agent agent;
        private Peer peer;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnTopButtonPress(object sender, EventArgs e)
        {

            Fetch();
        }



        private void OnBottomButtonPress(object sender, EventArgs e)
        {
            //Connect();
            if (peer == null) {
                FindPeer();
            } else {
                StartRecognize();
            }
        }

        private async void StartRecognize()
        {
            await peer.SendMessage(Encoding.UTF8.GetBytes("startRecognize"));
        }


        private async void Fetch()
        {
            if (peer != null)
            {
                await peer.SendMessage(Encoding.UTF8.GetBytes("Hello Message"));
            } else {
                textLabel.Text = "Has no connection to phone. Please click <Connect>";
            }
        }

        private async void FindPeer()
        {
            agent = await Agent.GetAgent("/tohaman/myWearApp", onMessage: OnMessage);
            var peers = await agent.FindPeers();
            if (peers.Count() > 0)
            {
                peer = peers.First();
                //ShowMessage("Peer found, now you can send data.");
                textLabel.Text = "Peer found, now you can send data.";
                bottomButton.Text = "Recognize";
            }
        }

        private void OnMessage(Peer peer, byte[] content)
        {
            //ShowMessage("Received data: " + Encoding.UTF8.GetString(content));
            textLabel.Text = Encoding.UTF8.GetString(content);
        }

        private void ShowMessage(string message, string debugLog = null)
        {
            Toast.DisplayText(message, 3000);
            if (debugLog != null)
            {
                debugLog = message;
            }
            Debug.WriteLine("[DEBUG] " + message);
        }
    }
}