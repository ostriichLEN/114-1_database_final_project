using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _114_1_database_final_project.Models;

namespace _114_1_database_final_project.Controllers;

[Authorize] // 確保有登入才能看首頁
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Character1Context _context; 

 
    public HomeController(ILogger<HomeController> logger, Character1Context context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
       
        ViewBag.BandCount = _context.Bands.Count();
        ViewBag.VoiceActorCount = _context.VoiceActors.Count();
        ViewBag.CharacterCount = _context.Characters.Count();

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
}