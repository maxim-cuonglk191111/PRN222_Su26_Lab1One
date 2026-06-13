using BusinessObjects;
using BusinessObjects.Models;

namespace DataAccessObjects;

public class CategoryDAO
{
    private readonly MyStoreContext _context;

    public CategoryDAO(MyStoreContext context)
    {
        _context = context;
    }

    public List<Category> GetAll()
        => _context.Categories.OrderBy(c => c.CategoryName).ToList();

    public Category? GetById(int id)
        => _context.Categories.Find(id);

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
