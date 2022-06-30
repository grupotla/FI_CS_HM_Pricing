using ti_pricing.Models;
using ti_pricing.Repository;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace ti_pricing.Controllers
{
    public class RangosController : Controller
    {
        DataListing objListing;
        LoginController valida;


        public RangosController()
        {
            objListing = new DataListing();
            valida = new LoginController();
        }





        // GET: Rangos/Index/5
        [HttpGet]
        public ActionResult Index(Rangos Model, FormCollection collection, string TextoBuscar, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            try
            {

                Model.RangosList = objListing.AllRangosListing(TextoBuscar);

                return View(Model);

            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
            
        }




        // GET: Rangos/Create
        public ActionResult Create()
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            try
            {
                Rangos Model = new Rangos();

                //Model.tpg_tipo = "";

                Model.TiposOpciones = objListing.GetTiposOptions("RANGO");

                ViewBag.actionType = "Create";

                return View("Form", Model);

            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }
            return null;
        }

        // POST: Rangos/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rangos Model)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            try
            {
                Model.tpg_tps_fk = "1";
                //Model.tpg_insert_user = Convert.ToInt32(Session["UserID"]);
                Model.tpg_insert_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.InsertRangos(Model);

                    ViewBag.m = "Rangos Created Successfully " + Model.tpg_valor;

                    return RedirectToAction("Index", new { m = ViewBag.m});
                }
                else
                {
                    ViewBag.errors = ModelState.Values;
                    /*
                    ModelState.AddModelError("Error", "something its wrong");
                    foreach (var item in ModelState) {
                        var tVals = item.Value;
                    }*/
                }

                Model.TiposOpciones = objListing.GetTiposOptions("RANGO");


                return View("Form", Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;

        }

        // GET: Rangos/Edit/5
        public ActionResult Edit(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            ViewBag.id_rango = id;

            if (!t) return null;
            
            ViewBag.actionType = "Update";

            var Model = objListing.GetRangosByID(Convert.ToString(id));

            Model.ArticulosRangosList = objListing.AllArticulosRangosListing("",id.ToString());

            int c = 0;
            foreach (ArticulosRangos item in Model.ArticulosRangosList) //se hizo esto porque el count no esta habilitado ??????
            {
                c++;
            }

            ViewBag.Articulos = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? c : 0;


            Model.TiposOpciones = objListing.GetTiposOptions("RANGO");



            return View("Form", Model);
            
        }

        // POST: Rangos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rangos Model, FormCollection collection, string actionType)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";

            ViewBag.id_rango = Model.tpg_pk;

                try
                {
                    //Model.tpg_update_user = Convert.ToInt32(Session["UserID"]);
                    Model.tpg_update_user = Session["UserID"].ToString();

                    if (ModelState.IsValid)
                    {
                        objListing.UpdateRangos(Model);

                        ViewBag.m = "Rangos Updated Successfully";

                        return RedirectToAction("Index", new { m = ViewBag.m});
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "something its wrong");

                    }


                Model.TiposOpciones = objListing.GetTiposOptions("RANGO");

                return View("Form", Model);
            }
            catch (Exception e)
                {
                    Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                }

            return null;

        }


        // GET: Rangos/Delete/5
        public ActionResult Delete(int id)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.s = "panel-info";
            ViewBag.c = "Rangos";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            try
            {
                var Model = objListing.GetRangosByID(Convert.ToString(id));

                if (Model != null)
                {
                    objListing.DeleteRangos(Model);

                    ViewBag.m = "Rangos Deleted Successfully";
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
