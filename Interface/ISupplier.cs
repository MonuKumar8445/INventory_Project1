using INnventory_Project1.Models;
using INventory_Project1.Models;
using SortOrder = INventory_Project1.Models.SortOrder;

namespace INventory_Project1.Interfaces
{
    public interface ISupplier
    {
        PaginatedList<Supplier> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Supplier GetItem(int Id); // read particular item

        Supplier Create(Supplier supplier);

        Supplier Edit(Supplier Supplier);

        Supplier Delete(Supplier Supplier);

        public bool IsSupplierNameExists(string name);
        public bool IsSupplierNameExists(string name, int Id);
        public bool IsSupplierCodeExists(string code);
        public bool IsSupplierCodeExists(string code, int Id);

        public bool IsSupplierEmailExists(string email);
        public bool IsSupplierEmailExists(string email, int Id);


    }
}
