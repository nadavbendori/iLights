using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iLights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class inTraining : Page
    {

        public Training training { get; set; }
        public int counter { get; set; }


        public user coach { get; set; }

        StreamSocket socket;

        private DispatcherTimer timer;
        public inTraining()
        {
            this.InitializeComponent();
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
            training = coach.currentTraining;
            //addScore(20);
            //All this part is to start training
            int j;
            if (Int32.TryParse(training.Time, out j))
                counter++;
            else
            {
                counter--;
                return;
            }
            counter = j;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            this.sendStart();
            timer.Start();
        }


        async private void addScore(int n)
        {
            string player;
            if (training.currentPlayer == null)
            {
                player = "unknown";
            }
            else
            {
                player = training.currentPlayer.Name;
            }

            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable trainingTable = tableClient.GetTableReference("TopScoresNew");

            //.Add(new Training(nameBox.Text, descriptionBox.Text, j));
            Score newScore = new Score(training.Name, n, coach.Name, player, coach.scores.Count);





            TableOperation insertOperation = TableOperation.Insert(newScore);
            TableResult result = await trainingTable.ExecuteAsync(insertOperation);
            coach.scores.Add((Score)result.Result);


        }

        private void timer_Tick(object sender, Object e)
        {
            lblTime.Text = Convert.ToString(counter);
            counter--;

            if (counter == -1)
            {
                timer.Stop();
                this.sendOver2();
            }
        }


        private async void sendStart()
        {
            await connectStart("192.168.43.126", "80", "is");
        }

        public async Task connectStart(string host, string port, string message)
        {
            HostName hostName;

            using (socket = new StreamSocket())
            {
                hostName = new HostName(host);

                // Set NoDelay to false so that the Nagle algorithm is not disabled
                socket.Control.NoDelay = false;

                try
                {
                    // Connect to the server
                    await socket.ConnectAsync(hostName, port);
                    // Send the message
                    await this.send("start");
                    //// Read response
                }
                catch (Exception exception)
                {
                    switch (SocketError.GetStatus(exception.HResult))
                    {
                        case SocketErrorStatus.HostNotFound:
                            // Handle HostNotFound Error
                            throw;
                        default:
                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
                            throw;
                    }
                }
            }
        }
        public async Task connectOver(string host, string port, string message)
        {
            HostName hostName;

            using (socket = new StreamSocket())
            {
                hostName = new HostName(host);

                // Set NoDelay to false so that the Nagle algorithm is not disabled
                socket.Control.NoDelay = false;

                try
                {
                    // Connect to the server
                    await socket.ConnectAsync(hostName, port);
                    // Send the message
                    await this.send("over");
                    //// Read response
                    await this.read();
                }
                catch (Exception exception)
                {
                    switch (SocketError.GetStatus(exception.HResult))
                    {
                        case SocketErrorStatus.HostNotFound:
                            // Handle HostNotFound Error
                            throw;
                        default:
                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
                            throw;
                    }
                }
            }
        }

        public async Task send(string message)
        {
            DataWriter writer;

            // Create the data writer object backed by the in-memory stream. 
            using (writer = new DataWriter(socket.OutputStream))
            {
                // Set the Unicode character encoding for the output stream
                writer.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                // Specify the byte order of a stream.
                writer.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

                // Gets the size of UTF-8 string.
                writer.MeasureString(message);
                // Write a string value to the output stream.
                writer.WriteString(message);

                // Send the contents of the writer to the backing stream.
                try
                {
                    await writer.StoreAsync();
                }
                catch (Exception exception)
                {
                    switch (SocketError.GetStatus(exception.HResult))
                    {
                        case SocketErrorStatus.HostNotFound:
                            // Handle HostNotFound Error
                            throw;
                        default:
                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
                            throw;
                    }
                }

                await writer.FlushAsync();
                // In order to prolong the lifetime of the stream, detach it from the DataWriter
                writer.DetachStream();
            }
        }

        public async Task<String> read()
        {
            DataReader reader;
            StringBuilder strBuilder;

            using (reader = new DataReader(socket.InputStream))
            {
                strBuilder = new StringBuilder();

                // Set the DataReader to only wait for available data (so that we don't have to know the data size)
                reader.InputStreamOptions = Windows.Storage.Streams.InputStreamOptions.Partial;
                // The encoding and byte order need to match the settings of the writer we previously used.
                reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                reader.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

                // Send the contents of the writer to the backing stream. 
                // Get the size of the buffer that has not been read.
                await reader.LoadAsync(256);

                // Keep reading until we consume the complete stream.
                while (reader.UnconsumedBufferLength > 0)
                {


                    this.textBlock.Text = reader.ReadString(reader.UnconsumedBufferLength);
                    strBuilder.Append(reader.ReadString(reader.UnconsumedBufferLength));
                    await reader.LoadAsync(256);
                }

                int j;
                if (Int32.TryParse(this.textBlock.Text, out j))
                    j++;
                else
                {
                    //errorBox.Text = "time is Not a number!";
                    //return;
                }

                this.textBlock.Text = Convert.ToString(j);
                this.textBlock2.Text = "Score:";
                this.addScore(j);

                reader.DetachStream();
                return strBuilder.ToString();
            }
        }

        private async void sendOver2()
        {
            await connectOver("192.168.43.126", "80", "is");
        }

        private void onGoBack(object sender, RoutedEventArgs e)
        {
            training.currentPlayer.Name = "unknown";
            Frame.Navigate(typeof(FocusTrainingxaml), coach);
        }

        private void stopGame(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.sendOver2();
        }
    }
}
