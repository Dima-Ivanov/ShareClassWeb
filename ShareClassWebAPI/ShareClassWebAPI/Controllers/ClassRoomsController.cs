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
    [Route("api/ClassRooms")]
    [EnableCors]
    [ApiController]
    public class ClassRoomsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public ClassRoomsController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Conflict(new { message = "You are not signed in!" });
            }

            var list = await _context.ClassRooms.GetListAsync();

            var roles = await _userManager.GetRolesAsync(curentUser);
            if (roles != null && roles.Count > 0)
            {
                var userRole = roles[0];
                if (userRole == Constants.adminRole)
                {
                    return Ok(list);
                }
            }

            var classRoomsUsers = await _context.ClassRoomsUsers.GetListAsync();
            var classRoomsIds = new HashSet<int>();
            foreach (var classRoomsUser in classRoomsUsers.Where(cru => cru.User.Id == curentUser.Id))
            {
                classRoomsIds.Add(classRoomsUser.ClassRoom.ID);
            }

            list = list.Where(i => classRoomsIds.Contains(i.ID)).ToList();

            return Ok(list);
        }

        [HttpPost("Join/{invitationCode}")]
        public async Task<IActionResult> JoinClassRoom([FromRoute] string invitationCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Guid.TryParse(invitationCode, out var invitationGuid))
            {
                var classRooms = await _context.ClassRooms.GetListAsync();
                var classRoomToJoin = classRooms.FirstOrDefault(cr => cr.InvitationCode == invitationGuid);
                if (classRoomToJoin != null)
                {
                    var curentUser = await _userManager.GetUserAsync(HttpContext.User);

                    if (curentUser == null)
                    {
                        return Conflict(new { message = "You are not signed in!" });
                    }

                    var classRoomsUsers = await _context.ClassRoomsUsers.GetListAsync();

                    var checkIfUserAlreadyJoined = classRoomsUsers.FirstOrDefault(cru => cru.ClassRoom.ID == classRoomToJoin.ID && cru.User.Id == curentUser.Id);

                    if (checkIfUserAlreadyJoined != null)
                    {
                        return Conflict(new { message = "User is already in ClassRoom: " + classRoomToJoin.Name });
                    }

                    ClassRoomsUsers classRoomsUser = new ClassRoomsUsers() { ClassRoom = classRoomToJoin, User = curentUser };

                    classRoomToJoin.Students_Count++;
                    await _context.ClassRooms.UpdateAsync(classRoomToJoin);
                    await _context.ClassRoomsUsers.CreateAsync(classRoomsUser);

                    return Ok(classRoomToJoin);
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpPost("Leave/{id}")]
        public async Task<IActionResult> LeaveClassRoom([FromRoute] int id)
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

            var classRoomToLeave = await _context.ClassRooms.GetItemAsync(id);

            if (classRoomToLeave == null)
            {
                return Conflict(new { message = "No ClassRoom with id: " + id });
            }

            var classRoomsUsersList = await _context.ClassRoomsUsers.GetListAsync();

            var classRoomsUsersToDelete = classRoomsUsersList.FirstOrDefault(cru => cru.ClassRoom.ID == classRoomToLeave.ID && cru.User.Id == curentUser.Id);

            if (classRoomsUsersToDelete == null)
            {
                return Conflict(new { message = "You are not in this ClassRoom" });
            }

            await _context.ClassRoomsUsers.DeleteAsync(classRoomsUsersToDelete.ID);

            classRoomToLeave.Students_Count--;

            await _context.ClassRooms.UpdateAsync(classRoomToLeave);

            return Ok(classRoomToLeave);
        }

        [HttpGet("IsAdministrator/{id}")]
        public async Task<IActionResult> IsAdministrator([FromRoute] int id)
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

            var classRoom = await _context.ClassRooms.GetItemAsync(id);

            if (classRoom == null)
            {
                return Conflict(new { message = "No ClassRoom with id: " + id });
            }

            return Ok(new { isAdministrator = classRoom.Administrator_ID == curentUser.Id });
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

            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Conflict(new { message = "You are not signed in!" });
            }

            classRoom.Administrator_ID = curentUser.Id;

            if (classRoom.InvitationCode == Guid.Empty)
            {
                classRoom.InvitationCode = Guid.NewGuid();
            }

            if (classRoom.Creation_Date == DateTime.MinValue)
            {
                classRoom.Creation_Date = DateTime.Now;
            }

            classRoom.Students_Count = 1;

            await _context.ClassRooms.CreateAsync(classRoom);

            ClassRoomsUsers classRoomsUsers = new ClassRoomsUsers() { ClassRoom = classRoom, User = curentUser };

            await _context.ClassRoomsUsers.CreateAsync(classRoomsUsers);

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
