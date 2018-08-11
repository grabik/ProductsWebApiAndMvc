using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVCApp.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            IEnumerable<MVCProductsModel> empList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products").Result;
            empList = response.Content.ReadAsAsync<IEnumerable<MVCProductsModel>>().Result;
            return View(empList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new MVCProductsModel());
            else
            {
                HttpResponseMessage responce = GlobalVariables.WebApiClient.GetAsync("Products/" + id.ToString()).Result;
                return View(responce.Content.ReadAsAsync<MVCProductsModel>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(MVCProductsModel emp)
        {
            if (emp.ID == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Products", emp).Result;
                TempData["SuccessMessage"] = "Saved Succesfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Products/" + emp.ID, emp).Result;
                TempData["SuccessMessage"] = "Updated Succesfully";
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage responce = GlobalVariables.WebApiClient.DeleteAsync("Products/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted Succesfully";
            return RedirectToAction("Index");
        }
    }
}