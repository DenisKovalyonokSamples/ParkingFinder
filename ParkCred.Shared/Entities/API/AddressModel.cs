using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ParkCred.Shared.Entities.API
{
    public partial class AddressModel
    {
        public string Address { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        public string Comment { get; set; }

        public string Kind { get; set; }
    }

    public partial class AddressSuggestModel
    {
        public string Address { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        public string Description { get; set; }

        public string Kind { get; set; }
    }
}
