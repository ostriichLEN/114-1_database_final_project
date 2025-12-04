using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _114_1_database_final_project.Models;

namespace _114_1_database_final_project.Controllers
{
    public class VoiceActorsController : Controller
    {
        private readonly Character1Context _context;

        public VoiceActorsController(Character1Context context)
        {
            _context = context;
        }

        // ==========================================
        // 修改 1: Index 維持不變
        // ==========================================
        // GET: VoiceActors
        public async Task<IActionResult> Index()
        {
            return View(await _context.VoiceActors.ToListAsync());
        }

        // ==========================================
        // 新增功能: 依年齡搜尋 (對應 Python 的 query_age_frame)
        // ==========================================
        // GET: VoiceActors/SearchByAge
        public async Task<IActionResult> SearchByAge(int? age)
        {
            // 建立查詢
            var query = _context.VoiceActors.AsQueryable();

            // 如果有輸入年齡 (例如輸入 20，代表要找 > 20 歲的人)
            if (age != null)
            {
                // 計算出生日期界線：要在 (今天 - age) 之前出生的人，年齡才會大於 age
                // 把 DateTime 轉成 DateOnly
                var dateLimit = DateOnly.FromDateTime(DateTime.Today.AddYears(-age.Value));

                query = query.Where(v => v.BirthDate.HasValue && v.BirthDate < dateLimit);

                // 將搜尋條件傳回 View，以便在畫面上顯示 "目前的搜尋條件: 大於 20 歲"
                ViewData["AgeFilter"] = age;
            }

            // 回傳 Index 視圖顯示結果
            return View("Index", await query.ToListAsync());
        }

        // ==========================================
        // 修改 2: Details 加入配音角色關聯
        // ==========================================
        // GET: VoiceActors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiceActor = await _context.VoiceActors
                // 加入關聯：載入此聲優配音的角色
                .Include(v => v.Characters)
                // 再加入關聯：載入那些角色所屬的樂團 (這樣詳細頁面就能顯示很完整的資訊)
                .ThenInclude(c => c.Band)
                .FirstOrDefaultAsync(m => m.VoiceActorId == id);

            if (voiceActor == null)
            {
                return NotFound();
            }

            return View(voiceActor);
        }

        // ==========================================
        // 以下為標準 CRUD 功能 (保持不變)
        // ==========================================

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