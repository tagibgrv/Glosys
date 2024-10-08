using Glosys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Glosys.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GlosysContext _sql;

        public HomeController(ILogger<HomeController> logger, GlosysContext sql)
        {
            _logger = logger;
            _sql = sql;
        }

        public IActionResult Index()
        {
            ViewBag.Products = _sql.Products.Include(x => x.ProductPhotos).ToList();
            return View();
        }

        public IActionResult Products(int id)
        {
            var productList = _sql.Products.Include(x => x.ProductPhotos).AsQueryable();
            if (id != 0)
            {
                productList = productList.Where(x => x.ProductCategoryId == id);
            }
            ViewBag.CategoreList = _sql.Categories.ToList();
            return View(productList.ToList());
        }
        public IActionResult ProductInfo(int id)
        {
            Product product = _sql.Products.Include(x => x.ProductPhotos).FirstOrDefault(x => x.ProductId == id);
            return View(product);
        }
        public IActionResult Projects()
        {
            var projectList = _sql.Projects.Include(x => x.ProjectPhotos).AsQueryable();
            return View(projectList.ToList());
        }
        public IActionResult ProjectInfo(int id)
        {
            Project project = _sql.Projects.Include(x => x.ProjectPhotos).FirstOrDefault(x => x.ProjectId == id);
            return View(project);
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Gallery(int id)
        {
            var galeryList = _sql.Galeries.AsQueryable();
            if (id != 0)
            {
                galeryList = galeryList.Where(x => x.GaleryPhotoCategoryId == id);
            }
            ViewBag.GalleryCategory = _sql.GaleryCategories.ToList();
            return View(galeryList.ToList());
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult GetGalleryPhoto(int id)
        {
            var galleryPhoto = _sql.Galeries.SingleOrDefault(x => x.GaleryPhotoId == id);
            return Ok(galleryPhoto);
        }
        public IActionResult GetNextPhoto(int id)
        {
            var lastPhoto = _sql.Galeries.OrderBy(p => p.GaleryPhotoId).FirstOrDefault();
            var nextPhoto = _sql.Galeries
                .Where(p => p.GaleryPhotoId > id)
                .OrderBy(p => p.GaleryPhotoId)
                .FirstOrDefault();
            if (nextPhoto == null)
            {
                return Ok(lastPhoto);
            }
            return Ok(nextPhoto);
        }
        public IActionResult GetPrevPhoto(int id)
        {
            var firstPhoto = _sql.Galeries.OrderBy(p => p.GaleryPhotoId).LastOrDefault();
            var prevPhoto = _sql.Galeries
                .Where(p => p.GaleryPhotoId < id)
                .OrderBy(p => p.GaleryPhotoId)
                .LastOrDefault();
            if (prevPhoto == null)
            {
                return Ok(firstPhoto);
            }
            return Ok(prevPhoto);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
