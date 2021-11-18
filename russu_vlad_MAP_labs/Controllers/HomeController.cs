using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LibraryModel.Data;
using LibraryModel.Models;
using LibraryModel.Models.LibraryViewModels;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace russu_vlad_MAP_labs.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;

        public HomeController(LibraryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
                from order in _context.Orders
                group order by order.OrderDate into dateGroup
                select new OrderGroup()
                {
                    OrderDate = dateGroup.Key,
                    BookCount = dateGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}
