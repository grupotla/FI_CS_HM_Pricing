using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class cs_navieras
    {
        public string id_naviera { get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
        public string tiporegimen { get; set; }
        public string nit { get; set; }
        public string id_pais { get; set; }
        public string id_naviera_group { get; set; }
        public string user_insert { get; set; }
        public string date_insert { get; set; }
        public string user_update { get; set; }
        public string date_update { get; set; }
        public string id_naviera_representante { get; set; }
     
        public string select { get; set; }
    }
}