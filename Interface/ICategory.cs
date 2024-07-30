using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface ICategory
    {
        PaginatedList<Category> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Category GetCategory(int id); // read particular item

        Category Create(Category category);

        Category Edit(Category category);

        Category Delete(Category category);
        //PaginatedList<Category> GetItems(string sortedProperty, Models.SortOrder sortedOrder, string searchText, int pg, int pageSize);

        public bool IsCategoryNameExists(string name);
        public bool IsCategoryNameExists(string name, int Id);


    }
}
