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
    public sealed partial class FocusTrainingxaml : Page
    {
        public FocusTrainingxaml()
        {
            this.InitializeComponent();
        }

        public user coach { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
            TrainingName.Text = coach.currentTraining.Name;
            descriptionList.Text = coach.currentTraining.Description;
            timer.Text = coach.currentTraining.Time + "s";


        }

        private void Go_Back_fun(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(trainingPage), coach);
        }

        private void ONCHOOSEPLAYER(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(choosePlayerPageTraining), coach);
        }

        private void startTraining(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(inTraining), coach);
        }

        private void getTopScores(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(topScoresPage), coach);
        }

    }
}
