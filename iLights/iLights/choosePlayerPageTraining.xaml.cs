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
    public sealed partial class choosePlayerPageTraining : Page
    {
        public choosePlayerPageTraining()
        {
            this.InitializeComponent();
        }

        public Training training { get; set; }

        public user coach { get; set; }
        public List<Player> players { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;

            training = coach.currentTraining;

            players = coach.players;

        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            training.currentPlayer = (Player)e.ClickedItem;
            coach.currentTraining = training;
            Frame.Navigate(typeof(FocusTrainingxaml), coach);
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FocusTrainingxaml), coach);
        }
    }
}
