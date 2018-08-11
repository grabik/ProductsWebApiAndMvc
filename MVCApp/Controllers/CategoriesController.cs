using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVCApp.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index()
        {
            IEnumerable<MVCCategoriesModel> catList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories").Result;
            catList = response.Content.ReadAsAsync<IEnumerable<MVCCategoriesModel>>().Result;
            return View(catList);
        }
        public ActionResult AddOrEditCategories(string name = "")
        {
            if (name == "")
                return View(new MVCCategoriesModel());
            else
            {
                HttpResponseMessage responce = GlobalVariables.WebApiClient.GetAsync("Categories/" + name.ToString()).Result;
                return View(responce.Content.ReadAsAsync<MVCCategoriesModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddOrEditCategories(MVCCategoriesModel cat)
        {
            if (cat.Name == "")
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories", cat).Result;
                TempData["SuccessMessage"] = "Saved Succesfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Categories/" + cat.Name, cat).Result;
                TempData["SuccessMessage"] = "Updated Succesfully";
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteCategories(string name)
        {
            HttpResponseMessage responce = GlobalVariables.WebApiClient.DeleteAsync("Categories/" + name.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted Succesfully";
            return RedirectToAction("Index");
        }
    }
}