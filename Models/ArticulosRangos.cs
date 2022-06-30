using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class ArticulosRangos
    {
        [Key]
        [Display(Name = "Articulo Rango #")]
        public int tpag_pk { get; set; }

        [Required(ErrorMessage = "Seleccione Articulo")]
        [Display(Name = "Articulo #")]
        public int tpag_tpa_fk { get; set; }

        [Required(ErrorMessage = "Seleccione Rango")]
        [Display(Name = "Rango #")]
        public string tpag_tpg_fk { get; set; }


        [Display(Name = "Estado")]
        public string tpag_tps_fk { get; set; }
        [Display(Name = "Creacion Usuario")]
        public string tpag_insert_user { get; set; }
        [Display(Name = "Creacion Fecha")]
        public string tpag_insert_date { get; set; }
        [Display(Name = "Modifica Usuario")]
        public string tpag_update_user { get; set; }
        [Display(Name = "Modifica Fecha")]
        public string tpag_update_date { get; set; }

        //public string TextoBuscar { get; set; }

        [Display(Name = "Rango")]
        public string tpg_valor { get; set; }

        [Display(Name = "Clase")]
        public string tpg_tipo { get; set; }

        [Display(Name = "Estado")]
        public string tps_nombre { get; set; }

        public IEnumerable<ArticulosRangos> ArticulosRangosList { get; set; }
        public IEnumerable<Rangos> RangosList { get; set; }

        public IEnumerable<ArticulosTarifas> ArticulosTarifasList { get; set; }

        public Articulos Articulos { get; set; }
        public Rutas Rutas { get; set; }
        //public IEnumerable<Listas> ListasList { get; set; }
        //public List<Opciones> ServiciosOpciones { get; set; }
        //public List<Opciones> RubrosOpciones { get; set; }
        public List<Opciones> EstadosOpciones { get; set; }

    }
}