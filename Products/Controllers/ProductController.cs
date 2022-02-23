using Microsoft.AspNetCore.Mvc;
using Products.Data;
using Products.Models;

namespace Products.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public String Create(IFormCollection collection)
    {
        var exists = _db.Products.FirstOrDefault(product => product.Name == "ali");
        if (exists == null)
        {
            _db.Products.Add(new Product("ali"));
            _db.SaveChanges();
        }

        var product = _db.Products.FirstOrDefault();
        return product != null ? product.Name : "not found";
    }
}