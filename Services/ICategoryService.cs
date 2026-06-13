using BusinessObjects.Models;

namespace Services;

public interface ICategoryService
{
    List<Category> GetAll();
    Category? GetById(int id);
    void Add(Category category);
    void Update(Category category);
    void Delete(int id);
}
