using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class ArticulosTarifas
    {
        [Key]
        [Display(Name = "Articulo Tarifa #")]
        public int tpat_pk { get; set; }

        [Display(Name = "Articulo #")]
        [Required(ErrorMessage = "Seleccione Articulo")]
        public int tpat_tpa_fk { get; set; }

        [Display(Name = "Rango #")]
        [Required(ErrorMessage = "Seleccione Rango")]
        public string tpat_tpg_fk { get; set; }

        [Display(Name = "Tarifa")]
        [Required(ErrorMessage = "Ingrese Tarifa")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double tpat_tarifa { get; set; }

        [Display(Name = "Estado")]
        public string tpat_tps_fk { get; set; }

        [Display(Name = "Creacion Usuario")]
        public string tpat_insert_user { get; set; }

        [Display(Name = "Creacion Fecha")]
        public string tpat_insert_date { get; set; }

        [Display(Name = "Modifica Usuario")]
        public string tpat_update_user { get; set; }

        [Display(Name = "Modifica Fecha")]
        public string tpat_update_date { get; set; }

        //[Display(Name = "Busqueda:")]
        //public string TextoBuscar { get; set; }


        /*
        [Required(ErrorMessage = "Ingrese Tarifa")]
        public double Tarifa { get; set; }
        */

        [Display(Name = "Rango")]
        public string tpg_valor { get; set; }

        [Display(Name = "Clase")]
        public string tpg_tipo { get; set; }

        [Display(Name = "Estado")]
        public string tps_nombre { get; set; }

        public IEnumerable<ArticulosTarifas> ArticulosRangosTarifasList { get; set; }


        public IEnumerable<ArticulosTarifas> ArticulosTarifasList { get; set; }
        public IEnumerable<Rangos> RangosList { get; set; }

        //public IEnumerable<ArticulosRangos> ArticulosRangosList { get; set; }

        public Articulos Articulos { get; set; }

        public Rutas Rutas { get; set; }
        //public IEnumerable<Listas> ListasList { get; set; }
        //public List<Opciones> ServiciosOpciones { get; set; }
        //public List<Opciones> RubrosOpciones { get; set; }
        public List<Opciones> EstadosOpciones { get; set; }

        //public IEnumerable<Opciones> ArticulosRangosTarifasOptions { get; set; }
        

    }
}