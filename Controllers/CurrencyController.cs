using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace INventory_Project1.Controllers
{
    [Authorize]
    public class CurrencyController : Controller
    {

        private ICurrency _currencyRepo;

        public CurrencyController(ICurrency currenyrepo) // here the repository will be passed by the dependency injection.
        {

            _currencyRepo = currenyrepo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Currency> currencies = _currencyRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(currencies.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(currencies);
        }


        public IActionResult Create()
        {
            Currency currency = new Currency();
            ViewBag.ExchangeCurrencyId = GetCurrency();
            return View(currency);
        }

        [HttpPost]
        public IActionResult Create(Currency currency)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {

                bolret = _currencyRepo.Create(currency);
                
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
               // errMessage = errMessage + " " + _currencyRepo.GetCurrency();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }
            else
            {
                TempData["SuccessMessage"] = "Currency " + currency.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            Currency currency = _currencyRepo.GetCurrency(id);
            ViewBag.ExchangeCurrencyId = GetCurrency();
            return View(currency);
        }


        public IActionResult Edit(int id)
        {
            Currency currency = _currencyRepo.GetCurrency(id);
            ViewBag.ExchangeCurrencyId = GetCurrency();
            TempData.Keep();
            return View(currency);
        }

        [HttpPost]
        public IActionResult Edit(Currency currency)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                bolret = _currencyRepo.Edit(currency);
         
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
                return View(currency);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            Currency currency = _currencyRepo.GetCurrency(id);
            ViewBag.ExchangeCurrencyId = GetCurrency();
            TempData.Keep();
            return View(currency);
        }


        [HttpPost]
        public IActionResult Delete(Currency currency)
        {
            string errMessage = "";

            bool bolret = false;
            try
            {
                bolret = _currencyRepo.Delete(currency);
            }
            catch (Exception ex)
            {
                 errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }
            else
            {

                TempData["SuccessMessage"] = "Currency " + currency.Name + " Deleted Successfully";
                return RedirectToAction(nameof(Index), new { pg = currentPage });
            } 
        }
        private List<SelectListItem> GetCurrency()
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
                Text = "--------Select Currencies---------"
            };
            IsCurrencies.Insert(0, deftItem);
            return IsCurrencies;
        }

    }
}

