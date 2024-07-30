using Microsoft.EntityFrameworkCore;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Areas.Identity.Data;
//using INventory_Project1.Migrations;
using INnventory_Project1.Models;

namespace INventory_Project1.Repository
{
    public class ProductRepo : IProduct
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        public ProductRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Delete(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Deleted;
            _context.SaveChanges();
            return product;
        }

        public Product Edit(Product product)
        {
            _context.Products.Update(product);
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return product;
        }


        private List<Product> DoSort(List<Product> products, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    products = products.OrderBy(n => n.Name).ToList();
                else
                    products = products.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    products = products.OrderBy(d => d.Description).ToList();
                else
                    products = products.OrderByDescending(d => d.Description).ToList();
            }

            return products;
        }



        public Product GetItem(string Code)
        {
            Product products = _context.Products.Where(u => u.Code == Code).FirstOrDefault();
            return products;
        }
        public bool IsExists(string name)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsExists(string name, string Code)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower() && n.Code != Code).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public PaginatedList<Product> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Product> products;

            if (SearchText != "" && SearchText != null)
            {
                products = _context.Products.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .Include(u=>u.Units)
                    .ToList();
            }
            else
                products = _context.Products.ToList();

            products = DoSort(products, SortProperty, sortOrder);

            PaginatedList<Product> product = new PaginatedList<Product>(products, pageIndex, pageSize);

            return product;
        }
    }
}
