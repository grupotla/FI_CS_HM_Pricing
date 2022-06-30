using ti_pricing.Models;
using ti_pricing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ti_pricing.Controllers
{
    public class RutasController : Controller
    {
        DataListing objListing;
        LoginController valida;
        List<Opciones> LPaises;

        public RutasController()
        {
            objListing = new DataListing();
            valida = new LoginController();
            LPaises = new List<Opciones>();
        }

        public ActionResult Articulos(string id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/Articulos/Index?id_ruta=" + id_ruta);
            return null;
        }

        // GET: Rutas
        [HttpGet]
        public ActionResult Index(Rutas Model, FormCollection collection, string TextoBuscar, string id_lista, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            string controller = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();

            ViewBag.controller = controller;

            try
            {

                ViewBag.id_lista = id_lista;

                Model.Listas = objListing.AllListasListing("","","","",id_lista).First();

                ViewBag.tipo = Model.Listas.tpl_tipo;

                ViewBag.tipo_entidad = "";

                switch (Model.Listas.tpl_transporte_fk)
                {
                    case "Aereo":
                    case "1":
                        ViewBag.tipo_entidad = "4"; //carriers
                        break;
                    case "Maritimo FCL": //2
                    case "Maritimo LCL": //3
                    case "2": //2
                    case "3": //3
                        ViewBag.tipo_entidad = "5"; //navieras
                        break;
                    case "Terrestre LTL": //4
                    case "Terrestre FTL": //5
                    case "Terrestre Local": //6
                    case "4": //4
                    case "5": //5
                    case "6": //6
                        ViewBag.tipo_entidad = "2"; //proveedores
                        break;
                }

                Model.RutasList = objListing.AllRutasListing(TextoBuscar, id_lista, "");

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + controller + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }

        // GET: Rutas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rutas/Create
        public ActionResult Create(string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            ViewBag.id_lista = id_lista;

            Rutas Model = new Rutas();

            try
            {
                Model.tpr_tpl_fk = int.Parse(id_lista);

                Model.Listas = objListing.AllListasListing("", "", "", "", id_lista).First();
                Model.tpr_transporte_fk = Model.Listas.tpl_transporte_fk;
                //Model.TransporteOpciones = objListing.GetTransportesOptions(Model.tpr_transporte_fk);

                //Model.TipoCargaOptions = objListing.GetMedidasOptions();
                //Model.MonedaOpciones = objListing.GetMonedasOptions();

                //Model.PaisesOpciones = objListing.GetPaises("", "", "'" + Model.Listas.tpl_pais_fk.ToString() + "'");


                string catalogo = "";
                string transporte = "";

                switch (Model.Listas.tpl_transporte_fk)
                {
                    case "Aereo":
                        catalogo = "4"; //carriers
                        transporte = "1";
                        break;
                    case "Maritimo FCL": //2
                        catalogo = "5"; //navieras
                        transporte = "2";
                        break;
                    case "Maritimo LCL": //3
                        catalogo = "5"; //navieras
                        transporte = "3";
                        break;
                    case "Terrestre LTL": //4
                        catalogo = "2"; //proveedores
                        transporte = "4";
                        break;
                    case "Terrestre FTL": //5
                        catalogo = "2"; //proveedores
                        transporte = "5";
                        break;
                    case "Terrestre Local": //6
                        catalogo = "2"; //proveedores
                        transporte = "6";
                        break;
                }

                Model.OrigenOpciones = objListing.AllPuertosListing("", transporte, "", "SI", "");

                IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };
                Model.OrigenOpciones = second.Concat(Model.OrigenOpciones);

                //    .Prepend(new Puertos { tpp_pk = "", tpp_nombre = "Select" });

                Model.DestinoOpciones = Model.OrigenOpciones;

                GetPaises(Model.DestinoOpciones);


                Model.PaisesOpciones = LPaises;

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);



                Model.RepresentantesOpciones = objListing.GetRepresentantesOptions(catalogo, "", "", "");


                string test = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                return null;
            }


            return View("Form", Model);
        }

        // POST: Rutas/Create
        [HttpPost]
        public ActionResult Create(Rutas Model)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            try
            {
                Model.tpr_tps_fk = "1";
                Model.tpr_insert_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.InsertRutas(Model);

                    ViewBag.m = "Rutas Created Successfully";

                    return RedirectToAction("Index", new { id_lista = Model.tpr_tpl_fk, m = ViewBag.m });
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

            ViewBag.id_lista = Model.tpr_tpl_fk.ToString();



            Model.Listas = objListing.AllListasListing("", "", "", "", Model.tpr_tpl_fk.ToString()).First();

            Model.tpr_transporte_fk = Model.Listas.tpl_transporte_fk;
            //Model.TransporteOpciones = objListing.GetTransportesOptions(Model.tpr_transporte_fk);

            //Model.TipoCargaOptions = objListing.GetMedidasOptions();
            //Model.MonedaOpciones = objListing.GetMonedasOptions();

            //Model.PaisesOpciones = objListing.GetPaises("", "", Model.Listas.tpl_pais_fk.ToString());


            string catalogo = "";
            string transporte = "";

            switch (Model.Listas.tpl_transporte_fk)
            {
                case "Aereo":
                    catalogo = "4"; //carriers
                    transporte = "1";
                    break;
                case "Maritimo FCL": //2
                    catalogo = "5"; //navieras
                    transporte = "2";
                    break;
                case "Maritimo LCL": //3
                    catalogo = "5"; //navieras
                    transporte = "3";
                    break;
                case "Terrestre LTL": //4
                    catalogo = "2"; //proveedores
                    transporte = "4";
                    break;
                case "Terrestre FTL": //5
                    catalogo = "2"; //proveedores
                    transporte = "5";
                    break;
                case "Terrestre Local": //6
                    catalogo = "2"; //proveedores
                    transporte = "6";
                    break;
            }

            Model.OrigenOpciones = objListing.AllPuertosListing("", transporte, "", "SI", "");

            //Model.OrigenOpciones = Model.OrigenOpciones.Prepend(new Puertos { tpp_pk = "", tpp_nombre = "Select" });

            IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };
            Model.OrigenOpciones = second.Concat(Model.OrigenOpciones);

            Model.DestinoOpciones = Model.OrigenOpciones;

            GetPaises(Model.DestinoOpciones);

            Model.PaisesOpciones = LPaises;

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);


            Model.RepresentantesOpciones = objListing.GetRepresentantesOptions(catalogo, "", "", "");

            return View("Form", Model);
        }

        // GET: Rutas/Edit/5
        public ActionResult Edit(int id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";
            ViewBag.id_ruta = id_ruta;

            var Model = objListing.GetRutasByID(Convert.ToString(id_ruta));  


            try
            {
                ViewBag.id_lista = Model.tpr_tpl_fk;

                Model.ArticulosList = objListing.AllArticulosListing("", "", id_ruta.ToString(), "");

                ViewBag.Articulos = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? Model.ArticulosList.Count() : 0;


                Model.Listas = objListing.AllListasListing("", "", "", "", Model.tpr_tpl_fk.ToString()).First();

                Model.tpr_transporte_fk = Model.Listas.tpl_transporte_fk;
//                Model.TransporteOpciones = objListing.GetTransportesOptions(Model.tpr_transporte_fk);

//                Model.TipoCargaOptions = objListing.GetMedidasOptions();
//                Model.MonedaOpciones = objListing.GetMonedasOptions();

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                string catalogo = "";
                string transporte = "";

                switch (Model.Listas.tpl_transporte_fk)
                {
                    case "Aereo":
                        catalogo = "4"; //carriers
                        transporte = "1";
                        break;
                    case "Maritimo FCL": //2
                        catalogo = "5"; //navieras
                        transporte = "2";
                        break;
                    case "Maritimo LCL": //3
                        catalogo = "5"; //navieras
                        transporte = "3";
                        break;
                    case "Terrestre LTL": //4
                        catalogo = "2"; //proveedores
                        transporte = "4";
                        break;
                    case "Terrestre FTL": //5
                        catalogo = "2"; //proveedores
                        transporte = "5";
                        break;
                    case "Terrestre Local": //6
                        catalogo = "2"; //proveedores
                        transporte = "6";
                        break;
                }

                Model.OrigenOpciones = objListing.AllPuertosListing("", transporte, "", "SI", "");
                Model.DestinoOpciones = Model.OrigenOpciones;



                Rutas temp = objListing.AllRutasListing("", "", id_ruta.ToString()).First();

                //Model.OrigenOpcion = objListing.GetPuertosByID(Model.tpr_tpp_origen_fk.ToString());
                //Model.DestinoOpcion = objListing.GetPuertosByID(Model.tpr_tpp_destino_fk.ToString());

                Model.tpr_tpp_pais_origen_fk = temp.tpr_tpp_pais_origen_fk;
                Model.tpr_tpp_pais_destino_fk = temp.tpr_tpp_pais_destino_fk;
                Model.tpr_transporte_fk = Model.tpr_transporte_fk;


                GetPaises(Model.DestinoOpciones);

                Model.PaisesOpciones = LPaises;



                Model.RepresentantesOpciones = objListing.GetRepresentantesOptions(catalogo, "", "", "");


                return View("Form", Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }

        // POST: Rutas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rutas Model, FormCollection collection)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            try
            {

                Model.tpr_update_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.UpdateRutas(Model);

                    ViewBag.m = "Rutas Updated Successfully";

                    return RedirectToAction("Index", new { id_lista = Model.tpr_tpl_fk, m = ViewBag.m });

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

            ViewBag.id_lista = Model.tpr_tpl_fk;

            Model.ArticulosList = objListing.AllArticulosListing("", "", Model.tpr_pk.ToString(), "");

            ViewBag.Articulos = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? Model.ArticulosList.Count() : 0;


            Model.Listas = objListing.AllListasListing("", "", "", "", Model.tpr_tpl_fk.ToString()).First();

            Model.tpr_transporte_fk = Model.Listas.tpl_transporte_fk;
            //                Model.TransporteOpciones = objListing.GetTransportesOptions(Model.tpr_transporte_fk);

            //                Model.TipoCargaOptions = objListing.GetMedidasOptions();
            //                Model.MonedaOpciones = objListing.GetMonedasOptions();

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            string catalogo = "";
            string transporte = "";

            switch (Model.Listas.tpl_transporte_fk)
            {
                case "Aereo":
                    catalogo = "4"; //carriers
                    transporte = "1";
                    break;
                case "Maritimo FCL": //2
                    catalogo = "5"; //navieras
                    transporte = "2";
                    break;
                case "Maritimo LCL": //3
                    catalogo = "5"; //navieras
                    transporte = "3";
                    break;
                case "Terrestre LTL": //4
                    catalogo = "2"; //proveedores
                    transporte = "4";
                    break;
                case "Terrestre FTL": //5
                    catalogo = "2"; //proveedores
                    transporte = "5";
                    break;
                case "Terrestre Local": //6
                    catalogo = "2"; //proveedores
                    transporte = "6";
                    break;
            }

            Model.OrigenOpciones = objListing.AllPuertosListing("", transporte, "", "SI", "");
            Model.DestinoOpciones = Model.OrigenOpciones;



            Rutas temp = objListing.AllRutasListing("", "", Model.tpr_pk.ToString()).First();

            //Model.OrigenOpcion = objListing.GetPuertosByID(Model.tpr_tpp_origen_fk.ToString());
            //Model.DestinoOpcion = objListing.GetPuertosByID(Model.tpr_tpp_destino_fk.ToString());

            Model.tpr_tpp_pais_origen_fk = temp.tpr_tpp_pais_origen_fk;
            Model.tpr_tpp_pais_destino_fk = temp.tpr_tpp_pais_destino_fk;
            Model.tpr_transporte_fk = Model.tpr_transporte_fk;


            GetPaises(Model.DestinoOpciones);

            Model.PaisesOpciones = LPaises;



            Model.RepresentantesOpciones = objListing.GetRepresentantesOptions(catalogo, "", "", "");



            /*


            ViewBag.id_lista = Model.tpr_tpl_fk;

            ViewBag.id_ruta = Model.tpr_pk;


            Model.Listas = objListing.GetListasByID(Model.tpr_tpl_fk.ToString());
            Model.tpr_transporte_fk = Model.Listas.tpl_transporte_fk;
            Model.TransporteOpciones = objListing.GetTransportesOptions(Model.tpr_transporte_fk);


            string catalogo = "";
            string transporte = "";

            switch (Model.Listas.tpl_transporte_fk)
            {
                case "Aereo":
                    catalogo = "4"; //carriers
                    transporte = "1";
                    break;
                case "Maritimo FCL": //2
                    catalogo = "5"; //navieras
                    transporte = "2";
                    break;
                case "Maritimo LCL": //3
                    catalogo = "5"; //navieras
                    transporte = "3";
                    break;
                case "Terrestre LTL": //4
                    catalogo = "2"; //proveedores
                    transporte = "4";
                    break;
                case "Terrestre FTL": //5
                    catalogo = "2"; //proveedores
                    transporte = "5";
                    break;
                case "Terrestre Local": //6
                    catalogo = "2"; //proveedores
                    transporte = "6";
                    break;
            }

            //Model.PaisesOpciones = objListing.GetPaises("", "", Model.Listas.tpl_pais_fk.ToString());
            Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Listas.tpl_transporte_fk.ToString());
            Model.TipoCargaOptions = objListing.GetMedidasOptions();
            Model.MonedaOpciones = objListing.GetMonedasOptions();

            Model.OrigenOpciones = objListing.AllPuertosListing("", transporte, "", "SI", "");

            Model.DestinoOpciones = Model.OrigenOpciones;

            GetPaises(Model.DestinoOpciones);

            Model.PaisesOpciones = LPaises;

            Model.OrigenOpcion = objListing.GetPuertosByID(Model.tpr_tpp_origen_fk.ToString());
            Model.DestinoOpcion = objListing.GetPuertosByID(Model.tpr_tpp_destino_fk.ToString());

            Model.tpr_tpp_pais_origen_fk = Model.OrigenOpcion.tpp_pais_iso_fk;
            Model.tpr_tpp_pais_destino_fk = Model.DestinoOpcion.tpp_pais_iso_fk;
            Model.tpr_transporte_fk = Model.OrigenOpcion.tpp_transporte_fk.ToString();

            Model.PaisesOpciones = LPaises;

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);


            Model.RepresentantesOpciones = objListing.GetRepresentantesOptions(catalogo, "", "", "");
            */


            return View("Form", Model);
        }



        // POST: Rutas/Delete/5
        public ActionResult Delete(int id, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.s = "panel-info";
            ViewBag.c = "Rutas";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            ViewBag.id_lista = String.IsNullOrEmpty(id_lista) ? "" : "?id_lista=" + id_lista;

            try
            {
                var Model = objListing.GetRutasByID(Convert.ToString(id));

                if (Model != null)
                {
                    objListing.DeleteRutas(Model);

                    ViewBag.m = "Rutas Deleted Successfully";
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

        public JsonResult GetPuertos(string pais, string transporte)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            pais = String.IsNullOrEmpty(pais) ? "" : "'" + pais + "'";
            transporte = String.IsNullOrEmpty(transporte) ? "" : transporte;

            IEnumerable<Puertos> puertosdata = objListing.AllPuertosListing(pais, transporte, "", "", "");

            if (puertosdata.Count() > 1)
            {
                IEnumerable<Puertos> second = new[] { new Puertos { tpp_pk = "", tpp_nombre = "Select" } };
                puertosdata = second.Concat(puertosdata);

                //puertosdata = puertosdata.Prepend(new Puertos { tpp_pk = "", tpp_nombre = "Select" });
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
                    if (!String.IsNullOrEmpty(item.tpp_pais_iso_fk)) {
                        if (paises != "") paises += ",";
                        paises += "'" + item.tpp_pais_iso_fk + "'";
                    }
                }
            }
            else
            {
                paises = "'99'";
            }

            LPaises = objListing.GetPaisesOptions("", "", paises);

            return Json(LPaises, JsonRequestBehavior.AllowGet);
        }
    }
}
