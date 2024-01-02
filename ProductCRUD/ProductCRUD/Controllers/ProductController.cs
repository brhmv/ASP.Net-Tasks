using Microsoft.AspNetCore.Mvc;
using ProductCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCRUD.Controllers
{
    public class ProductController : Controller
    {
        public static List<Product> Products = new()
        {
            new Product { Id = Guid.NewGuid().ToString(), Name = "Pen", Price = 10 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Book", Price = 30 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Ball", Price = 40 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Phone", Price = 50 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Computer", Price = 60 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Case", Price = 80 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Glue", Price = 90 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Paper", Price = 100 },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Eraser", Price = 110 }
        };


        #region GetProducts
        [HttpGet]
        public IActionResult GetProducts()
        {
            return View(Products);
        }
        #endregion

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CommitProduct(Product newProduct)
        {
            if (newProduct == null)
            {
                return BadRequest("Invalid product data.");
            }

            newProduct.Id = Guid.NewGuid().ToString();

            Products.Add(newProduct);

            return RedirectToAction("GetProducts");
        }


        #region Edit
        [HttpGet]
        public IActionResult EditProduct(string id)
        {
            var productToUpdate = Products.FirstOrDefault(p => p.Id == id);

            if (productToUpdate == null)
            {
                return NotFound();
            }
            return View(productToUpdate);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product updatedProduct)
        {
            var existingProduct = Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;
            }
            return RedirectToAction("GetProducts");
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult DeleteProduct(string id)
        {
            var productToDelete = Products.FirstOrDefault(p => p.Id == id);

            if (productToDelete != null)
            {
                Products.Remove(productToDelete);
            }

            return RedirectToAction("GetProducts");
        }
        #endregion

        #region GetProduct
        [HttpGet]
        public IActionResult GetProductViaName()
        {
            TempData["AllProducts"] = Products;

            return View();
        }

        [HttpPost]
        public IActionResult GetProductViaName(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            //var product = Products.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            var product = Products.FirstOrDefault(p => p.Name.ToLower().StartsWith(name.ToLower()));

            if (product == null)
            {
                ViewBag.ErrorMessage = "Product not found.";
            }

            TempData["AllProducts"] = Products;

            return View(product);
        }
        #endregion

    }
}