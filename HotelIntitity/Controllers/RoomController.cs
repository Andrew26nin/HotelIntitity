using Data.Models;
using HotelIntitity.ViewModels.FilterViewModel;
using HotelIntitity.ViewModels.FilterViewModel.RoomVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public  RoomController(ApplicationDbContext context)
        {
            // Create database context
          
            _context = context;

           
        }



        // GET: Rooms
        public async Task<IActionResult> Index(int? roomtype, int id, int page = 1,
            SortState sortOrder = SortState.IdAsc)
        {
            int pageSize = 3;

            //фильтрация
            IQueryable<Room> rooms = _context.Room.Include(x => x.RoomType);

            if (roomtype != null && roomtype != 0)
            {
                rooms = rooms.Where(p => p.RoomTypeId == roomtype);
            }
            if (id != null && id != 0)
            {
                rooms = rooms.Where(p => p.Id==id);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.IdDesc:
                    rooms = rooms.OrderByDescending(s => s.Id);
                    break;
               
                case SortState.TypeAsc:
                    rooms = rooms.OrderBy(s => s.RoomType.Type);
                    break;
                case SortState.TypeDesc:
                    rooms = rooms.OrderByDescending(s => s.RoomType.Type);
                    break;
                default:
                    rooms = rooms.OrderBy(s => s.Id);
                    break;
            }

            // пагинация
            var count = await rooms.CountAsync();
            var items = await rooms.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(_context.RoomType.ToList(), roomtype, id),
                Rooms = items
            };
            return View(viewModel);
        }





        // GET: Rooms
        //public async Task<IActionResult> Index()
        //{
          
        //    return View(await _context.Room.ToListAsync());
        //}




        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.Include(x => x.RoomType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }




        // GET: Rooms/Create
        public IActionResult Create()
        {
            var q = _context.Room.ToList().Max(i => i.Id)+1;
            ViewBag.Id = q.ToString();
            ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Id", "Type");
            return View();
        }

        // POST: Rooms/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomTypeId")] Room room)
        {
            var q = _context.Room.ToList().Max(i => i.Id) + 1;
            if (ModelState.IsValid)
            {
               // room.Id = q;
                _context.Room.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }




        // GET: Rooms/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.SingleOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Id", "Type");
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomTypeId")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Room.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }





        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(x => x.RoomType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.SingleOrDefaultAsync(m => m.Id == id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.Id == id);
        }
    }
}
