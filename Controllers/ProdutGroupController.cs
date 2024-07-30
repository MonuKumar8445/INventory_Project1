using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Repository;

namespace INventory_Project1.Controllers
{
    [Authorize]
    public class ProductGroupController : Controller
    {


        private readonly IProductGroup _productGroupRepo;
        //private IProductProfile _productProfile;

        public ProductGroupController(IProductGroup productGroupRepo) // here the repository will be passed by the dependency injection.
        {
            _productGroupRepo = productGroupRepo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<ProductGroup> productGroups = _productGroupRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(productGroups.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(productGroups);
        }


        public IActionResult Create()
        {
            ProductGroup productGroup = new ProductGroup();
            return View(productGroup);
        }

        [HttpPost]
        public IActionResult Create(ProductGroup productGroup)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (productGroup.Description.Length < 4 || productGroup.Description == null)
                    errMessage = "ProductGroup Description Must be atleast 4 Characters";

                if (_productGroupRepo.IsProductGroupNameExists(productGroup.Name) == true)
                    errMessage = errMessage + " " + " ProductGroup Name " + productGroup.Name + " Exists Already";

                if (errMessage == "")
                {
                    productGroup = _productGroupRepo.Create(productGroup);
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
                return View(productGroup);
            }
            else
            {
                TempData["SuccessMessage"] = "ProductGroup " + productGroup.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            ProductGroup productGroup = _productGroupRepo.GetProductGroup(id);
            return View(productGroup);
        }


        public IActionResult Edit(int id)
        {
            ProductGroup productGroup = _productGroupRepo.GetProductGroup(id);
            TempData.Keep();
            return View(productGroup);
        }

        [HttpPost]
        public IActionResult Edit(ProductGroup productGroup)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (productGroup.Description.Length < 4 || productGroup.Description == null)
                    errMessage = "ProductGroup Description Must be atleast 4 Characters";

                if (_productGroupRepo.IsProductGroupNameExists(productGroup.Name, productGroup.Id) == true)
                    errMessage = errMessage + "ProductGroup Name " + productGroup.Name + " Already Exists";

                if (errMessage == "")
                {
                    productGroup = _productGroupRepo.Edit(productGroup);
                    TempData["SuccessMessage"] = productGroup.Name + ", ProductGroup Saved Successfully";
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
                return View(productGroup);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            ProductGroup productGroup = _productGroupRepo.GetProductGroup(id);
            TempData.Keep();
            return View(productGroup);
        }


        [HttpPost]
        public IActionResult Delete(ProductGroup productGroup)
        {
            try
            {
                productGroup = _productGroupRepo.Delete(productGroup);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productGroup);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "ProductGroup " + productGroup.Name + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }




    }
}

