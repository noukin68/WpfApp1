using SocketIOClient;
using SocketIOClient.Transport;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Markup;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private SocketIO socket;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer(true);
            InitializeSocket();
        }

        private async void ConnectToServer(bool connect)
        {
            using (HttpClient client = new HttpClient())
            {
                string action = connect ? "connect" : "disconnect";
                Uri url = new Uri($"http://62.217.182.138:3000?action={action}");
                await client.GetAsync(url);

                if (connect)
                {
                    Console.WriteLine("Connected from WPF application");
                }
                else
                {
                    Console.WriteLine("Disconnected from WPF application");
                }
            }
        }

        private void InitializeSocket()
        {
            socket = new SocketIO("http://62.217.182.138:3000");

            socket.On("text-received", (text) => { 
                Dispatcher.Invoke(() => {
                    UpdateTextBlock(text.ToString());
                });
            });
            socket.ConnectAsync();
        }

        private void UpdateTextBlock(string text)
        {
            textBlock.Text = text;
        }

        protected override void OnClosed(EventArgs e)
        {
            ConnectToServer(false);
            base.OnClosed(e);
        }

    }
}


