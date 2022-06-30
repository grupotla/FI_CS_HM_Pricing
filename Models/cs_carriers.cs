using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class cs_carriers
    {
        public string carrier_id { get; set; }
        public string name { get; set; }
        public string countries { get; set; }
        public string carriercode { get; set; }
        public string tiporegimen { get; set; }
        public string nit { get; set; }
        public string activo { get; set; }
        public string user_insert { get; set; }
        public string date_insert { get; set; }
        public string user_update { get; set; }
        public string date_update { get; set; }

        public string select { get; set; }

    }
}