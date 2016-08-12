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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace iLights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        async private void login_click(object sender, RoutedEventArgs e)
        {
            if (passBox.Text == "" || nameBox.Text == "")
            {
                errorBox.Text = "Error! one of the fields is empty. try again!";
                return;
            }
            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);

            CloudTableClient tableClient = account.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("Coaches");





            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<user>(nameBox.Text, passBox.Text);


            // Execute the retrieve operation.
            TableResult result = await table.ExecuteAsync(retrieveOperation);






            if (result.Result == null)
            {
                errorBox.Text = "Error! There is no such user";
            }
            else
            {
                getTrainings((user)result.Result);
                getPlayers((user)result.Result);
                getTopScoresTraining((user)result.Result);
                Frame.Navigate(typeof(chooseProgram), result.Result);
            }
               
        }


        private void SighIn_click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(signUpPage));

        }

        async private void getTrainings(user coach)
        {

            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable trainingTable = tableClient.GetTableReference("Training");

            coach.trainings = new List<Training> { };

            TableQuery<Training> query =
                     new TableQuery<Training>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
             QueryComparisons.Equal, coach.Name));


            TableContinuationToken token = null;
            query.TakeCount = 50;
            do
            {
                TableQuerySegment<Training> segment = await trainingTable.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                foreach (Training entity in segment)
                {
                    coach.trainings.Add(entity);
                }
            }
            while (token != null);

        }

        public async void getPlayers(user coach)
        {
            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable trainingTable = tableClient.GetTableReference("Players");

            coach.players = new List<Player> { };

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

        async private void getTopScoresTraining(user coach)
        {
            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable trainingTable = tableClient.GetTableReference("TopScoresNew");
            coach.scores = new List<Score> { };


            TableQuery<Score> query =
                     new TableQuery<Score>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
             QueryComparisons.Equal, coach.Name));


            TableContinuationToken token = null;
            query.TakeCount = 50;
            do
            {
                TableQuerySegment<Score> segment = await trainingTable.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                foreach (Score entity in segment)
                {
                    coach.scores.Add(entity);
                }
            }
            while (token != null);

        }

        private void moveToQuickGamePage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(quickGamePage));
        }
    }
}
