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
    public sealed partial class topScoresPageOfPlayer : Page
    {
        public topScoresPageOfPlayer()
        {
            this.InitializeComponent();
        }


        public user coach { get; set; }

        public List<Score> scores { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coach = (user)e.Parameter;

            scores = new List<Score> { };
            updateScores();
        }


        private void updateScores()
        {
            foreach (Score entity in coach.scores)
            {
                if (entity.playerName == coach.currentPlayer.Name)
                {
                    scores.Add(entity);
                }
            }

            scores.Sort(delegate (Score p1, Score p2)
            {
                int compareDate = p1.trainingName.CompareTo(p2.trainingName);
                if (compareDate == 0)
                {
                    return p2.Timestamp.CompareTo(p1.Timestamp);
                }
                return compareDate;
            });

        }






        private void onGoBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(playersPage), coach);
        }
    }
}

