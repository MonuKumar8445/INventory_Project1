using INnventory_Project1.Models;
using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface IProduct
    {
        PaginatedList<Product> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Product GetItem(string Code); // read particular item

        Product Create(Product product);

        Product Edit(Product product);

        Product Delete(Product product);

        public bool IsExists(string name);
        public bool IsExists(string name, string Code);


    }
}
