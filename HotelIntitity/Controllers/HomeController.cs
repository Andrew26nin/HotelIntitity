using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using HotelIntitity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelIntitity.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            // Create database context
            _context = context;

            if (!_context.Room.Any())
            {
                _context.Room.Add(new Room { RoomTypeId = 1 });
                _context.Room.Add(new Room { RoomTypeId = 2 });
                _context.SaveChanges();
            }

            if (!_context.Client.Any())
            {
                _context.Client.Add(new Client { Name = "Andrew", Email = "test1@test.com" });
                _context.Client.Add(new Client { Name = "Nick", Email = "test2@test.com" });
                _context.SaveChanges();
            }
            if (!_context.Booking.Any())
            {
                _context.Booking.Add(new Booking { ClientId = 1, RoomId = 1, IsActive = true, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3) });
                _context.Booking.Add(new Booking { ClientId = 2, RoomId = 2, IsActive = true, StartDate = DateTime.Today.AddDays(4), EndDate = DateTime.Today.AddDays(5) });
                _context.SaveChanges();
            }

        }






        // GET: Bookings
        public async Task<IActionResult> Index(int? id)
        {
            var bookings = _context.Booking.Include(b => b.Client).Include(b => b.Room).Include(b => b.Room.RoomType);


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
                .Include(b => b.Room.RoomType)
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
            ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Type", "Type");
            // ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name");
            return View();
        }




        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,Name,Email,RoomType")] BookingViewModel bookingVM)
        {
            Booking booking = new Booking();
            if (ModelState.IsValid)
            {
                var client = new Client { Name = bookingVM.Name, Email = bookingVM.Email };
                _context.Client.Add(client);
                _context.SaveChanges();
                int roomId = -1;
                DateTime startDate = bookingVM.StartDate.AddHours(12);
                DateTime endDate = bookingVM.EndDate.AddHours(12);

                if (startDate < DateTime.Today || startDate > endDate || endDate < DateTime.Today)
                {
                    ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Type", "Type", bookingVM.RoomType);
                    // ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
                    ViewBag.Status = $"Дата заезда не может быть до {DateTime.Today.Date.ToShortDateString()} или позже даты выезда.";
                    return View(bookingVM);
                }

                var currentRoomType = _context.Room.Where(b => b.RoomType.Type == bookingVM.RoomType);
                var activeBookings = _context.Booking.Where(b => b.IsActive);
                foreach (var room in currentRoomType)
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
                    booking.StartDate = startDate;
                    booking.EndDate = endDate;

                    //var currentClient = _context.Client.SingleOrDefaultAsync(cl => cl.Name == bookingVM.Name && cl.Email == bookingVM.Email);

                    booking.ClientId = client.Id;
                    _context.Booking.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Type", "Type", bookingVM.RoomType);
            // ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
            ViewBag.Status = "Создание записи невозможна. Нет свободных номеров.";
            return View(booking);
        }




        //// GET: Bookings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var booking = await _context.Booking
        //        .Include(b => b.Client)
        //        .Include(b => b.Room)
        //        .Include(b => b.Room.RoomType).SingleOrDefaultAsync(m => m.Id == id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookingVM = new BookingViewModel
        //    {
        //        Id =booking.Id,
        //        Email =booking.Client.Email,
        //        EndDate =booking.EndDate,
        //        StartDate =booking.StartDate,
        //        RoomType =booking.Room.RoomType.Type,
        //        Name =booking.Client.Name
        //    };

        //    //ViewData["ClientName"] = new SelectList(_context.Set<Client>(), "Id", "Name", booking.ClientId);
        //    ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Type", "Type", bookingVM.RoomType);
        //    return View(bookingVM);
        //}

        //// POST: Bookings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("StartDate,EndDate,Name,Email,RoomType,Id")] BookingViewModel bookingVM)
        //{
        //    if (id != bookingVM.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var booking = new Booking {Id=id, };


        //        try
        //        {
        //            _context.Update(booking);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookingExists(booking.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RoomType"] = new SelectList(_context.Set<RoomType>(), "Type", "Type", bookingVM.RoomType);

        //    return View(bookingVM);
        //}


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
                .Include(b => b.Room.RoomType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            decimal cash = (booking.EndDate.Day - booking.StartDate.Day) * booking.Room.RoomType.Cost;
            decimal cashToday = 0.0M;
            if (DateTime.Today > booking.StartDate)
            {
                cashToday = (DateTime.Today.Day - booking.StartDate.Day) * booking.Room.RoomType.Cost;
                ViewBag.CashToday = cashToday.ToString();
            }
            else { ViewBag.CashToday = "-"; }

            ViewBag.Cash = cash.ToString();

            return View(booking);
        }


        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Client)
                .SingleOrDefaultAsync(m => m.Id == id);
            var client = _context.Client.SingleOrDefault(c => c.Id == booking.ClientId);


            _context.Booking.Remove(booking);
            _context.Client.Remove(client);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

















        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
