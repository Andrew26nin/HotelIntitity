using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelIntitity.Data;
using HotelIntitity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelIntitity.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ValuesController(ApplicationDbContext context)
        {
            // Create database context

            _context = context;
            if (!_context.Client.Any())
            {
                _context.Client.Add(new Client { Name = "Филимонов Григорий", Email="test@gmail.com" });
                _context.Client.Add(new Client { Name = "Горин Сергей", Email = "test@gmail2.com" });
                _context.SaveChanges();
            }
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return _context.Client.ToList();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            Client client = _context.Client.FirstOrDefault(x => x.Id == id);
            if (client == null)
                return NotFound();
            return new ObjectResult(client);
        }

        // POST api/<controller>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post([FromBody] Client client)
        {
            //if (ModelState.IsValid)
            //{
            if (client == null)
            {
                return BadRequest();
            }
            _context.Client.Add(client);
                _context.SaveChanges();
                return Ok(client);
                //return RedirectToAction(nameof(Index));
            //}
            //return View(client);
   

        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }
            if (!_context.Client.Any(x => x.Id == client.Id))
            {
                return NotFound();
            }

            _context.Update(client);
            _context.SaveChanges();
            return Ok(client);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Client client = _context.Client.FirstOrDefault(x => x.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            _context.Client.Remove(client);
            _context.SaveChanges();
            return Ok(client);
        }



        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}
