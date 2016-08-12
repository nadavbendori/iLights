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
    public sealed partial class topScoresPage : Page
    {
        public topScoresPage()
        {
            this.InitializeComponent();
        }

        public user coach { get; set; }
        public Training training { get; set; }

        public List<Score> scores { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;
            training = coach.currentTraining;

            scores = new List<Score> { };
            updateScores();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(AddPlayerPage), coach);

        }


        private void updateScores()
        {
            foreach (Score entity in coach.scores)
            {
                if (entity.trainingName == coach.currentTraining.Name)
                {
                    scores.Add(entity);
                }
            }

            scores.Sort(delegate (Score p1, Score p2)
            {
                int compareDate = p1.trainingScore.CompareTo(p2.trainingScore);
                if (compareDate == 0)
                {
                    return p2.Timestamp.CompareTo(p1.Timestamp);
                }
                return -compareDate;
            });

        }


        private void onGoBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FocusTrainingxaml), coach);
        }
    }
}
