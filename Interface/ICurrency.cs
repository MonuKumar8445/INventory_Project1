using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface ICurrency
    {
        PaginatedList<Currency> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Currency GetCurrency(int Id); // read particular item

        bool Create(Currency currency);

        bool Edit(Currency currency);

        bool Delete(Currency currency);

        public bool IsItemExists(string name);
        public bool IsItemExists(string name , int Id);
        //public bool IsCurrencyNameExists(string name, int Id);
        public bool IsCurrencyCombExists(int srcCurrencyId , int excCurrencyId);
       // string GetCurrency();
    }
}
