using ti_pricing.Models;
using ti_pricing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace ti_pricing.Controllers
{
    public class ListasController : Controller
    {

        DataListing objListing;
        LoginController valida;
        List<Opciones> LPaises;



        public ListasController()
        {
            objListing = new DataListing();
            valida = new LoginController();
            LPaises = new List<Opciones>();

        }


        // AJAX: Listas
        [HttpGet]
        public ActionResult IndexX(Listas Model, FormCollection collection, string TextoBuscar, string tpl_pais_fk, string tpl_transporte_fk, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            var Context = HttpContext.Request.RequestContext.RouteData.Values;

            ViewBag.controller = Context["Controller"].ToString();

            ViewBag.action = Context["Action"].ToString();

            ViewBag.ajaxLoader = "si";

            try
            {
                tpl_pais_fk = String.IsNullOrEmpty(tpl_pais_fk) ? "" : "'" + tpl_pais_fk + "'";
                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));
                tpl_transporte_fk = String.IsNullOrEmpty(tpl_transporte_fk) ? "" : "'" + tpl_transporte_fk + "'";
                Model.TransporteOpciones = objListing.GetTransportesOptions("");

                return View("_Index",Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + ViewBag.controller + "&a=" + ViewBag.action + "&m=" + e.Message);  
            }

            return null;
            
        }




        // GET: Listas
        [HttpGet]
        public ActionResult Index(Listas Model, FormCollection collection, string TextoBuscar, string tpl_pais_fk, string tpl_transporte_fk, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            var Context = HttpContext.Request.RequestContext.RouteData.Values;

            ViewBag.controller = Context["Controller"].ToString();

            ViewBag.action = Context["Action"].ToString();

            ViewBag.ajaxLoader = "no";

            try
            {
                tpl_pais_fk = String.IsNullOrEmpty(tpl_pais_fk) ? "" : "'" + tpl_pais_fk + "'";

                tpl_transporte_fk = String.IsNullOrEmpty(tpl_transporte_fk) ? "" : "'" + tpl_transporte_fk + "'";

                Model.ListasList = objListing.AllListasListing(TextoBuscar, "", tpl_pais_fk, tpl_transporte_fk, "");

                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

                Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
                Model.TransporteOpciones = objListing.GetTransportesOptions("");
                Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
                Model.MonedaOpciones = objListing.GetMonedasOptions();

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }
            return null;
        }


        [HttpGet]
        public ActionResult Report(Listas Model, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            var Context = HttpContext.Request.RequestContext.RouteData.Values;

            ViewBag.controller = Context["Controller"].ToString();

            ViewBag.action = Context["Action"].ToString();

            try
            {

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + ViewBag.controller + "&a=" + ViewBag.action + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }

        public PartialViewResult Listas2(Listas Model, string accion)
        {

            var Context = HttpContext.Request.RequestContext.RouteData.Values;

            ViewBag.controller = Context["Controller"].ToString();

            ViewBag.action = Context["Action"].ToString();

            try
            {

                Model.ListasList = objListing.AllListasReportListing(Model);

                if (string.IsNullOrEmpty(accion)) accion = "Report";

                ViewBag.action = accion; 

                return PartialView("~/Views/Listas/_TableComun.cshtml", Model);
            }
            catch (Exception e)
            {
                ViewBag.c = Context["Controller"];
                ViewBag.a = Context["Action"];
                ViewBag.m = e.Message;
                ViewBag.s = "panel-danger";
                return PartialView("~/Views/Login/Error.cshtml");
            }


        }


        [HttpGet]
        public PartialViewResult Search2()
        {

            try
            {
                Listas Model = new Listas();

                Model.EmpresasOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

                Model.TipoOpciones = objListing.GetTipoListasOptions();
                Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
                Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
                Model.MonedaOpciones = objListing.GetMonedasOptions();
                Model.MovimientoOpciones = objListing.GetMovimientosOptions();

                Model.tpl_fecha_inicio = DateTime.Now.ToString("yyyy-MM-dd");
                Model.tpl_fecha_vencimiento = DateTime.Now.ToString("yyyy-MM-dd");

                var data = objListing.AllListasPaisesPuertosTransportesListing();


                if (LPaises.Count > 0)
                    Model.PaisesOpciones = LPaises;
                else
                    Model.PaisesOpciones = objListing.GetPaisesOptions("", "", data.id);

                Model.OrigenOpciones = objListing.AllPuertosListing("", "", data.codigo, "", "");

                IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };

                Model.OrigenOpciones = second.Concat(Model.OrigenOpciones);

                Model.DestinoOpciones = Model.OrigenOpciones;

                Model.TransporteOpciones = objListing.GetTransportesOptions(data.nombre);

                return PartialView("~/Views/Listas/_Form.cshtml", Model);
            }
            catch (Exception e)
            {
                ViewBag.c = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
                ViewBag.a = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();
                ViewBag.m = e.Message;
                ViewBag.s = "panel-danger";

                return PartialView("~/Views/Login/Error.cshtml");
            }

        }

        // GET: Listas/Details/5
        public ActionResult Rutas(string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/Rutas/Index?id_lista=" + id_lista);
            return null;

        }


        public PartialViewResult Rutas2(string id_lista, string accion)
        {
            Rutas Model = new Rutas();

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrEmpty(id_lista))
                {
                    ViewBag.id_lista = id_lista;
                    ViewBag.action = accion;
                    ViewBag.controller = "Listas";
                    ViewBag.ajaxLoader = "si";

                    Model.RutasList = objListing.AllRutasListing("", id_lista, "");

                }

                return PartialView("~/Views/Rutas/_TableComun.cshtml", Model);
            }
            else
            {
                ViewBag.c = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
                ViewBag.a = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();
                ViewBag.m = "No es llamada Ajax"; //e.Message;
                ViewBag.s = "panel-danger";

                return PartialView("~/Views/Login/Error.cshtml");
            }

        }

       

        // GET: Listas/Details/5
        public ActionResult Articulos2(string id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;


            try { 

                string controller = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();

                ViewBag.controller = controller;

                Articulos Model = new Articulos();

                ViewBag.id_ruta = id_ruta;

                Model.Rutas = objListing.AllRutasListing("", "", id_ruta).First();

                Model.ArticulosList = objListing.AllArticulosListing("", "", id_ruta, "");

                string articulos = "";
                foreach (Articulos item in Model.ArticulosList)
                {
                    if (articulos != "") articulos += ",";
                    articulos += item.tpa_pk;
                }

                Model.ArticulosRangosTarifasList = objListing.AllArticulosRangosTarifasListing(articulos);

                return PartialView("_Articulos", Model);

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                
                ViewBag.c = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
                ViewBag.a = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();
                ViewBag.m = e.Message;
                ViewBag.s = "panel-danger";
                
                return PartialView("~/Views/Login/Error.cshtml");
            }

        }





        // GET: Listas/Create
        public ActionResult Create()
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            Listas Model = new Listas();

            try
            {
                //Model.PaisesOpciones2 = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));
                Model.TipoOpciones = objListing.GetTipoListasOptions();
                Model.TransporteOpciones = objListing.GetTransportesOptions("");
                Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
                Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
                Model.MonedaOpciones = objListing.GetMonedasOptions();
                Model.MovimientoOpciones = objListing.GetMovimientosOptions();
                //Model.OrigenOpciones = objListing.AllPuertosListing("", "", "", "SI", "");

                //Model.OrigenOpciones = Model.OrigenOpciones.Prepend(new Puertos { tpp_pk = "", tpp_nombre = "Select" });

                //IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };
                //Model.OrigenOpciones = second.Concat(Model.OrigenOpciones);

                //Model.DestinoOpciones = Model.OrigenOpciones;

                //GetPaisesOptions(Model.DestinoOpciones);

                //Model.PaisesOpciones = LPaises;
                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

                Model.tpl_fecha_inicio = DateTime.Now.ToString("yyyy-MM-dd");
                Model.tpl_fecha_vencimiento = DateTime.Now.ToString("yyyy-MM-dd");

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                return View("Form", Model);

            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }

        // POST: Listas/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Listas Model)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            try
            {
                Model.tpl_tps_fk = "1";
                Model.tpl_insert_user = Session["UserID"].ToString();

                if (string.IsNullOrEmpty(Model.tpl_referencia)) Model.tpl_referencia = "";

                if (ModelState.IsValid)
                {
                    objListing.InsertListas(Model);

                    ViewBag.m = "Listas Created Successfully";

                    return RedirectToAction("Index", new { m = ViewBag.m});
                }
                else
                {
                    ModelState.AddModelError("Error", "something its wrong");

                }
            }
            catch(Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                return null;
            }


            //Model.PaisesOpciones2 = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));
            Model.TipoOpciones = objListing.GetTipoListasOptions();
            Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
            Model.TransporteOpciones = objListing.GetTransportesOptions("");
            Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
            Model.MonedaOpciones = objListing.GetMonedasOptions();
            Model.MovimientoOpciones = objListing.GetMovimientosOptions();
            //Model.OrigenOpciones = objListing.AllPuertosListing("", "", "", "SI", "");

            //Model.OrigenOpciones = Model.OrigenOpciones.Prepend(new Puertos { tpp_pk = "", tpp_nombre = "Select" });

            //IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };
            //Model.OrigenOpciones = second.Concat(Model.OrigenOpciones);

            //Model.DestinoOpciones = Model.OrigenOpciones;

            //GetPaisesOptions(Model.DestinoOpciones);

            //Model.PaisesOpciones = LPaises;
            Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }

        public ActionResult Edit(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";

            var Model = objListing.GetListasByID(Convert.ToString(id));

            try
            {
                ViewBag.id = Model.tpl_pk;

                Model.RutasList = objListing.AllRutasListing("", id.ToString(), "");

                ViewBag.Rutas = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? Model.RutasList.Count() : 0;
                
                //Model.PaisesOpciones2 = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

                Model.TipoOpciones = objListing.GetTipoListasOptions();
                Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
                Model.TransporteOpciones = objListing.GetTransportesOptions("");
                Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
                Model.MonedaOpciones = objListing.GetMonedasOptions();
                Model.MovimientoOpciones = objListing.GetMovimientosOptions();
                //Model.OrigenOpciones = objListing.AllPuertosListing("", "", "", "SI", "");


                //Model.OrigenOpcion = objListing.GetPuertosByID(Model.tpl_tpu_origen_fk.ToString());
                //Model.DestinoOpcion = objListing.GetPuertosByID(Model.tpl_tpu_destino_fk.ToString());

                //Model.tpl_tpu_pais_origen_fk = Model.OrigenOpcion.tpp_pais_iso_fk;
                //Model.tpl_tpu_pais_destino_fk = Model.DestinoOpcion.tpp_pais_iso_fk;

                //Model.DestinoOpciones = Model.OrigenOpciones;

                //GetPaisesOptions(Model.DestinoOpciones);

                //Model.PaisesOpciones = LPaises;
                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                return View("Form", Model);

            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;

        }

        // POST: Listas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Listas Model, FormCollection collection)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            try
            {
                ViewBag.actionType = "Update";

                Model.tpl_update_user = Session["UserID"].ToString();

                if (string.IsNullOrEmpty(Model.tpl_referencia)) Model.tpl_referencia = "";

                if (ModelState.IsValid)
                {
                    objListing.UpdateListas(Model);

                    ViewBag.m = "Listas Updated Successfully";

                    return RedirectToAction("Index", new { m = ViewBag.m});
                }
                else
                {
                    ModelState.AddModelError("Error", "something its wrong");

                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                return null;
            }

            //Model.PaisesOpciones2 = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

            Model.TipoOpciones = objListing.GetTipoListasOptions();
            Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
            Model.TransporteOpciones = objListing.GetTransportesOptions("");
            Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
            Model.MonedaOpciones = objListing.GetMonedasOptions();
            Model.MovimientoOpciones = objListing.GetMovimientosOptions();
            //Model.OrigenOpciones = objListing.AllPuertosListing("", "", "", "SI", "");

            //Model.OrigenOpcion = objListing.GetPuertosByID(Model.tpl_tpu_origen_fk.ToString());
            //Model.DestinoOpcion = objListing.GetPuertosByID(Model.tpl_tpu_destino_fk.ToString());

            //Model.tpl_tpu_pais_origen_fk = Model.OrigenOpcion.tpp_pais_iso_fk;
            //Model.tpl_tpu_pais_destino_fk = Model.DestinoOpcion.tpp_pais_iso_fk;

            //Model.DestinoOpciones = Model.OrigenOpciones;

            //GetPaisesOptions(Model.DestinoOpciones);

            //Model.PaisesOpciones = LPaises;
            Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Convert.ToString(Session["Countries"]));

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }

        // GET: Listas/Delete/5
        public ActionResult Delete(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.s = "panel-info";
            ViewBag.c = "Listas";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            try
            {
                var Model = objListing.GetListasByID(Convert.ToString(id));

                if (Model != null)
                {
                    objListing.DeleteListas(Model);

                    ViewBag.m = "Listas Deleted Successfully";
                }

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }

            //return RedirectToAction("Index", new { m = ViewBag.m});
            return PartialView("~/Views/Login/Error.cshtml");
        }


        // GET: Listas/Copy/5
        public ActionResult Copy(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.s = "panel-info";
            ViewBag.c = "Listas";
            ViewBag.a = "Copy";
            ViewBag.m = "";

            try
            {

                var Model = objListing.GetListasByID(Convert.ToString(id));

                if (Model != null)
                {

                    int c = objListing.GetListasComplete(Model.tpl_pk);

                    if (c > 0)
                    {

                        Model.tpl_insert_user = Session["UserID"].ToString();

                        objListing.CopyListas(Model);

                        ViewBag.m = "Listas Copied Successfully";

                    } else
                    {

                        ViewBag.m = "No es posible Copiar la lista, no esta completa";

                    }
                }

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }

            //return RedirectToAction("Index", new { m = ViewBag.m});
            return PartialView("~/Views/Login/Error.cshtml");
        }




        // GET: Listas/View/5
        public ActionResult View(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

     

            try
            {

                var Model = objListing.GetListasByID(Convert.ToString(id));

                if (Model != null)
                {

                    ViewBag.id = Model.tpl_pk;

                    Model.PaisesOpciones = objListing.GetPaisesOptions("", "", "'" + Model.tpl_pais_fk + "'");

                    Model.tpl_pais_fk = (string)Model.PaisesOpciones[0].id;

                    Model.TipoOpciones = objListing.GetTipoListasOptions();
                    Model.RegionalOpciones = objListing.GetLocalRegionalOptions();
                    Model.TransporteOpciones = objListing.GetTransportesOptions("");
                    Model.TipoCargaOptions = objListing.GetTiposOptions("TIPO_CARGA");
                    Model.MonedaOpciones = objListing.GetMonedasOptions();
                    Model.MovimientoOpciones = objListing.GetMovimientosOptions();

                    //Model.RangosList = objListing.AllRangosListing("");


                    ////// RUTAS

                    Model.RutasList = objListing.AllRutasListing("", Model.tpl_pk.ToString(), "");

                    List<int> ruta_old_ids = new List<int>();
                    string str = "";
                    foreach (Rutas ruta in Model.RutasList)
                    {




                        ruta_old_ids.Add(ruta.tpr_pk);

                        str += ruta.tpr_pk + ",";
                    }
                    str = str.TrimEnd(',');





                    ///// ARTICULOS

                    Model.ArticulosList = objListing.AllArticulosListing("", "", str, "");



                    ////// ARTICULOS RANGOS

                    //Model.ArticulosRangosList = objListing.AllArticulosRangosListing(str, "");




                    /////// ARTICULOS TARIFAS
                    //Model.ArticulosTarifasList = objListing.AllArticulosTarifasListing(str, "");

                    /////////////// ARTICULOS





                    //////////// RUTAS
                   



                }

                return View(Model);

            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }


        
        public JsonResult GetPuertos(string pais, string transporte)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            pais = String.IsNullOrEmpty(pais) ? "" : "'" + pais + "'";
            transporte = String.IsNullOrEmpty(transporte) ? "" : transporte ;

            IEnumerable<Puertos> puertosdata = objListing.AllPuertosListing(pais,transporte,"","","");

            if (puertosdata.Count() > 1) {

                //puertosdata = puertosdata.Prepend(new Puertos { tpp_pk = "", tpp_nombre = "Select" });
                IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };
                puertosdata = second.Concat(puertosdata);
            }

            return Json(puertosdata, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPaises(IEnumerable<Puertos> puertos)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            string paises = "";
            if (puertos != null)
            {
                foreach (Puertos item in puertos)
                {
                    if (!String.IsNullOrEmpty(item.tpp_pais_iso_fk))
                    {
                        if (paises != "") paises += ",";
                        paises += "'" + item.tpp_pais_iso_fk + "'";
                    }
                }
            } else
            {
                paises = "'99'";
            }

            LPaises = objListing.GetPaisesOptions("", "", paises);

            return Json(LPaises, JsonRequestBehavior.AllowGet);
        }

        
    }
}
