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
    public class BandsController : Controller
    {
        private readonly Character1Context _context;

        public BandsController(Character1Context context)
        {
            _context = context;
        }


        // GET: Bands
        public async Task<IActionResult> Index(string searchString)
        {
            
            var bands = _context.Bands
            .Include(b => b.Characters) // 成員數
            .Where(b => b.BandId != 0)  // 不顯示隱形樂團
            .AsQueryable();

            // 篩選 "樂團名稱"
            if (!string.IsNullOrEmpty(searchString))
            {
                bands = bands.Where(b => b.BandName.Contains(searchString));
            }

            // 把搜尋字串存起來傳回 View
            ViewData["CurrentFilter"] = searchString;

            return View(await bands.ToListAsync());
        }

  
        // 進階搜尋
        // GET: Bands/AdvancedSearch
        public async Task<IActionResult> AdvancedSearch(int? years, int? minMembers)
        {
            // 如果沒有輸入任何條件，直接回傳原本的 Index 頁面
            if (years == null && minMembers == null)
            {
                return RedirectToAction(nameof(Index));
            }

            int currentYear = DateTime.Now.Year;

            // 建立查詢，並預先載入 Characters 以便計算人數
            var query = _context.Bands
                .Include(b => b.Characters)
                .AsQueryable();

            // 成立超過 N 年
            if (years != null)
            {
                query = query.Where(b => (currentYear - b.SinceYear) >= years);
            }

            // 成員至少 M 人
            if (minMembers != null)
            {
                query = query.Where(b => b.Characters.Count >= minMembers);
            }

           
            return View("Index", await query.ToListAsync());
        }


        // 顯示樂團成員與樂手
        // GET: Bands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.Bands
                .Include(b => b.Characters)      // 載入樂團成員
                .ThenInclude(c => c.VoiceActor)  
                .FirstOrDefaultAsync(m => m.BandId == id);

            if (band == null)
            {
                return NotFound();
            }

            return View(band);
        }

   
        // GET: Bands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BandId,BandName,SinceYear")] Band band)
        {
            // 檢查編號是否重複
            if (_context.Bands.Any(x => x.BandId == band.BandId))
            {
                // 設定錯誤訊息給 View 
                ViewBag.AlertMessage = "輸入的樂團編號已存在";
                return View(band);
            }

            if (ModelState.IsValid)
            {
                _context.Add(band);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(band);
        }

        // GET: Bands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.Bands.FindAsync(id);
            if (band == null)
            {
                return NotFound();
            }
            return View(band);
        }

        // POST: Bands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BandId,BandName,SinceYear")] Band band)
        {
            if (id != band.BandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(band);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandExists(band.BandId))
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
            return View(band);
        }

        // GET: Bands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.Bands
                .FirstOrDefaultAsync(m => m.BandId == id);
            if (band == null)
            {
                return NotFound();
            }

            return View(band);
        }

        // POST: Bands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var band = await _context.Bands.FindAsync(id);
            if (band != null)
            {
                _context.Bands.Remove(band);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BandExists(int id)
        {
            return _context.Bands.Any(e => e.BandId == id);
        }
    }
}