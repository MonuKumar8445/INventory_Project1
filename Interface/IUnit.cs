using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface IUnit
    {
        PaginatedList<Unit> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Unit GetUnit(int id); // read particular item

        Unit Create(Unit unit);

        Unit Edit(Unit unit);

        Unit Delete(Unit unit);
        //PaginatedList<Unit> GetItems(string sortedProperty, Models.SortOrder sortedOrder, string searchText, int pg, int pageSize);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, int Id);


    }
}
