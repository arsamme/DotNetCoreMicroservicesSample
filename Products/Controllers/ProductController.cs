using Products.Data;
using Products.Models;

namespace Products.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private ApplicationDbContext db;

    public ProductController(ApplicationDbContext db)
    {
        this.db = db;
    }

    [HttpPost]
    public String Create(IFormCollection collection)
    {
        var exists = db.Products.FirstOrDefault(product => product.Name == "ali");
        if (exists == null)
        {
            db.Products.Add(new Product("ali"));
            db.SaveChanges();
        }

        var product = db.Products.FirstOrDefault();
        return product != null ? product.Name : "not found";
    }
}