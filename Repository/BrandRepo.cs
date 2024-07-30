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
    public class BrandRepo : IBrand
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        public BrandRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public Brand Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return brand;
        }

        public Brand Delete(Brand brand)
        {
            _context.Brands.Attach(brand);
            _context.Entry(brand).State = EntityState.Deleted;
            _context.SaveChanges();
            return brand;
        }

        public Brand Edit(Brand brand)
        {
            _context.Brands.Attach(brand);
            _context.Entry(brand).State = EntityState.Modified;
            _context.SaveChanges();
            return brand;
        }


        private List<Brand> DoSort(List<Brand> Brands, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    Brands = Brands.OrderBy(n => n.Name).ToList();
                else
                    Brands = Brands.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    Brands = Brands.OrderBy(d => d.Description).ToList();
                else
                    Brands = Brands.OrderByDescending(d => d.Description).ToList();
            }

            return Brands;
        }

        public PaginatedList<Brand> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Brand> Brands;

            if (SearchText != "" && SearchText != null)
            {
                Brands = _context.Brands.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                Brands = _context.Brands.ToList();

            Brands = DoSort(Brands, SortProperty, sortOrder);

            PaginatedList<Brand> retBrands = new PaginatedList<Brand>(Brands, pageIndex, pageSize);

            return retBrands;
        }

        public Brand GetBrand(int id)
        {
            Brand Brand = _context.Brands.Where(u => u.Id == id).FirstOrDefault();
            return Brand;
        }
        public bool IsBrandNameExists(string name)
        {
            int ct = _context.Brands.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsBrandNameExists(string name, int Id)
        {
            int ct = _context.Brands.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}

