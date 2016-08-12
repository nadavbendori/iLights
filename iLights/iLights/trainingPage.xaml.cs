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
    public sealed partial class trainingPage : Page
    {
        public user coach { get; set; }
        public List<Training> trainings { get; set; }
        public trainingPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
            //getTopScoresTraining(coach);
            trainings = coach.trainings;

        }



        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //in here when i click on something on the list of training i get the object
            coach.currentTraining = (Training)e.ClickedItem;

            Frame.Navigate(typeof(FocusTrainingxaml), coach);

        }

        private void Go_back(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(chooseProgram), coach);
        }


        private void addTraining(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(addTrainingPage), coach);
        }
    }
}
