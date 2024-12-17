using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGoldenFood.ApplicationDbContext;
using MyGoldenFood.Models;

namespace MyGoldenFood.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie", Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
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
                    string uploadsFolder = Path.Combine("wwwroot", "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    model.ImagePath = "/uploads/" + uniqueFileName;
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
        public async Task<IActionResult> Edit(Product model, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products.FindAsync(model.Id);
                if (existingProduct == null) return NotFound();

                existingProduct.Name = model.Name;
                existingProduct.Description = model.Description;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine("wwwroot", "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    existingProduct.ImagePath = "/uploads/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Ürün başarıyla güncellendi!" });
            }
            return PartialView("_EditProductPartial", model);
        }



        // Ürün Silme
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı!" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Ürün başarıyla silindi!" });
        }

    }
}
