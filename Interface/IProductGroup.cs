using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface IProductGroup
    {
        PaginatedList<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        ProductGroup GetProductGroup(int id); // read particular item

        ProductGroup Create(ProductGroup productGroup);

        ProductGroup Edit(ProductGroup productGroup);

        ProductGroup Delete(ProductGroup productGroup);
        //PaginatedList<ProductGroup> GetItems(string sortedProperty, Models.SortOrder sortedOrder, string searchText, int pg, int pageSize);

        public bool IsProductGroupNameExists(string name);
        public bool IsProductGroupNameExists(string name, int Id);


    }
}
