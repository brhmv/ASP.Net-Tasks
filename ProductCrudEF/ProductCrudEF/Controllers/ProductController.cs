using Microsoft.AspNetCore.Mvc;
using ProductCrudEF.Models;
using ProductCrudEF.Data;
using ProductCrudEF.Models.ViewModels;
using ProductCrudEF.Helpers;
using AutoMapper;

namespace ProductCRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }


        #region GetProducts

        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = _appDbContext.Products.ToList();
            var categories = _appDbContext.Categories.ToList();

            return View(productList);
        }

        #endregion

        #region AddProduct
        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = _appDbContext.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CommitProduct(AddProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                if (product == null)
                {
                    return BadRequest("Invalid product data.");
                }

                var newProduct = _mapper.Map<Product>(product);
                newProduct.Id = Guid.NewGuid().ToString();

                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    string filePath = await UploadFileHelper.UploadFile(product.ImageFile);

                    newProduct.ImageUrl = Path.Combine(Path.GetFileName(filePath));
                }

                _appDbContext.Products.Add(newProduct);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("GetProducts");
            }
            else
            {
                return View(product);
            }
        }
        #endregion

        #region AddCategory
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CommitCategory(Category newCategory)
        {
            if (newCategory == null)
            {
                return BadRequest("Invalid Category data.");
            }

            newCategory.Id = Guid.NewGuid().ToString();


            _appDbContext.Categories.Add(newCategory);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("GetProducts");
        }


        #endregion

        #region Edit
        [HttpGet]
        public IActionResult EditProduct(string id)
        {
            var productToUpdate = _appDbContext.Products.FirstOrDefault(p => p.Id == id);
            var categories = _appDbContext.Categories.ToList();

            ViewBag.Categories = categories;


            if (productToUpdate == null)
            {
                return NotFound();
            }
            return View(productToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product updatedProduct)
        {
            var existingProduct = _appDbContext.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;
            }
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("GetProducts");
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult DeleteProduct(string id)
        {
            var productToDelete = _appDbContext.Products.FirstOrDefault(p => p.Id == id);

            if (productToDelete != null)
            {
                _appDbContext.Products.Remove(productToDelete);
            }

            _appDbContext.SaveChanges();
            return RedirectToAction("GetProducts");
        }
        #endregion

        #region GetProduct
        [HttpGet]
        public IActionResult GetProductViaName()
        {
            ViewBag.Products = _appDbContext.Products.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetProductViaName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            var product = _appDbContext.Products.FirstOrDefault(p => p.Name.ToLower().StartsWith(name.ToLower()));

            if (product == null)
            {
                ViewBag.ErrorMessage = "Product not found.";
            }

            ViewBag.Products = _appDbContext.Products;

            await _appDbContext.SaveChangesAsync();

            return View(product);
        }
        #endregion

    }
}