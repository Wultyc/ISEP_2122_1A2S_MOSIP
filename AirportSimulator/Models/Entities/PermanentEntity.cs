using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Models.Entities
{
    internal class PermanentEntity : IEntity
    {
        public string Lable { get; init; }
        public string Description { get; init; }
        public bool IsBusy { get; set; } = false;

        public PermanentEntity(string lable, string description)
        {
            Lable = lable;
            Description = description;
        }
    }
}
