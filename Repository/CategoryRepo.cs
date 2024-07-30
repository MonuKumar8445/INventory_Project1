using Microsoft.EntityFrameworkCore;
using INventory_Project1.Interfaces;
using INventory_Project1.Models;
using INventory_Project1.Areas.Identity.Data;

namespace INventory_Project1.Repository
{
    public class CategoryRepo : ICategory
    {
        private readonly ApplicationDbContext _context; // for connecting to efcore.
        private string _errors = "";
        public CategoryRepo(ApplicationDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public Category Create(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return category;
            }
            catch(Exception ex)
            {
                if(ex.InnerException !=null)
                    
                _errors = "Sql execption Occured, Error Info : " + ex.InnerException.Message;
                else
                    _errors = "Sql execption Occured, Error Info : " + ex.Message;

                return category;
            }
        }

        public Category Delete(Category category)
        {
            _context.Categories.Attach(category);
            _context.Entry(category).State = EntityState.Deleted;
            _context.SaveChanges();
            return category;
        }

        public Category Edit(Category category)
        {
            _context.Categories.Attach(category);
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return category;
        }


        private List<Category> DoSort(List<Category> categories, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(n => n.Name).ToList();
                else
                    categories = categories.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(d => d.Description).ToList();
                else
                    categories = categories.OrderByDescending(d => d.Description).ToList();
            }

            return categories;
        }

        public PaginatedList<Category> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Category> categories;

            if (SearchText != "" && SearchText != null)
            {
                categories = _context.Categories.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                categories = _context.Categories.ToList();

            categories = DoSort(categories, SortProperty, sortOrder);

            PaginatedList<Category> categorie = new PaginatedList<Category>(categories, pageIndex, pageSize);

            return categorie;
        }

        public Category GetCategory(int id)
        {
            Category categories = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            return categories;
        }
        public bool IsCategoryNameExists(string name)
        {
            int ct = _context.Categories.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsCategoryNameExists(string name, int Id)
        {
            int ct = _context.Categories.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


    }
}
