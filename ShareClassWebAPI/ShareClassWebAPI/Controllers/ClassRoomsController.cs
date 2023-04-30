using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShareClassWebAPI;
using ShareClassWebAPI.Entities;

namespace ShareClassWebAPI.Controllers
{
    [Route("api/ClassRooms")]
    [EnableCors]
    [ApiController]
    public class ClassRoomsController : Controller
    {
        private readonly DataContext _context;

        public ClassRoomsController(DataContext context)
        {
            _context = context;

            if (context.ClassRooms.GetListAsync().Result.Count == 0)
            {
                context.ClassRooms.CreateAsync(new ClassRoom
                {
                    Name = "Maths",
                    Description = "We share maths here",
                    Teacher_Name = "Ivanov",
                    Creation_Date = DateTime.Now,
                    InvitationCode = Guid.NewGuid(),
                    Students_Count = 0
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.ClassRooms.GetListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassRoom([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classRoom = await _context.ClassRooms.GetItemAsync(id);

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

            if (classRoom.InvitationCode == Guid.Empty)
            {
                classRoom.InvitationCode = Guid.NewGuid();
            }

            if (classRoom.Creation_Date == DateTime.MinValue)
            {
                classRoom.Creation_Date = DateTime.Now;
            }

            await _context.ClassRooms.CreateAsync(classRoom);

            return CreatedAtAction("GetClassRoom", new { id = classRoom.ID }, classRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ClassRoom classRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toUpdateAsync = await _context.ClassRooms.GetItemAsync(id);

            if (toUpdateAsync == null)
            {
                return NotFound();
            }

            classRoom.ID = id;

            await _context.ClassRooms.UpdateAsync(classRoom);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.adminRole)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteResult = await _context.ClassRooms.DeleteAsync(id);

            if (deleteResult)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
