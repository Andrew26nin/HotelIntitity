using Data.Models;
using HotelIntitity.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            // Create database context
            _context = context;
        }






        // GET: Bookings
        public async Task<IActionResult> Index(int? id)
        {
            var bookings = _context.Booking.Include(b => b.Client).Include(b => b.Room);

        
            {
                return View(await bookings.ToListAsync());
            }
            
        }









        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Client)
                .Include(b => b.Room)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }





        // GET: Bookings/Create
        public IActionResult Create()
        {
            //ViewData["ClientId"] = new SelectList(_context.Set<Client>(), "Id", "Id");
            //ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Id");
            // ViewData["RoomType"] = new SelectList(_context.Set<Room>(), "Id", "Type");
            ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name");
            return View();
        }




        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,ClientId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                int roomId = -1;
                DateTime startDate = booking.StartDate.AddHours(12);
                DateTime endDate = booking.EndDate.AddHours(12);

                if (startDate < DateTime.Today || startDate > endDate)
                {
                    ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
                    ViewBag.Status = $"Дата заезда не может быть до {DateTime.Today.Date.ToString()} или позже даты выезда.";
                    return View(booking);
                }

                var activeBookings = _context.Booking.Where(b => b.IsActive);
                foreach (var room in _context.Room)
                {
                    var activeBookingsForCurrentRoom = activeBookings.Where(b => b.RoomId == room.Id);
                    if (activeBookingsForCurrentRoom.All(b => startDate < b.StartDate &&
                        endDate < b.StartDate || startDate > b.EndDate && endDate > b.EndDate))
                    {
                        roomId = room.Id;
                        break;
                    }
                }

                if (roomId >= 0)
                {
                    booking.RoomId = roomId;
                    booking.IsActive = true;
                    _context.Booking.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
            ViewBag.Status = "Создание записи невозможна. Нет свободных номеров.";
            return View(booking);
        }




        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.SingleOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Id", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartDate,EndDate,IsActive,ClientId,RoomId,Id")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Id", booking.RoomId);
            return View(booking);
        }


        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Client)
                .Include(b => b.Room)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            //int cash = (booking.EndDate.Day - booking.StartDate.Day) * booking.Room.Cost;
            //ViewBag.Cash = cash.ToString();
            return View(booking);
        }


        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.SingleOrDefaultAsync(m => m.Id == id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }


    }
}