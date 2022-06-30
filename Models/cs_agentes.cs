using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class cs_agentes
    {
        public string agente_id { get; set; }
        public string agente { get; set; }
        public string activo { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string contacto { get; set; }
        public string nit { get; set; }
        public string countries { get; set; }
        public string tiporegimen { get; set; }
        public string id_usuario_creacion { get; set; }
        public string fecha_creacion { get; set; }
        public string hora_creacion { get; set; }
        public string id_grupo { get; set; }
        public string id_usuario_modificacion { get; set; }
        public string fecha_modificacion { get; set; }

        public string select { get; set; }

    }
}