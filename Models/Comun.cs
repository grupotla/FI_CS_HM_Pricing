using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class Comun
    {
    }


    public class Opciones
    {
        public string id { get; set; }
        public string nombre { get; set; }

        public string codigo { get; set; }

        public string FullOption
        {
            get
            {
                return nombre == "Select" || nombre == "No Data" ? nombre : (nombre.ToUpper() + (String.IsNullOrEmpty(codigo) ? "" : " (" + codigo + ")") + " (" + id + ")");
            }
        }

    }

    public class Tarifas_
    {
        public string rango_id { get; set; }
        public string tarifa_input { get; set; }
        public string tarifa_saved { get; set; }
    }


}