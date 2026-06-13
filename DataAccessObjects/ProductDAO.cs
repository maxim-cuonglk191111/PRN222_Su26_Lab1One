using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects;

public class ProductDAO
{
    private readonly MyStoreContext _context;

    public ProductDAO(MyStoreContext context)
    {
        _context = context;
    }

    public List<Product> GetAll()
        => _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.ProductName)
            .ToList();

    public Product? GetById(int id)
        => _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductID == id);

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    public List<Product> Search(string keyword)
        => _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.ProductName.Contains(keyword))
            .OrderBy(p => p.ProductName)
            .ToList();
}
