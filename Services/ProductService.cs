using BusinessObjects.Models;
using Repositories;

namespace Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public List<Product> GetAll() => _repo.GetAll();
    public Product? GetById(int id) => _repo.GetById(id);
    public void Add(Product product) => _repo.Add(product);
    public void Update(Product product) => _repo.Update(product);
    public void Delete(int id) => _repo.Delete(id);
    public List<Product> Search(string keyword) => _repo.Search(keyword);
}
