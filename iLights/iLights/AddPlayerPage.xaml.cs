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
    public sealed partial class AddPlayerPage : Page
    {

        public user coach { get; set; }
        public AddPlayerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
        }

        async private void submitPlayer(object sender, RoutedEventArgs e)
        {
            if ( nameBox.Text == "")
            {
                errorBox.Text = "one of the fields is empty";
                return;
            }

            var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
            var account = new CloudStorageAccount(credentials, true);

            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable trainingTable = tableClient.GetTableReference("Players");

            //.Add(new Training(nameBox.Text, descriptionBox.Text, j));
            Player newPlayer = new Player(nameBox.Text, coach.Name);

            TableOperation insertOperation = TableOperation.Insert(newPlayer);
            TableResult result =  await trainingTable.ExecuteAsync(insertOperation);
            coach.players.Add((Player)result.Result);

            Frame.Navigate(typeof(playersPage), coach);

        }

        private void onGoBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(playersPage), coach);
        }
    }
}
