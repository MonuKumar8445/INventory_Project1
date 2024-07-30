using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Areas.Identity.Data;

namespace INventory_Project1.Repository
{
    public class ProductGroupRepo : IProductGroup
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        public ProductGroupRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public ProductGroup Create(ProductGroup productGroup)
        {
            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
            return productGroup;
        }

        public ProductGroup Delete(ProductGroup productGroup)
        {
            _context.ProductGroups.Attach(productGroup);
            _context.Entry(productGroup).State = EntityState.Deleted;
            _context.SaveChanges();
            return productGroup;
        }

        public ProductGroup Edit(ProductGroup productGroup)
        {
            _context.ProductGroups.Attach(productGroup);
            _context.Entry(productGroup).State = EntityState.Modified;
            _context.SaveChanges();
            return productGroup;
        }


        private List<ProductGroup> DoSort(List<ProductGroup> productGroups, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    productGroups = productGroups.OrderBy(n => n.Name).ToList();
                else
                    productGroups = productGroups.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    productGroups = productGroups.OrderBy(d => d.Description).ToList();
                else
                    productGroups = productGroups.OrderByDescending(d => d.Description).ToList();
            }

            return productGroups;
        }

        public PaginatedList<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ProductGroup> productGroups;

            if (SearchText != "" && SearchText != null)
            {
                productGroups = _context.ProductGroups.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                productGroups = _context.ProductGroups.ToList();

            productGroups = DoSort(productGroups, SortProperty, sortOrder);

            PaginatedList<ProductGroup> productGroups1 = new PaginatedList<ProductGroup>(productGroups, pageIndex, pageSize);

            return productGroups1;
        }

        public ProductGroup GetProductGroup(int id)
        {
            ProductGroup productGroup = _context.ProductGroups.Where(u => u.Id == id).FirstOrDefault();
            return productGroup;
        }
        public bool IsProductGroupNameExists(string name)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsProductGroupNameExists(string name, int Id)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}
