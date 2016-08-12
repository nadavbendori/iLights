using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLights
{
    public class Player : TableEntity
    {
        public Player(string name, string coach)
        {
            this.PartitionKey = coach;
            this.RowKey = name;
            this.Name = name;
        }

        public Player() { }

        public string Name { get; set; }


    }
}
