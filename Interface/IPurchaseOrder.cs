using INventory_Project1.Models;

namespace INventory_Project1.Interface
{
    public interface IPurchaseOrder
    {
        PaginatedList<PoHeader> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        PoHeader GetItem(int Id); // read particular item

        PoHeader Create(PoHeader poHeader);

        PoHeader Edit(PoHeader poHeader);

        PoHeader Delete(PoHeader poHeader);

        public bool IsPoNumberExists(string poNumber);
        public bool IsPoNumberExists(string poNumber, int Id);

        public bool IsQuotaionNoExists(string quoNumber);
        public bool IsQuotaionNoExists(string quoNumber, int Id);
        public string GetNewPoNumber();
    }
}
