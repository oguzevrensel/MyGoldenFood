using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGoldenFood.ApplicationDbContext;
using MyGoldenFood.Models;
using MyGoldenFood.Services;

namespace MyGoldenFood.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie", Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public ProductController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // Ürün Listeleme
        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var products = await _context.Products.ToListAsync();
            return PartialView("_ProductListPartial", products);
        }

        // Yeni Ürün Ekle - GET
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateProductPartial");
        }

        // Yeni Ürün Ekle - POST
        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadResult = await _cloudinaryService.UploadImageAsync(ImageFile, "products");
                    if (uploadResult != null)
                    {
                        model.ImagePath = uploadResult;
                    }
                }

                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Ürün başarıyla eklendi!" });
            }

            return PartialView("_CreateProductPartial", model);
        }

        // Ürün Düzenle - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return PartialView("_EditProductPartial", product);
        }

        // Ürün Düzenle - POST
        [HttpPost]
        public async Task<IActionResult> Edit(Product model, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products.FindAsync(model.Id);
                if (existingProduct == null) return NotFound();

                existingProduct.Name = model.Name;
                existingProduct.Description = model.Description;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Yeni resim yüklendiğinde eski resim silinir
                    await _cloudinaryService.DeleteImageAsync(existingProduct.ImagePath);
                    var uploadResult = await _cloudinaryService.UploadImageAsync(ImageFile, "products");
                    if (uploadResult != null)
                    {
                        existingProduct.ImagePath = uploadResult;
                    }
                }

                // Resim değişikliği yapılmadığında mevcut resim yolu korunur
                else if (string.IsNullOrEmpty(existingProduct.ImagePath) && !string.IsNullOrEmpty(model.ImagePath))
                {
                    existingProduct.ImagePath = model.ImagePath;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Ürün başarıyla güncellendi!" });
            }

            return PartialView("_EditProductPartial", model);
        }





        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı!" });
            }

            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                await _cloudinaryService.DeleteImageAsync(product.ImagePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Ürün başarıyla silindi!" });
        }
    }
}
