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

    public async Task<IActionResult> Index(string? search)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        var products = string.IsNullOrWhiteSpace(search)
            ? await _productService.GetAllAsync()
            : await _productService.SearchAsync(search);

        ViewBag.Search = search;
        ViewBag.IsAdmin = IsAdmin();
        ViewBag.Username = HttpContext.Session.GetString("Username");
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryID", "CategoryName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        if (ModelState.IsValid)
        {
            await _productService.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
        ViewBag.IsAdmin = IsAdmin();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;

        if (id != product.ProductID) return BadRequest();

        if (ModelState.IsValid)
        {
            await _productService.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
        ViewBag.IsAdmin = IsAdmin();
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var redirect = RequireAuth();
        if (redirect != null) return redirect;
        if (!IsAdmin()) return Forbid();

        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
