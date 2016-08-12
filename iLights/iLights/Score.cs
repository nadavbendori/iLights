using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLights
{
    public class Score : TableEntity
    {

        public Score(string trainingName, int newScore, string coachName, string playerName,int ID)
        {
            this.trainingName = trainingName;
            this.PartitionKey = coachName;
            this.RowKey = ID.ToString();
            this.playerName = playerName;

            this.trainingScore = newScore;
            //this.trainings.Add(new Training("idan", "4 4 2"));
        }
        public Score()
        {
        }
        public string trainingName { get; set; }
        public int trainingScore { get; set; }



        public string playerName { get; set; }

    }


}
