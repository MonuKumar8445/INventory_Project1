using Microsoft.EntityFrameworkCore;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Areas.Identity.Data;
using System.Xml.Linq;

namespace INventory_Project1.Repository
{
    public class CurrencyRepo : ICurrency
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        private string _errors = "";

        public CurrencyRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }

        public bool Create(Currency currency)
        {
            try
            {
                if (!IsDescriptionValid(currency)) return false;

                if(IsItemExists(currency.Name)) return false;

                _context.Currencies.Add(currency);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors ="Sql Exception Occured , Error Info : "  + ex.Message;
                return false;
            }
        }

        public bool Delete(Currency currency)
        { 
            try
            {

         
               _context.Currencies.Remove(currency);
               _context.Entry(currency).State = EntityState.Deleted;
               _context.SaveChanges(true);
               return true;
            }
            catch (Exception ex)
            {
                if(ex.InnerException !=null)

                _errors = "Sql Exception Occured , Error Info : " + ex.Message;
                else
                    _errors = "Sql Exception Occured , Error Info : " + ex.InnerException.Message;

                return false;
            }

        }

        public bool Edit(Currency currency)
        {
            try
            {
                if (!IsDescriptionValid(currency)) return false;

                _context.Currencies.Add(currency);
                _context.Entry(currency).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }
        
        public Currency GetCurrency(int Id)
        {
           Currency currency = _context.Currencies.Where(c => c.Id == Id).FirstOrDefault();
            return currency;
        }

        public PaginatedList<Currency> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Currency> currencies;

            if (SearchText != "" && SearchText != null)
            {
                currencies = _context.Currencies.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                currencies = _context.Currencies.ToList();

            currencies = DoSort(currencies, SortProperty, sortOrder);

            PaginatedList<Currency> currencie = new PaginatedList<Currency>(currencies, pageIndex, pageSize);

            return currencie;
        }

        private List<Currency> DoSort(List<Currency> currencies, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    currencies = currencies.OrderBy(n => n.Name).ToList();
                else
                    currencies = currencies.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    currencies = currencies.OrderBy(d => d.Description).ToList();
                else
                    currencies = currencies.OrderByDescending(d => d.Description).ToList();
            }

            return currencies;
        }

        public bool IsCurrencyCombExists(int srcCurrencyId, int excCurrencyId)
        {
            int ct = _context.Currencies.Where(n => n.Id==srcCurrencyId && n.ExchangeCurrencyId != excCurrencyId).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int Id)
        {
            int ct = _context.Currencies.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.Currencies.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
            {
                    _errors = "Name" + name + " Exists Already";

                return true;
            }
            else 
                return false;
        }

        private bool IsDescriptionValid(Currency Item)
        {
            if (Item.Description.Length < 4 || Item.Description == null)
            {
                _errors = "Description Must Be atleast 4 Charectors";
                return false;
            }
            return true;

        }

    }
}
