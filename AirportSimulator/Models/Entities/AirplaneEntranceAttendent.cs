using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Models.Entities
{
    internal class AirplaneEntranceAttendent : PermanentEntity
    {
        public AirplaneEntranceAttendent(string lable, string description) : base(lable, description)
        {
            Lable = lable;
            Description = description;
        }
    }
}
