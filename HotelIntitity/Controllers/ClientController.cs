using HotelIntitity.Data;
using HotelIntitity.Models;
using HotelIntitity.ViewModels.FilterViewModel;
using HotelIntitity.ViewModels.FilterViewModel.ClientVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelIntitity.Controllers
{
    [Authorize(Roles = "admin")]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            // Create database context

            _context = context;
        }




        // GET: Clients
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Client.ToListAsync());
        //}




        #region Test

        public async Task<IActionResult> Index(string email, string name, int page = 1,
             SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 3;

            //фильтрация
            IQueryable<Client> clients = _context.Client;

          
            if (!String.IsNullOrEmpty(name))
            {
                clients = clients.Where(p => p.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(email))
            {
                clients = clients.Where(p => p.Email.Contains(email));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    clients = clients.OrderByDescending(s => s.Name);
                    break;
                case SortState.EmailAsc:
                    clients = clients.OrderBy(s => s.Email);
                    break;
                case SortState.EmailDesc:
                    clients = clients.OrderByDescending(s => s.Email);
                    break;
                
                default:
                    clients = clients.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            var count = await clients.CountAsync();
            var items = await clients.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, email),
                Clients = items
            };
            return View(viewModel);
        }


        #endregion





        #region API Detail
        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Client = await _context.Client
                .SingleOrDefaultAsync(m => m.Id == id);
            if (Client == null)
            {
                return NotFound();
            }

            return View(Client);
        }
        #endregion

        #region API Create
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Client.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
        #endregion

        #region API Edit
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Client = await _context.Client.SingleOrDefaultAsync(m => m.Id == id);
            if (Client == null)
            {
                return NotFound();
            }
            return View(Client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }
        #endregion

        #region API Delete
        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Client = await _context.Client
                .SingleOrDefaultAsync(m => m.Id == id);
            if (Client == null)
            {
                return NotFound();
            }

            return View(Client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Client = await _context.Client.SingleOrDefaultAsync(m => m.Id == id);
            _context.Client.Remove(Client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }

        #endregion
    }
}