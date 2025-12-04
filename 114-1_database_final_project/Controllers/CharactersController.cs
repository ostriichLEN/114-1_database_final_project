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
    public class CharactersController : Controller
    {
        private readonly Character1Context _context;

        public CharactersController(Character1Context context)
        {
            _context = context;
        }

        // ==========================================
        // 修改 1: Index 維持不變 (或可加入預設排序)
        // ==========================================
        // GET: Characters
        public async Task<IActionResult> Index()
        {
            var character1Context = _context.Characters
                .Include(c => c.Band)
                .Include(c => c.VoiceActor);
            return View(await character1Context.ToListAsync());
        }

        // ==========================================
        // 新增功能: 依位置搜尋 (對應 Python 的 position_query_frame)
        // ==========================================
        // GET: Characters/SearchByPosition
        public async Task<IActionResult> SearchByPosition(string position)
        {
            // 建立查詢
            var query = _context.Characters
                .Include(c => c.Band)
                .Include(c => c.VoiceActor)
                .AsQueryable();

            // 如果有輸入位置 (例如 "guitar", "vocal")，就進行篩選
            if (!string.IsNullOrEmpty(position))
            {
                query = query.Where(c => c.BandPosition == position);
            }

            // 回傳 Index 視圖顯示結果
            // 可以在 View 上方顯示 "目前的搜尋條件: position"
            ViewData["CurrentFilter"] = position;
            return View("Index", await query.ToListAsync());
        }

        // ==========================================
        // 修改 2: Details 加入親屬關係查詢 (對應 relation_frame)
        // ==========================================
        // GET: Characters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.Band)
                .Include(c => c.VoiceActor)
                .FirstOrDefaultAsync(m => m.CharacterId == id);

            if (character == null)
            {
                return NotFound();
            }

            // --- 新增：查詢此角色的親屬關係 ---
            // 對應 SQL: SELECT * FROM relation WHERE id_1 = ? OR id_2 = ?
            var relations = await _context.Relations
                .Where(r => r.Id1 == id || r.Id2 == id)
                .ToListAsync();

            // 將親屬資料傳給 View (View 需自行處理顯示邏輯，例如顯示 ID 或嘗試對應名字)
            ViewBag.Relations = relations;
            // -------------------------------

            return View(character);
        }

        // ==========================================
        // 修改 3: Create 優化下拉選單顯示文字
        // ==========================================
        // GET: Characters/Create
        public IActionResult Create()
        {
            // 修改 SelectList 參數：顯示 BandName 而不是 BandId
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName");

            // 修改 SelectList 參數：顯示聲優的 LastName (或可組合成全名)
            // 這裡示範用 LastName，若要全名需在 View 或 Model 處理
            ViewData["VoiceActorId"] = new SelectList(_context.VoiceActors, "VoiceActorId", "LastName");

            return View();
        }

        // POST: Characters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacterId,FirstName,LastName,Birthdate,Height,VoiceActorId,BandId,BandPosition")] Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", character.BandId);
            ViewData["VoiceActorId"] = new SelectList(_context.VoiceActors, "VoiceActorId", "LastName", character.VoiceActorId);
            return View(character);
        }

        // ==========================================
        // 修改 4: Edit 同步優化下拉選單
        // ==========================================
        // GET: Characters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", character.BandId);
            ViewData["VoiceActorId"] = new SelectList(_context.VoiceActors, "VoiceActorId", "LastName", character.VoiceActorId);
            return View(character);
        }

        // POST: Characters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CharacterId,FirstName,LastName,Birthdate,Height,VoiceActorId,BandId,BandPosition")] Character character)
        {
            if (id != character.CharacterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.CharacterId))
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
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", character.BandId);
            ViewData["VoiceActorId"] = new SelectList(_context.VoiceActors, "VoiceActorId", "LastName", character.VoiceActorId);
            return View(character);
        }

        // GET: Characters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.Band)
                .Include(c => c.VoiceActor)
                .FirstOrDefaultAsync(m => m.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character != null)
            {
                _context.Characters.Remove(character);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.CharacterId == id);
        }
    }
}