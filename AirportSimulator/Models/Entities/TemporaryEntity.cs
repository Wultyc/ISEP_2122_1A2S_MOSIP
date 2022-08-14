using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Models.Entities
{
    internal class TemporaryEntity
    {
        public string Lable { get; init; }
        public bool IsDoingOnSiteCheckIn { get; init; }
        public bool HasToValidatePassport { get; init; }




        public TemporaryEntity(string lable, bool isDoingOnSiteCheckIn, bool hasToValidatePassport)
        {
            Lable = lable;
            IsDoingOnSiteCheckIn = isDoingOnSiteCheckIn;
            HasToValidatePassport = hasToValidatePassport;
        }
    }
}
