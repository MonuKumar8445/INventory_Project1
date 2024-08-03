using INventory_Project1.Interface;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace INventory_Project1.Controllers
{
    [Authorize]
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrder _purchaseOrderRepo;
        private readonly IProduct _productRepo;
        private readonly ISupplier _supplierRepo;
        private readonly  ICurrency _currencyRepo;
        public PurchaseOrderController(IPurchaseOrder purchaseOrderRepo, IProduct ProductRepo, ISupplier supplierRepo, ICurrency currencyRepo) // here the repository will be passed by the dependency injection.
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _productRepo = ProductRepo;
            _supplierRepo = supplierRepo;
            _currencyRepo = currencyRepo;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("poNumber");
            sortModel.AddColumn("quoNmber");
            sortModel.ApplySort(sortExpression);
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<PoHeader> poHeaders = _purchaseOrderRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(poHeaders.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(poHeaders);
        }
        public IActionResult Create()
        {
            PoHeader poHeader = new PoHeader();
            ViewBag.ProductList = GetProducts();
            poHeader.PoDetails.Add(new
            PoDetail() { Id =  1 });
            ViewBag.SupplierId = GetSuppliers();
            ViewBag.CurrencyList = GetPoCurrencies();
            ViewBag.CurrencyList = GetBaseCurrencies();
            ViewBag.ExchangeRate = GetExchangeRate();
            //ViewBag.ProductList = GetProductsDes();
            //ViewBag.UnitNames = GetUnitNames();
            poHeader.PoNumber = _purchaseOrderRepo.GetNewPoNumber();
            return View(poHeader);
        }
        [HttpPost]
        public IActionResult Create(PoHeader poHeader)
        {
            poHeader.PoDetails.RemoveAll(a=>a.Quantity == 0);
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_purchaseOrderRepo.IsPoNumberExists(poHeader.PoNumber) == true)
                    errMessage = errMessage + " " + " PoNumber " + poHeader.PoNumber + " Exists Already";

                if (_purchaseOrderRepo.IsQuotaionNoExists(poHeader.QuotationNo) == true)
                    errMessage = errMessage + " " + " Quotaion " + poHeader.QuotationNo + " Exists Already";
                if (errMessage == "")
                {
                    poHeader = _purchaseOrderRepo.Create(poHeader);
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
                return View(poHeader);
            }
            else
            {
                TempData["SuccessMessage"] = "PoHeader " + poHeader.PoNumber + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int Id) //Read
        {
            PoHeader poHeader = _purchaseOrderRepo.GetItem(Id);

            return View(poHeader);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            PoHeader poHeader = _purchaseOrderRepo.GetItem(Id);
            if (poHeader is null)
            {
                return View(new PoHeader());
            }
            TempData.Keep();
            TempData["SuccessMessage"] = poHeader.PoNumber + ", PoHeader Saved Successfully";
            return View(poHeader);
        }
        [HttpPost]
        public IActionResult Edit(PoHeader poHeader)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_purchaseOrderRepo.IsPoNumberExists(poHeader.PoNumber) == true)
                    errMessage = errMessage + " " + " PoNumber " + poHeader.PoNumber + " Exists Already";

                if (_purchaseOrderRepo.IsQuotaionNoExists(poHeader.QuotationNo) == true)
                    errMessage = errMessage + " " + " Quotaion " + poHeader.QuotationNo + " Exists Already";
                if (errMessage == "")
                {
                    poHeader = _purchaseOrderRepo.Edit(poHeader);
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
                return View(poHeader);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            PoHeader poHeader = _purchaseOrderRepo.GetItem(Id);
            return View(poHeader);
        }
        [HttpPost]
        public IActionResult Delete(PoHeader poHeader)
        {
            try
            {
                poHeader = _purchaseOrderRepo.Delete(poHeader);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(poHeader);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "PoHeader " + poHeader.PoNumber + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });
        }




        private List<SelectListItem> GetProducts()
        {
            var products = _productRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            var productList = products.Select(p => new SelectListItem
            {
                Value = p.Code.ToString(),
                Text = p.Code
            }).ToList();

            productList.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Product"
            });

            return productList;
        }








        //private List<SelectListItem> GetProducts()
        //{
        //    var IsProducts = new List<SelectListItem>();
        //    PaginatedList<Product> Product = _productRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
        //    IsProducts = Product.Select(ut => new SelectListItem()
        //    {
        //        Value = ut.Code.ToString(),
        //        Text = ut.Code,
        //    }).ToList();
        //    var deftItem = new SelectListItem()
        //    {
        //        Value = "",
        //        Text = "Product"
        //    };
        //    IsProducts.Insert(0, deftItem);
        //    return IsProducts;

        //}
        //private List<SelectListItem> GetProductsDes()
        //{
        //    var IsProducts = new List<SelectListItem>();
        //    PaginatedList<Product> Product = _productRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
        //    IsProducts = Product.Select(ut => new SelectListItem()
        //    {
        //        Value = ut.Code.ToString(),
        //        Text = ut.Description,
        //    }).ToList();
        //    return IsProducts;
        //}
        private List<SelectListItem> GetSuppliers()
        {
            var IsSuppliers = new List<SelectListItem>();
            PaginatedList<Supplier> suppliers = _supplierRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            IsSuppliers = suppliers.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select Supplier---------"
            };
            IsSuppliers.Insert(0, deftItem);
            return IsSuppliers;
        }
        private List<SelectListItem> GetPoCurrencies()
        {
            var IsCurrencies = new List<SelectListItem>();
            PaginatedList<Currency> currencies = _currencyRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "AED", 1, 1000);
            IsCurrencies = currencies.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select Currency---------"
            };
            IsCurrencies.Insert(0, deftItem);
            return IsCurrencies;
        }
        private List<SelectListItem> GetBaseCurrencies()
        {
            var IsCurrencies = new List<SelectListItem>();
            PaginatedList<Currency> currencies = _currencyRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            IsCurrencies = currencies.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select Currency---------"
            };
            IsCurrencies.Insert(0, deftItem);
            return IsCurrencies;
        }
        private List<SelectListItem> GetExchangeRate()
        {
            var IsCurrencies = new List<SelectListItem>();
            PaginatedList<Currency> currencies = _currencyRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);
            IsCurrencies = currencies.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.ExchangeRate.ToString()
            }).ToList();
            return IsCurrencies;
        }
        //private List<SelectListItem> GetUnitNames()
        //{
        //    var IsProducts = new List<SelectListItem>();
        //    PaginatedList<Product> Product = _productRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
        //    IsProducts = Product.Select(ut => new SelectListItem()
        //    {
        //        Value = ut.Code.ToString(),
        //        Text = ut.Units .Name
        //    }).ToList();
        //    var deftItem = new SelectListItem()
        //    {
        //        Value = "",
        //        Text = "--------Select Units---------"
        //    };
        //    IsProducts.Insert(0, deftItem);
        //    return IsProducts;
        //}

    }
}

