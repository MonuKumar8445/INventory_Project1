using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface IProductProfile
    {
        PaginatedList<ProductProfile> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        ProductProfile GetProductProfile(int id); // read particular item

        ProductProfile Create(ProductProfile productProfile);

        ProductProfile Edit(ProductProfile productProfile);

        ProductProfile Delete(ProductProfile productProfile);
        //PaginatedList<ProductProfile> GetItems(string sortedProperty, Models.SortOrder sortedOrder, string searchText, int pg, int pageSize);

        public bool IsProductProfileNameExists(string name);
        public bool IsProductProfileNameExists(string name, int Id);


    }
}
