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
    public class ProductProfileRepo : IProductProfile
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        public ProductProfileRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public ProductProfile Create(ProductProfile productProfile)
        {
            _context.ProductProfiles.Add(productProfile);
            _context.SaveChanges();
            return productProfile;
        }

        public ProductProfile Delete(ProductProfile productProfile)
        {
            _context.ProductProfiles.Attach(productProfile);
            _context.Entry(productProfile).State = EntityState.Deleted;
            _context.SaveChanges();
            return productProfile;
        }

        public ProductProfile Edit(ProductProfile productProfile)
        {
            _context.ProductProfiles.Attach(productProfile);
            _context.Entry(productProfile).State = EntityState.Modified;
            _context.SaveChanges();
            return productProfile;
        }


        private List<ProductProfile> DoSort(List<ProductProfile> productProfiles, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    productProfiles = productProfiles.OrderBy(n => n.Name).ToList();
                else
                    productProfiles = productProfiles.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    productProfiles = productProfiles.OrderBy(d => d.Description).ToList();
                else
                    productProfiles = productProfiles.OrderByDescending(d => d.Description).ToList();
            }

            return productProfiles;
        }

        public PaginatedList<ProductProfile> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ProductProfile> productProfiles;

            if (SearchText != "" && SearchText != null)
            {
                productProfiles = _context.ProductProfiles.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                productProfiles = _context.ProductProfiles.ToList();

            productProfiles = DoSort(productProfiles, SortProperty, sortOrder);

            PaginatedList<ProductProfile> productProfile = new PaginatedList<ProductProfile>(productProfiles, pageIndex, pageSize);

            return productProfile;
        }

        public ProductProfile GetProductProfile(int id)
        {
            ProductProfile productProfiles = _context.ProductProfiles.Where(u => u.Id == id).FirstOrDefault();
            return productProfiles;
        }
        public bool IsProductProfileNameExists(string name)
        {
            int ct = _context.ProductProfiles.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsProductProfileNameExists(string name, int Id)
        {
            int ct = _context.ProductProfiles.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}
