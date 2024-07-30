using INventory_Project1.Areas.Identity.Data;
using INventory_Project1.Interface;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;



namespace INventory_Project1.Repository
{
    public class PurchaseOrderRepo : IPurchaseOrder
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        public PurchaseOrderRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public PoHeader Create(PoHeader poHeader)
        {

            _context.PoHeaders.Add(poHeader);
            _context.SaveChanges();
            return poHeader;
        }

        public PoHeader Delete(PoHeader poHeader)
        {
            _context.PoHeaders.Attach(poHeader);
            _context.Entry(poHeader).State = EntityState.Deleted;
            _context.SaveChanges();
            return poHeader;
        }

        public PoHeader Edit(PoHeader poHeader)
        {
            _context.PoHeaders.Update(poHeader);
            _context.Entry(poHeader).State = EntityState.Modified;
            _context.SaveChanges();
            return poHeader;
        }


        private List<PoHeader> DoSort(List<PoHeader> poHeaders, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "PoNumber")
            {
                if (sortOrder == SortOrder.Ascending)
                    poHeaders = poHeaders.OrderBy(n => n.PoNumber).ToList();
                else
                    poHeaders = poHeaders.OrderByDescending(n => n.PoNumber).ToList();
            }
            else if (SortProperty.ToLower() == "podate")
            {
                if (sortOrder == SortOrder.Ascending)
                    poHeaders = poHeaders.OrderBy(n => n.PoDate).ToList();
                else
                    poHeaders = poHeaders.OrderByDescending(n => n.PoDate).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    poHeaders = poHeaders.OrderBy(d => d.Id).ToList();
                else
                    poHeaders = poHeaders.OrderByDescending(d => d.Id)

                    .ToList();
            }

            return poHeaders;
        }



        public PoHeader GetItem(int Id)
        {
            PoHeader poHeader = _context.PoHeaders.Where(u => u.Id == Id)
                .Include(d => d.PoDetails)
                .FirstOrDefault();
            return poHeader;
        }
        public bool IsPoNumberExists(string poNumber)
        {
            int ct = _context.PoHeaders.Where(n => n.PoNumber.ToLower() == poNumber.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsPoNumberExists(string poNumber, int Id)
        {
            int ct = _context.PoHeaders.Where(n => n.PoNumber.ToLower() == poNumber.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        PaginatedList<PoHeader> IPurchaseOrder.GetItems(string SortProperty, SortOrder sortOrder, string SearchText, int pageIndex, int pageSize)
        {
            List<PoHeader> poHeaders;

            if (SearchText != "" && SearchText != null)
            {
                poHeaders = _context.PoHeaders.Where(n => n.PoNumber.Contains(SearchText) || n.QuotationNo.Contains(SearchText))
                    .Include(u => u.BaseCurrencyId)
                    .Include(s => s.SupplierId)
                    .Include(p => p.PoCurrencyId)
                    .ToList();
            }
            else
                poHeaders = _context.PoHeaders.ToList();

            poHeaders = DoSort(poHeaders, SortProperty, sortOrder);

            PaginatedList<PoHeader> poHeaders1 = new PaginatedList<PoHeader>(poHeaders, pageIndex, pageSize);

            return poHeaders1;
        }
        public bool IsQuotaionNoExists(string quoNumber)
        {
            int ct = _context.PoHeaders.Where(q => q.QuotationNo.ToLower() == quoNumber.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsQuotaionNoExists(string quoNumber, int Id)
        {
            int ct = _context.PoHeaders.Where(q => q.QuotationNo == quoNumber.ToLower() && q.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public string GetNewPoNumber()
        {
            string ponumber = "";
            var LastPoNumber = _context.PoHeaders.Max(cd => cd.PoNumber);
                if(LastPoNumber == null)
                ponumber = "PO0001";
                else
                {
                  int lastdigit = 1;
                  int.TryParse(LastPoNumber.Substring(2, 5).ToString(), out lastdigit);
                  ponumber = "PO" + (lastdigit + 1).ToString().PadLeft(5, '0');
                }
                return ponumber;
        }
    }

}

