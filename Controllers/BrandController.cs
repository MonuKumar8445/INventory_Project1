

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;

namespace INventory_Project1.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {

        private readonly IBrand _brandrepo;

        public BrandController(IBrand brandrepo) // here the repository will be passed by the dependency injection.
        {

            _brandrepo = brandrepo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Brand> brands = _brandrepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(brands.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(brands);
        }


        public IActionResult Create()
        {
            Brand brand = new Brand();
            return View(brand);
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (brand.Description.Length < 4 || brand.Description == null)
                    errMessage = "Brand Description Must be atleast 4 Characters";

                if (_brandrepo.IsBrandNameExists(brand.Name) == true)
                    errMessage = errMessage + " " + " Brand Name " + brand.Name + " Exists Already";

                if (errMessage == "")
                {
                    brand =_brandrepo.Create(brand);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);
            }
            else
            {
                TempData["SuccessMessage"] = "Brand " + brand.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            Brand brand = _brandrepo.GetBrand(id);
            return View(brand);
        }


        public IActionResult Edit(int id)
        {
            Brand brand = _brandrepo.GetBrand(id);
            TempData.Keep();
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (brand.Description.Length < 4 || brand.Description == null)
                    errMessage = "Brand Description Must be atleast 4 Characters";

                if (_brandrepo.IsBrandNameExists(brand.Name, brand.Id) == true)
                    errMessage = errMessage + "Brand Name " + brand.Name + " Already Exists";

                if (errMessage == "")
                {
                    brand = _brandrepo.Edit(brand);
                    TempData["SuccessMessage"] = brand.Name + ", Brand Saved Successfully";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }



            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            Brand brand = _brandrepo.GetBrand(id);
            TempData.Keep();
            return View(brand);
        }


        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            try
            {
                brand = _brandrepo.Delete(brand);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Brand " + brand.Name + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }


    }
}

