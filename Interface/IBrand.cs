using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface IBrand
    {
        PaginatedList<Brand> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Brand GetBrand(int id); // read particular item

        Brand Create(Brand brand);
          

        Brand Edit(Brand brand);

        Brand Delete(Brand brand);
        //PaginatedList<Brand> GetItems(string sortedProperty, Models.SortOrder sortedOrder, string searchText, int pg, int pageSize);

        public bool IsBrandNameExists(string name);
        public bool IsBrandNameExists(string name, int Id);


    }
}
