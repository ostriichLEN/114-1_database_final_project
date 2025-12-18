using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _114_1_database_final_project.Models;
using Microsoft.AspNetCore.Authorization;

namespace _114_1_database_final_project.Controllers
{
    [Authorize]
    public class VoiceActorsController : Controller
    {
        private readonly Character1Context _context;

        public VoiceActorsController(Character1Context context)
        {
            _context = context;
        }


        // GET: VoiceActors
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.VoiceActors.Where(v => v.VoiceActorId != 0).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(v => v.FirstName.Contains(searchString)
                                      || v.LastName.Contains(searchString)
                                      || v.Subsidiary.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await query.ToListAsync());
        }


        // GET: VoiceActors/SearchByAge
        public async Task<IActionResult> SearchByAge(int? age)
        {
            // 建立查詢
            var query = _context.VoiceActors.AsQueryable();


            if (age != null)
            {
            
                // 把 DateTime 轉成 DateOnly
                var dateLimit = DateOnly.FromDateTime(DateTime.Today.AddYears(-age.Value));

                query = query.Where(v => v.BirthDate.HasValue && v.BirthDate < dateLimit);

               
                ViewData["AgeFilter"] = age;
            }

            // 回傳 Index 視圖顯示結果
            return View("Index", await query.ToListAsync());
        }

       
        // GET: VoiceActors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiceActor = await _context.VoiceActors
               
                .Include(v => v.Characters)

                .ThenInclude(c => c.Band)
                .FirstOrDefaultAsync(m => m.VoiceActorId == id);

            if (voiceActor == null)
            {
                return NotFound();
            }

            return View(voiceActor);
        }

     

        // GET: VoiceActors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VoiceActors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoiceActorId,FirstName,LastName,BirthDate,Subsidiary")] VoiceActor voiceActor)
        {
            
            if (_context.VoiceActors.Any(x => x.VoiceActorId == voiceActor.VoiceActorId))
            {
                ViewBag.AlertMessage = "輸入的藝人編號已存在";
                return View(voiceActor);
            }

            if (ModelState.IsValid)
            {
                _context.Add(voiceActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voiceActor);
        }

        // GET: VoiceActors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiceActor = await _context.VoiceActors.FindAsync(id);
            if (voiceActor == null)
            {
                return NotFound();
            }
            return View(voiceActor);
        }

        // POST: VoiceActors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoiceActorId,FirstName,LastName,BirthDate,Subsidiary")] VoiceActor voiceActor)
        {
            if (id != voiceActor.VoiceActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voiceActor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoiceActorExists(voiceActor.VoiceActorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voiceActor);
        }

        // GET: VoiceActors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiceActor = await _context.VoiceActors
                .FirstOrDefaultAsync(m => m.VoiceActorId == id);
            if (voiceActor == null)
            {
                return NotFound();
            }

            return View(voiceActor);
        }

        // POST: VoiceActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voiceActor = await _context.VoiceActors.FindAsync(id);
            if (voiceActor != null)
            {
                _context.VoiceActors.Remove(voiceActor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoiceActorExists(int id)
        {
            return _context.VoiceActors.Any(e => e.VoiceActorId == id);
        }
    }
}