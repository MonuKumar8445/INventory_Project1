using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Repository;
using INnventory_Project1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace INventory_Project1.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {

        private readonly ISupplier _supplierRepo;
     

        public SupplierController(ISupplier supplierRepo) // here the repository will be passed by the dependency injection.
        {
            _supplierRepo = supplierRepo;
          
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("code");
            sortModel.AddColumn("name");
            sortModel.ApplySort(sortExpression);

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Supplier> suppliers = _supplierRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(suppliers.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;
           


            return View(suppliers);
        }

        public IActionResult Create()
        {
            Supplier supplier = new Supplier();
           
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (supplier.Address.Length < 4 || supplier.Address == null)
                    errMessage = "Supplier Address Must be atleast 4 Characters";

                if (_supplierRepo.IsSupplierNameExists(supplier.Name) == true)
                    errMessage = errMessage + " " + " Suppier Name " + supplier.Name + " Exists Already";

                if (_supplierRepo.IsSupplierCodeExists(supplier.Code) == true)
                    errMessage = errMessage + " " + " Suppier Code " + supplier.Code + " Exists Already";

                if (_supplierRepo.IsSupplierEmailExists(supplier.EmailId) == true)
                    errMessage = errMessage + " " + " Suppier EmailId " + supplier.EmailId + " Exists Already";

                if (errMessage == "")
                {

                    supplier = _supplierRepo.Create(supplier);
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
                return View(supplier);
            }
            else
            {
                TempData["SuccessMessage"] = "Supplier " + supplier.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int Id) //Read
        {
            Supplier supplier = _supplierRepo.GetItem(Id);
    
            return View(supplier);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {

            Supplier supplier = _supplierRepo.GetItem(Id);
            if(supplier is null)
            {
                return View(new Supplier());
            }
         
            TempData.Keep();
            TempData["SuccessMessage"] = supplier.Name + ", Supplier Saved Successfully";

            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (supplier.Address.Length < 4 || supplier.Address == null)
                    errMessage = "Supplier Address Must be atleast 4 Characters";

                if (_supplierRepo.IsSupplierNameExists(supplier.Name, supplier.Id) == true)
                    errMessage = errMessage + "Supplier Name " + supplier.Name + " Already Exists";


                if (_supplierRepo.IsSupplierCodeExists(supplier.Code, supplier.Id) == true)
                    errMessage = errMessage + "Supplier Code " + supplier.Code + " Already Exists";


                if (_supplierRepo.IsSupplierNameExists(supplier.EmailId, supplier.Id) == true)
                    errMessage = errMessage + "Supplier EmailId " + supplier.EmailId + " Already Exists";


                if (errMessage == "")
                {
                    supplier = _supplierRepo.Edit(supplier);
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
                return View(supplier);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [HttpGet]
        public IActionResult Delete(int Id )
        {
            Supplier supplier = _supplierRepo.GetItem(Id);
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Delete(Supplier supplier)
        {
            try
            {
                supplier = _supplierRepo.Delete(supplier);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(supplier);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Supplier " + supplier.Name + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
       
    }
}

