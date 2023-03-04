using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShareClassWebAPI;
using ShareClassWebAPI.Entities;

namespace ShareClassWebAPI.Controllers
{
    [Route("api/ClassRooms")]
    [ApiController]
    public class ClassRoomsController : Controller
    {
        private readonly DataContext _context;

        public ClassRoomsController(DataContext context)
        {
            _context = context;
            if (context.ClassRooms.GetList().Count() == 0)
            {
                context.ClassRooms.Create(new ClassRoom
                {
                    Name = "Maths",
                    Description = "We share maths here",
                    Teacher_Name = "Ivanov",
                    Creation_Date = DateTime.Now,
                    InvitationCode = Guid.NewGuid(),
                    Students_Count = 0
                });
                context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.DBClassRoom.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassRoom([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classRoom = await _context.DBClassRoom.SingleOrDefaultAsync(c => c.ID == id);

            if (classRoom == null)
            {
                return NotFound();
            }

            return Ok(classRoom);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassRoom classRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = _context.ClassRooms.Create(classRoom);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassRoom", new { id = newId }, classRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ClassRoom classRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toUpdate = await _context.DBClassRoom.SingleOrDefaultAsync(c => c.ID == id);

            if (toUpdate == null)
            {
                return NotFound();
            }

            classRoom.ID = id;

            _context.ClassRooms.Update(classRoom);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.ClassRooms.Delete(id))
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return NotFound();
        }
    }
}
