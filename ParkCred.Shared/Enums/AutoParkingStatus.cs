using System;
using System.Collections.Generic;
using System.Text;

namespace ParkCred.Shared.Enums
{
    public enum AutoParkingStatus : int
    {
        TurnedOff = 0,
        
        Initialised = 1,

        Planned = 2,

        Activated = 3,

        Finished = 4,

        NeedInitialisation = 5,

        NeedInitialisationWithInsurance = 6
    }
}
