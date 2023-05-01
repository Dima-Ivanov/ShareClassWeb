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
    [Route("api/Solutions/{classRoomId}/{homeTaskId}")]
    [EnableCors]
    [ApiController]
    public class SolutionsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public SolutionsController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] int classRoomId, [FromRoute] int homeTaskId)
        {
            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Conflict(new { message = "You are not signed in!" });
            }

            var classRoomsUsers = await _context.ClassRoomsUsers.GetListAsync();
            var userInClassRoom = classRoomsUsers.FirstOrDefault(i => i.ClassRoom.ID == classRoomId && i.User.Id == curentUser.Id);

            if (userInClassRoom == null)
            {
                Conflict(new { message = "You are not in this ClassRoom!" });
            }

            var solutions = await _context.Solutions.GetListAsync();
            solutions = solutions.Where(i => i.HomeTask.ID == homeTaskId).ToList();
            var users = await _context.Users.GetListAsync();

            var solutionsWithUsers = from solution in solutions
                                     join user in users on solution.UserID equals user.Id
                                     select new { solution = solution, userName = user.Login };

            return Ok(solutionsWithUsers);
        }

        [HttpGet("{solutionId}")]
        public async Task<IActionResult> GetSolution([FromRoute] int classRoomId, [FromRoute] int homeTaskId, [FromRoute] int solutionId)
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

            var solution = await _context.Solutions.GetItemAsync(solutionId);

            if (solution == null)
            {
                return NotFound();
            }

            return Ok(new { solution = solution, userName = curentUser.Login });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] int classRoomId, [FromRoute] int homeTaskId, [FromBody] Solution solution)
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
                return Conflict(new { message = "No HomeTask with id: " + homeTaskId });
            }

            solution.HomeTask = homeTask;
            solution.UserID = curentUser.Id;

            await _context.Solutions.CreateAsync(solution);

            return CreatedAtAction("GetSolution", new { classRoomId = classRoomId, homeTaskId = homeTaskId, solutionId = solution.ID }, new { solution = solution, userName = curentUser.Login });
        }

        [HttpPut("{solutionId}")]
        public async Task<IActionResult> Update([FromRoute] int classRoomId, [FromRoute] int homeTaskId, [FromRoute] int solutionId, [FromBody] Solution solution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toUpdateAsync = await _context.Solutions.GetItemAsync(solutionId);

            if (toUpdateAsync == null)
            {
                return NotFound();
            }

            solution.ID = solutionId;

            await _context.Solutions.UpdateAsync(solution);

            return NoContent();
        }

        [HttpDelete("{solutionId}")]
        public async Task<IActionResult> Delete([FromRoute] int classRoomId, [FromRoute] int homeTaskId, [FromRoute] int solutionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteResult = await _context.Solutions.DeleteAsync(solutionId);

            if (deleteResult)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
