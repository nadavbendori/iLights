using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iLights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class playersPage : Page
    {
        public playersPage()
        {
            this.InitializeComponent();
        }

        public user coach { get; set; }
        public List<Player> players { get; set; }

        public List<Score> scores { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
            players = coach.players;
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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            coach.currentPlayer = (Player)e.ClickedItem;


            Frame.Navigate(typeof(topScoresPageOfPlayer), coach);

        }



        private void moveToAddPlayer(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddPlayerPage), coach);
        }

        private void onGoBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(chooseProgram), coach);
        }
    }
}
