using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLights
{
    public class Training : TableEntity
    {
        public Training(string name, string description, string time,string coach)
        {
            this.PartitionKey = coach;
            this.RowKey = name;
            this.Name = name;
            this.Description = description;
             this.Time = time;
            currentPlayer = null;
        }

        public Training() { }
        public string Name { get; set; }
        public string Description { get; set; }


        public Player currentPlayer { get; set; }

        public string Time { get; set; }


    }
}
