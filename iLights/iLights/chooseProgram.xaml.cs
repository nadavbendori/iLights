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

using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Microsoft.WindowsAzure.Storage.Auth;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iLights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class chooseProgram : Page
    {
        StreamSocket socket;
        public chooseProgram()
        {
            this.InitializeComponent();

        }

        public user coach { get; set; }

        private void addTableFunc(object sender, RoutedEventArgs e)
        {
            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);

            CloudTableClient tableClient = account.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("people");

            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync();

            // Create a new customer entity.
            user customer1 = new user("idan", "1233334");
            customer1.Name = "idanchu";
            customer1.Password = "05050";
            user customer2 = new user("rami", "1234");

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(customer1);

            // Execute the insert operation.
            table.ExecuteAsync(insertOperation);
            textBlock.Text = "stam";
        }


        async private void showUserFunc(object sender, RoutedEventArgs e)
        {
            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);

            CloudTableClient tableClient = account.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("people");


            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<user>("idan", "1233334");


            // Execute the retrieve operation.
            TableResult x = await table.ExecuteAsync(retrieveOperation);

            if (x.Result != null)
                textBlock.Text = ((user)x.Result).Password;

        }

        //this way we can get information from previous page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
        }


        private void BackToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        async private void connectFunc(object sender, RoutedEventArgs e)
        {
            await connect("192.168.43.126","80","is");

        }


        public async Task connect(string host, string port, string message)
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
                    await Task.Delay(TimeSpan.FromSeconds(60));
                    await socket.ConnectAsync(hostName, port);
                    await this.send("over");
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


        public async void getPlayers()
        {
            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable trainingTable = tableClient.GetTableReference("Players");

            coach.trainings = new List<Training> { };

            TableQuery<Player> query =
                     new TableQuery<Player>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
             QueryComparisons.Equal, coach.Name));


            TableContinuationToken token = null;
            query.TakeCount = 50;
            do
            {
                TableQuerySegment<Player> segment = await trainingTable.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                foreach (Player entity in segment)
                {
                    coach.players.Add(entity);
                }
            }
            while (token != null);
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
                    this.testTextBlock.Text = reader.ReadString(reader.UnconsumedBufferLength);
                    strBuilder.Append(reader.ReadString(reader.UnconsumedBufferLength));
                    await reader.LoadAsync(256);
                }

                reader.DetachStream();
                return strBuilder.ToString();
            }
        }

        private void moveToTrainings(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(trainingPage), coach);
        }

        private void goToPlayers(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(playersPage), coach);
        }
    }
}
