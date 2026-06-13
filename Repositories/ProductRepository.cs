using BusinessObjects.Models;
using DataAccessObjects;

namespace Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDAO _dao;

    public ProductRepository(ProductDAO dao)
    {
        _dao = dao;
    }

    public List<Product> GetAll() => _dao.GetAll();
    public Product? GetById(int id) => _dao.GetById(id);
    public void Add(Product product) => _dao.Add(product);
    public void Update(Product product) => _dao.Update(product);
    public void Delete(int id) => _dao.Delete(id);
    public List<Product> Search(string keyword) => _dao.Search(keyword);
}
