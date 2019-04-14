using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcoloWeb.Models;
using EcoloWeb.Data;
using Microsoft.AspNetCore.Authorization;
using EcoloWeb.Data.Entity;
using EcoloWeb.Data.Entity.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcoloWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Intro()
        {
            return View();
        }

        [Authorize]
        public IActionResult Metodologia()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }



        [Authorize]
        public IActionResult Temas()
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

        ///Esto tiene que ver con la parte del Comprimiso mientras se ubica 

        public async Task<IActionResult> Compromiso()
        {


            //var Tienda = from m in _context.Compromisos.Include(a => a.Notas) select m;

            //if (!String.IsNullOrEmpty(Buscar))
            //{
            //    Tienda = Tienda.Where(s => s.Producto.Contains(Buscar));
            //}
            return View(await _context.Compromisos.ToListAsync());

            //return View(await Tienda.ToListAsync());


        }
      /*  public IActionResult Compromiso()
        {
            return View();
        }
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Notas")] Compromiso compromiso)
        {
          
            if (ModelState.IsValid)
            {
                _context.Add(compromiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
 
            return View(compromiso);
        }

    }
}
