

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;

namespace INventory_Project1.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {

        private readonly ICategory _categoryrepo;

        public CategoryController(ICategory categoryrepo) // here the repository will be passed by the dependency injection.
        {
            
            _categoryrepo = categoryrepo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Category> categories =  _categoryrepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(categories.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(categories);
        }


        public IActionResult Create()
        {
            Category category = new Category();
            return View(category); 
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (category.Description.Length < 4 || category.Description == null)
                    errMessage = "Category Description Must be atleast 4 Characters";

                if ( _categoryrepo.IsCategoryNameExists(category.Name) == true)
                    errMessage = errMessage + " " + " Category Name " + category.Name + " Exists Already";

                if (errMessage == "")
                {
                    category =  _categoryrepo.Create(category);
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
                return View(category);
            }
            else
            {
                TempData["SuccessMessage"] = "Category " + category.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            Category category =  _categoryrepo.GetCategory(id);
            return View(category);
        }


        public IActionResult Edit(int id)
        {
            Category category =  _categoryrepo.GetCategory(id);
            TempData.Keep();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (category.Description.Length < 4 || category.Description == null)
                    errMessage = "Category Description Must be atleast 4 Characters";

                if ( _categoryrepo.IsCategoryNameExists(category.Name, category.Id) == true)
                    errMessage = errMessage + "Category Name " + category.Name + " Already Exists";

                if (errMessage == "")
                {
                    category =  _categoryrepo.Edit(category);
                    TempData["SuccessMessage"] = category.Name + ", Category Saved Successfully";
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
                return View(category);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            Category category =  _categoryrepo.GetCategory(id);
            TempData.Keep();
            return View(category);
        }


        [HttpPost]
        public IActionResult Delete(Category category)
        {
            try
            {
                category =  _categoryrepo.Delete(category);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(category);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Categoey " + category.Name + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }


    }
}

