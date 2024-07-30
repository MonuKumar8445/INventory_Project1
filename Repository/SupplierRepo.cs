using Microsoft.EntityFrameworkCore;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Areas.Identity.Data;
//using INventory_Project1.Migrations;
using INnventory_Project1.Models;

namespace INventory_Project1.Repository
{
    public class SupplierRepo : ISupplier
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        public SupplierRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public Supplier Create(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier;
        }

        public Supplier Delete(Supplier supplier)
        {
            _context.Suppliers.Attach(supplier);
            _context.Entry(supplier).State = EntityState.Deleted;
            _context.SaveChanges();
            return supplier;
        }

        public Supplier Edit(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            _context.Entry(supplier).State = EntityState.Modified;
            _context.SaveChanges();
            return supplier;
        }


        private List<Supplier> DoSort(List<Supplier> supplier, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    supplier = supplier.OrderBy(n => n.Name).ToList();
                else
                    supplier = supplier.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    supplier = supplier.OrderBy(d => d.Code).ToList();
                else
                    supplier = supplier.OrderByDescending(d => d.Code).ToList();
            }

            return supplier;
        }



        public Supplier GetItem(int Id)
        {
            Supplier supplier = _context.Suppliers.Where(u => u.Id == Id).FirstOrDefault();
            return supplier;
        }
        public bool IsSupplierNameExists(string name)
        {
            int ct = _context.Suppliers.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsSupplierNameExists(string name, int Id)
        {
            int ct = _context.Suppliers.Where(s => s.Name.ToLower() == name.ToLower() && s.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        public bool IsSupplierCodeExists(string code)
        {
            int ct = _context.Suppliers.Where(s => s.Code.ToLower() == code.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsSupplierCodeExists(string code, int Id)
        {
            int ct = _context.Suppliers.Where(s => s.Code.ToLower() == code.ToLower() && s.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        public bool IsSupplierEmailExists(string email)
        {
            int ct = _context.Suppliers.Where(n => n.EmailId.ToLower() == email.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsSupplierEmailExists(string email, int Id)
        {
            int ct = _context.Suppliers.Where(s => s.EmailId.ToLower() == email.ToLower() && s.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public PaginatedList<Supplier> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Supplier> supplier;

            if (SearchText != "" && SearchText != null)
            {
                supplier = _context.Suppliers.Where(n => n.Name.Contains(SearchText) || n.Code.Contains(SearchText))
                    .Include(u=>u.Code)
                    .ToList();
            }
            else
                supplier = _context.Suppliers.ToList();

            supplier = DoSort(supplier, SortProperty, sortOrder);

            PaginatedList<Supplier> suppliers = new PaginatedList<Supplier>(supplier, pageIndex, pageSize);

            return suppliers;
        }
    }
}
