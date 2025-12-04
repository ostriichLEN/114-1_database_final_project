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

        // ==========================================
        // 修改 1: Index 加入 Include 讓 View 可以計算成員數
        // ==========================================
        // GET: Bands
        public async Task<IActionResult> Index()
        {
            // 加入 Include(b => b.Characters) 以便在 View 中計算 Count
            var bands = _context.Bands.Include(b => b.Characters);
            return View(await bands.ToListAsync());
        }

        // ==========================================
        // 新增功能: 進階搜尋 (對應 Python 的 specific_band_frame)
        // ==========================================
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

            // 條件 1: 成立超過 N 年
            if (years != null)
            {
                query = query.Where(b => (currentYear - b.SinceYear) >= years);
            }

            // 條件 2: 成員至少 M 人
            if (minMembers != null)
            {
                query = query.Where(b => b.Characters.Count >= minMembers);
            }

            // 回傳 "Index" 視圖，但使用篩選後的資料
            return View("Index", await query.ToListAsync());
        }

        // ==========================================
        // 修改 2: Details 加入關聯資料，顯示樂團成員與聲優
        // ==========================================
        // GET: Bands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.Bands
                .Include(b => b.Characters)      // 載入樂團成員
                .ThenInclude(c => c.VoiceActor)  // 再載入成員對應的聲優
                .FirstOrDefaultAsync(m => m.BandId == id);

            if (band == null)
            {
                return NotFound();
            }

            return View(band);
        }

        // ==========================================
        // 以下為原本 Scaffolding 產生的 CRUD 功能 (保持不變)
        // ==========================================

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