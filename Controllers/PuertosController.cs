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
    public class PuertosController : Controller
    {
        DataListing objListing;
        LoginController valida;

        public PuertosController()
        {
            objListing = new DataListing();
            valida = new LoginController();
        }




        // GET: Puertos/Index/5
        [HttpGet]
        public ActionResult Index(Puertos Model, FormCollection collection, string TextoBuscar, string tpp_pais_iso_fk, string tpp_transporte_fk, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            try
            {

                tpp_pais_iso_fk = String.IsNullOrEmpty(tpp_pais_iso_fk) ? "" : "'" + tpp_pais_iso_fk + "'";

                tpp_transporte_fk = String.IsNullOrEmpty(tpp_transporte_fk) ? "" : tpp_transporte_fk;

                Model.PuertosList = objListing.AllPuertosListing(tpp_pais_iso_fk, tpp_transporte_fk, "", "NO", TextoBuscar);

                //if (Model.PuertosList.Count() > 1)
                //  Model.PuertosList.First(). .RemoveAt(0);


                Model.PuertosPaisesList = objListing.AllPuertosListing("", tpp_transporte_fk, "", "NO", TextoBuscar);

                string paises = "";
                foreach (Puertos item in Model.PuertosPaisesList)
                {
                    if (!String.IsNullOrEmpty(item.tpp_pais_iso_fk))
                    {
                        if (paises != "") paises += ",";
                        paises += "'" + item.tpp_pais_iso_fk + "'";
                    }
                }


                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", paises);

                Model.TransporteOpciones = objListing.GetTransportesOptions("");

                            return View(Model);

            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;          
           
        }




        // GET: Puertos/Create
        public ActionResult Create()
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;
            
            ViewBag.actionType = "Create";

            Puertos Model = new Puertos();

            try
            {
                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", "");

                Model.TransporteOpciones = objListing.GetTransportesOptions("");

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

        // POST: Puertos/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Puertos Model)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            try
            {
                Model.tpp_tps_fk = "1";
                //Model.tpp_insert_user = Convert.ToInt32(Session["UserID"]);
                Model.tpp_insert_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.InsertPuertos(Model);

                    ViewBag.m = "Puertos Created Successfully";

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

            Model.PaisesOpciones = objListing.GetPaisesOptions("", "", "");

            Model.TransporteOpciones = objListing.GetTransportesOptions("");

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);

        }

        // GET: Puertos/Edit/5
        public ActionResult Edit(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);
            if (!t) return null;
            
            ViewBag.actionType = "Update";
            ViewBag.id_puerto = id;

            var Model = objListing.GetPuertosByID(Convert.ToString(id));


            try
            {
                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", "");

                Model.TransporteOpciones = objListing.GetTransportesOptions("");

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

        // POST: Puertos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Puertos Model, FormCollection collection, string actionType)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";

            ViewBag.id_puerto = Model.tpp_pk;
            //if (actionType == "Update")
            //{

                try
                {
                    //Model.tpp_update_user = Convert.ToInt32(Session["UserID"]);
                    Model.tpp_update_user = Session["UserID"].ToString();

                    if (ModelState.IsValid)
                    {
                        objListing.UpdatePuertos(Model);

                        ViewBag.m = "Puertos Updated Successfully";

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

            Model.PaisesOpciones = objListing.GetPaisesOptions("", "", "");

            Model.TransporteOpciones = objListing.GetTransportesOptions("");

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }


        // GET: Puertos/Delete/5
        public ActionResult Delete(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.s = "panel-info";
            ViewBag.c = "Puertos";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            try
            {

                var Model = objListing.GetPuertosByID(Convert.ToString(id));

                if (Model != null)
                {
                    objListing.DeletePuertos(Model);

                    ViewBag.m = "Puertos Deleted Successfully";
                }

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());

                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }

            return PartialView("~/Views/Login/Error.cshtml");

        }


    }
}
