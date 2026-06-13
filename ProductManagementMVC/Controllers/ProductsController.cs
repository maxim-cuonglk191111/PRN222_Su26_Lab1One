using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;

namespace ProductManagementMVC.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    private bool IsAuthenticated() => HttpContext.Session.GetString("UserId") != null;
    private bool IsAdmin() => HttpContext.Session.GetInt32("MemberRole") == 1;

    private IActionResult? RequireAuth()
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        return null;
    }

    public IActionResult Index(string? search)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        var products = string.IsNullOrWhiteSpace(search)
            ? _productService.GetAll()
            : _productService.Search(search);

        ViewBag.Search = search;
        ViewBag.IsAdmin = IsAdmin();
        ViewBag.Username = HttpContext.Session.GetString("Username");
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        var product = _productService.GetById(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryID", "CategoryName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        if (ModelState.IsValid)
        {
            _productService.Add(product);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryID", "CategoryName", product.CategoryID);
        return View(product);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        var product = _productService.GetById(id);
        if (product == null) return NotFound();

        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryID", "CategoryName", product.CategoryID);
        ViewBag.IsAdmin = IsAdmin();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Product product)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        if (id != product.ProductID) return BadRequest();

        if (ModelState.IsValid)
        {
            _productService.Update(product);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryID", "CategoryName", product.CategoryID);
        ViewBag.IsAdmin = IsAdmin();
        return View(product);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        var product = _productService.GetById(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        _productService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
