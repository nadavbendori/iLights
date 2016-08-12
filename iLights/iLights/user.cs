using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLights
{
    public class user : TableEntity
    {
        public user(string name, string password)
        {
            this.PartitionKey = name;
            this.RowKey = password;
            this.Name = name;
            this.Password = password;
            this.trainings = new List<Training> { };
            this.players = new List<Player> { };
            this.scores = new List<Score> { };

        }

        public user() { }

        public string Name { get; set; }
        public string Password { get; set; }


        public List<Training> trainings { get; set; }


        public Training currentTraining { get; set; }

        public List<Player> players { get; set; }

        public Player currentPlayer { get; set; }


        public List<Score> scores { get; set; }


    }
}
