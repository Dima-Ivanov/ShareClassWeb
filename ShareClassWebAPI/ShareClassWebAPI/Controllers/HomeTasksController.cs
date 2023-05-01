using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShareClassWebAPI;
using ShareClassWebAPI.Entities;

namespace ShareClassWebAPI.Controllers
{
    [Route("api/HomeTasks/{classRoomId}")]
    [EnableCors]
    [ApiController]
    public class HomeTasksController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public HomeTasksController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] int classRoomId)
        {
            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Conflict(new { message = "You are not signed in!" });
            }

            var homeTasks = await _context.HomeTasks.GetListAsync();

            var roles = await _userManager.GetRolesAsync(curentUser);
            if (roles != null && roles.Count > 0)
            {
                var userRole = roles[0];
                if (userRole == Constants.adminRole)
                {
                    return Ok(homeTasks);
                }
            }

            var classRoomsUsers = await _context.ClassRoomsUsers.GetListAsync();
            var userInClassRoom = classRoomsUsers.FirstOrDefault(i => i.ClassRoom.ID == classRoomId && i.User.Id == curentUser.Id);

            if (userInClassRoom == null)
            {
                Conflict(new { message = "You are not in this ClassRoom!" });
            }

            homeTasks = homeTasks.Where(i => i.ClassRoom.ID == classRoomId).ToList();

            return Ok(homeTasks);
        }

        [HttpGet("{homeTaskId}")]
        public async Task<IActionResult> GetHomeTask([FromRoute] int homeTaskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Conflict(new { message = "You are not signed in!" });
            }

            var homeTask = await _context.HomeTasks.GetItemAsync(homeTaskId);

            if (homeTask == null)
            {
                return NotFound();
            }

            return Ok(homeTask);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] int classRoomId, [FromBody] HomeTask homeTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Conflict(new { message = "You are not signed in!" });
            }

            if (homeTask.Creation_Date == DateTime.MinValue)
            {
                homeTask.Creation_Date = DateTime.Now;
            }

            var classRoom = await _context.ClassRooms.GetItemAsync(classRoomId);

            if (classRoom == null)
            {
                return Conflict(new { message = "No ClassRoom with id: " + classRoomId });
            }

            homeTask.ClassRoom = classRoom;

            await _context.HomeTasks.CreateAsync(homeTask);

            return CreatedAtAction("GetHomeTask", new { classRoomId = classRoomId, homeTaskId = homeTask.ID }, homeTask);
        }

        [HttpPut("{homeTaskId}")]
        public async Task<IActionResult> Update([FromRoute] int classRoomId, [FromRoute] int homeTaskId, [FromBody] HomeTask homeTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toUpdateAsync = await _context.HomeTasks.GetItemAsync(homeTaskId);

            if (toUpdateAsync == null)
            {
                return NotFound();
            }

            homeTask.ID = homeTaskId;

            await _context.HomeTasks.UpdateAsync(homeTask);

            return NoContent();
        }

        [HttpDelete("{homeTaskId}")]
        public async Task<IActionResult> Delete([FromRoute] int classRoomId, [FromRoute] int homeTaskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteResult = await _context.HomeTasks.DeleteAsync(homeTaskId);

            if (deleteResult)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
