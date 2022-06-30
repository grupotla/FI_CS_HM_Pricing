using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ti_pricing.Models
{
    public class Articulos
    {
        [Key]
        [Display(Name = "Articulo #")]
        public int tpa_pk { get; set; }

        [Required(ErrorMessage = "Seleccione Ruta")]
        [Display(Name = "Ruta #")]
        public int tpa_tpr_fk { get; set; }

        [Required(ErrorMessage = "Seleccione Servicio")]
        [Display(Name = "Servicio")]
        public string tpa_servicio_fk { get; set; }

        [Required(ErrorMessage = "Seleccione Rubro")]
        [Display(Name = "Rubro")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione Rubro")]
        public string tpa_rubro_fk { get; set; }

        [Required(ErrorMessage = "Seleccione Medida")]
        [Display(Name = "Medida")]
        public string tpa_medida_fk { get; set; }



        [Display(Name = "Estado")]
        public string tpa_tps_fk { get; set; }
        [Display(Name = "Creacion Usuario")]
        public string tpa_insert_user { get; set; }
        [Display(Name = "Creacion Fecha")]
        public string tpa_insert_date { get; set; }
        [Display(Name = "Modifica Usuario")]
        public string tpa_update_user { get; set; }
        [Display(Name = "Modifica Fecha")]
        public string tpa_update_date { get; set; }



        [Display(Name = "Estado")]
        public string tps_nombre { get; set; }

        [Display(Name = "Servicio")]
        public string tpa_servicio { get; set; }

        [Display(Name = "Rubro")]
        public string tpa_rubro { get; set; }

        [Display(Name = "Medida")]
        public string tpa_medida { get; set; }

        

            
            


        public IEnumerable<Articulos> ArticulosList { get; set; }
        public IEnumerable<ArticulosRangos> ArticulosRangosList { get; set; }
        public IEnumerable<ArticulosTarifas> ArticulosTarifasList { get; set; }
        public List<Opciones> ServiciosOpciones { get; set; }
        public List<Opciones> RubrosOpciones { get; set; }
        //public IEnumerable<Articulos> ArticulosList2 { get; set; }
        //public List<Opciones> ServiciosOpciones2 { get; set; }

        public IEnumerable<ArticulosTarifas> ArticulosRangosTarifasList { get; set; }


        public Rutas Rutas { get; set; }
        
        public Listas Listas { get; set; }

        //public List<Opciones> PaisesOpciones { get; set; }
        //public List<Opciones> TipoOpciones { get; set; }
        //public List<Opciones> TransporteOpciones { get; set; }
        //public List<Opciones> MedidaOpciones { get; set; }
        //public List<Opciones> MonedaOpciones { get; set; }
        //public List<Opciones> MovimientoOpciones { get; set; }
        //public IEnumerable<Puertos> OrigenOpciones { get; set; }
        //public IEnumerable<Puertos> DestinoOpciones { get; set; }
        public List<Opciones> EstadosOpciones { get; set; }
        //public string TextoBuscar { get; set; }

        public List<Opciones> MedidaOpciones { get; set; }
        public IEnumerable<Rangos> RangosList { get; set; }




    }
}