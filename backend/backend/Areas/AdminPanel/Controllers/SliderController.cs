using backend.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Helpers;

namespace backend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Read
        public IActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }
        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            Slider slider = _context.Sliders.Find(id);
            if(slider == null)
            {
                return NotFound();
            }
            var path = Helper.GetPath(_env.WebRootPath, "images",slider.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Create (GET)
        public IActionResult Create()
        {
            return View();
        }
        //Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slider.Photo.CheckFileSize(500))
            {
                ModelState.AddModelError("Photo", "File size must be less than 500 KB");
                return View();
            }
            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type must be image");
                return View();
            }
            slider.Url = await slider.Photo.SaveFileAsync(_env.WebRootPath, "images");
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        //Update (GET)
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider sliderDb = _context.Sliders.Find(id);
            if (sliderDb == null)
            {
                return NotFound();
            }
            return View(sliderDb);
        }
        //Update (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Slider slider)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider sliderDb = _context.Sliders.Find(id);
            if (sliderDb == null)
            {
                return NotFound();
            }
            slider.Url = await slider.Photo.SaveFileAsync(_env.WebRootPath, "images");
            var pathDb = Helper.GetPath(_env.WebRootPath, "images", sliderDb.Url);
            if (System.IO.File.Exists(pathDb))
            {
                System.IO.File.Delete(pathDb);
            }
            sliderDb.Url = slider.Url;
            sliderDb.Title = slider.Title;
            sliderDb.Description = slider.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
