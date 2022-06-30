using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class cs_clientes
    {
        public string id_cliente { get; set; }
        public string codigo_tributario { get; set; }
        public string email { get; set; }
        public string nombre_cliente { get; set; }
        public string nombre_facturar { get; set; }
        public string id_vendedor { get; set; }
        public string id_tipo_cliente { get; set; }
        public string id_grupo { get; set; }
        public string id_cobrador { get; set; }
        public string id_estatus { get; set; }
        public string es_coloader { get; set; }
        public string es_consigneer { get; set; }
        public string es_shipper { get; set; }
        public string id_pais { get; set; }
        public string id_regimen { get; set; }
        public string observacion { get; set; }
        public string id_usuario_creacion { get; set; }
        public string fecha_creacion { get; set; }
        public string hora_creacion { get; set; }
        public string id_usuario_modificacion { get; set; }
        public string fecha_modificacion { get; set; }

        public string select { get; set; }


    }
}