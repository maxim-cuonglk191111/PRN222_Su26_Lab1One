using BusinessObjects.Models;

namespace Repositories;

public interface ICategoryRepository
{
    List<Category> GetAll();
    Category? GetById(int id);
    void Add(Category category);
    void Update(Category category);
    void Delete(int id);
}
