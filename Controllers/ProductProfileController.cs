using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Repository;

namespace INventory_Project1.Controllers
{
    [Authorize]
    public class ProductProfileController : Controller
    {

        
        private readonly IProductProfile _productProfileRepo;
        //private IProductProfile _productProfile;

        public ProductProfileController(IProductProfile productProfileRepo) // here the repository will be passed by the dependency injection.
        {
            _productProfileRepo = productProfileRepo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<ProductProfile> productProfiles = _productProfileRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(productProfiles.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(productProfiles);
        }


        public IActionResult Create()
        {
            ProductProfile productProfile = new ProductProfile();
            return View(productProfile);
        }

        [HttpPost]
        public IActionResult Create(ProductProfile productProfile)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (productProfile.Description.Length < 4 || productProfile.Description == null)
                    errMessage = "ProducProfile Description Must be atleast 4 Characters";

                if (_productProfileRepo.IsProductProfileNameExists(productProfile.Name) == true)
                    errMessage = errMessage + " " + " ProducProfile Name " + productProfile.Name + " Exists Already";

                if (errMessage == "")
                {
                    productProfile = _productProfileRepo.Create(productProfile);
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
                return View(productProfile);
            }
            else
            {
                TempData["SuccessMessage"] = "ProductProfile " + productProfile.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            ProductProfile productProfile = _productProfileRepo.GetProductProfile(id);
            return View(productProfile);
        }


        public IActionResult Edit(int id)
        {
            ProductProfile productProfile = _productProfileRepo.GetProductProfile(id);
            TempData.Keep();
            return View(productProfile);
        }

        [HttpPost]
        public IActionResult Edit(ProductProfile productProfile)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (productProfile.Description.Length < 4 || productProfile.Description == null)
                    errMessage = "ProductProfile Description Must be atleast 4 Characters";

                if (_productProfileRepo.IsProductProfileNameExists(productProfile.Name, productProfile.Id) == true)
                    errMessage = errMessage + "ProductProfile Name " + productProfile.Name + " Already Exists";

                if (errMessage == "")
                {
                    productProfile = _productProfileRepo.Edit(productProfile);
                    TempData["SuccessMessage"] = productProfile.Name + ", ProductProfile Saved Successfully";
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
                return View(productProfile);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            ProductProfile productProfile = _productProfileRepo.GetProductProfile(id);
            TempData.Keep();
            return View(productProfile);
        }


        [HttpPost]
        public IActionResult Delete(ProductProfile productProfile)
        {
            try
            {
                productProfile = _productProfileRepo.Delete(productProfile);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productProfile);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "ProductProfile " + productProfile.Name + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }


    }
}

