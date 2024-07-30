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
    public class ProductController : Controller
    {

        private readonly IProduct _productRepo;
        private IBrand _brandRepo;
        private IUnit _unitRepo;
        private IProductGroup _productGroupRepo;
        private IProductProfile _productProfileRepo;
        private ICategory _categoryRepo;
        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public ProductController(IProduct productRepo, IUnit unitRepo,IBrand brandRepo,IProductGroup productGroupRepo,IProductProfile productProfileRepo,
            ICategory categoryRepo, IWebHostEnvironment hostingEnvironment) // here the repository will be passed by the dependency injection.
        {
            _productRepo = productRepo;
            _unitRepo = unitRepo;
            _productGroupRepo = productGroupRepo;
            _productProfileRepo = productProfileRepo;
            _categoryRepo = categoryRepo;
            _IWebHostEnvironment = hostingEnvironment;
            _brandRepo = brandRepo;
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("code");
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.AddColumn("cost");
            sortModel.AddColumn("price");
            sortModel.AddColumn("units");

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Product> products = _productRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(products.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;
           


            return View(products);
        }

        public IActionResult Create()
        {
            Product product = new Product();
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategorys();
            ViewBag.ProductGroups = GetProductGroups();
            ViewBag.ProductProfiles = GetProductProfiles();
            return View(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";

                if (_productRepo.IsExists(product.Name) == true)
                    errMessage = errMessage + " " + " Product Name " + product.Name + " Exists Already";

                if (errMessage == "")
                {


                    string uniqueFileName = GetUploadedFileName(product); //Image Uploat in database 
                    product.PhotoUrl = uniqueFileName;


                    product = _productRepo.Create(product);
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
                return View(product);
            }
            else
            {
                TempData["SuccessMessage"] = "Product " + product.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(string Code) //Read
        {
            Product product = _productRepo.GetItem(Code);
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategorys();
            ViewBag.ProductGroups = GetProductGroups();
            ViewBag.ProductProfiles = GetProductProfiles();
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            //Product product =_productRepo.GetItem.in

            Product product = _productRepo.GetItem(id);
            if(product is null)
            {
                return View(new Product());
            }
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategorys();
            ViewBag.ProductGroups = GetProductGroups();
            ViewBag.ProductProfiles = GetProductProfiles();
            TempData.Keep();
            TempData["SuccessMessage"] = product.Name + ", Product Saved Successfully";

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";

                if (_productRepo.IsExists(product.Name, product.Code) == true)
                    errMessage = errMessage + "Product Name " + product.Name + " Already Exists";
                if (product.ProductPhoto != null)
                {
                    string uniqueFileName = GetUploadedFileName(product); //Image Uploat in database 
                    product.PhotoUrl = uniqueFileName;
                }
                if (errMessage == "")
                {
                    product = _productRepo.Edit(product);
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
                return View(product);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [HttpGet]
        public IActionResult Delete(string Code)
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                product = _productRepo.Delete(product);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "ProductGroup " + product.Name + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }

        private List<SelectListItem> GetUnits()
        {
            var IsUnits = new List<SelectListItem>();
            PaginatedList<Unit> units = _unitRepo.GetItems("Name",sortOrder: SortOrder.Ascending,"",1,1000);
            IsUnits = units.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select Units---------"
            };
            IsUnits.Insert(0, deftItem);
            return IsUnits;
        }

        private List<SelectListItem> GetBrands()
        {
            var IsBrands = new List<SelectListItem>();
            PaginatedList<Brand> brands = _brandRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            IsBrands = brands.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select Brands---------"
            };
            IsBrands.Insert(0, deftItem);
            return IsBrands;
        }

        private List<SelectListItem> GetCategorys()
        {
            var IsCategorys = new List<SelectListItem>();
            PaginatedList<Category> categories = _categoryRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            IsCategorys = categories.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select Category---------"
            };
            IsCategorys.Insert(0, deftItem);
            return IsCategorys;
        }

        private List<SelectListItem> GetProductGroups()
        {
            var IsProductGroups = new List<SelectListItem>();
            PaginatedList<ProductGroup> productGroups = _productGroupRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            IsProductGroups     = productGroups.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select ProductGroup---------"
            };
            IsProductGroups.Insert(0, deftItem);
            return IsProductGroups;
        }

        private List<SelectListItem> GetProductProfiles()
        {
            var IsProductProfiles = new List<SelectListItem>();
            PaginatedList<ProductProfile> productProfiles = _productProfileRepo.GetItems("Name", sortOrder: SortOrder.Ascending, "", 1, 1000);
            IsProductProfiles = productProfiles.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name,
            }).ToList();
            var deftItem = new SelectListItem()
            {
                Value = "",
                Text = "--------Select ProductProfile---------"
            };
            IsProductProfiles.Insert(0, deftItem);
            return IsProductProfiles;
        }

        //Images Upload Method
        private string GetUploadedFileName(Product product)
        {
            string uniqueFileName = string.Empty ;
            if (product.ProductPhoto != null)
            {
                string uploadsFolder = Path.Combine(_IWebHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ProductPhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductPhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}

