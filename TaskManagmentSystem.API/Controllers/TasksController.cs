using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagmentSystem.API.DTOs.Tasks;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskManagmentSystem.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService, IMapper mapper)
        {
            this._taskService = taskService;
            this._mapper = mapper;
        }

        #region POST EndPoint
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _mapper.Map<Entities.TodoItem>(taskDto);

            #region Business logic 
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            task.UserId = userId;

            #endregion
            var result = await _taskService.CreateAsync(task);

            return Ok("Task Created");
            //return CreatedAtAction(nameof(GetByIdAsync), new { id = task.Id }, task);
        }

        #endregion

        #region GET -Endpoint 
        [HttpGet]
        [Authorize]  // Make sure this is here!
        public async Task<IActionResult> GetAllAsync()
        {
            // Now this will work correctly
            string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized("Invalid or missing user ID in token.");
            }

            var tasks = await _taskService.GetAllAsync(userId);
            var responseTask = _mapper.Map<IEnumerable<ResponseTaskDto>>(tasks);

            return Ok(responseTask);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var task = await _taskService.GetByIdAsync(id, userId);
            if (task == null)
                return NotFound();
            var resposneTask = _mapper.Map<ResponseTaskDto>(task);
            return Ok(resposneTask);
        }
        #endregion

        #region HttpPut 
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = _mapper.Map<Entities.TodoItem>(taskDto);

            var isUpdated = await _taskService.UpdateAsync(id, task);
            if (!isUpdated)
            {
                return BadRequest("Update Failed");
            }
            return Ok("Update is Successful");
        }
        #endregion

        #region HttpDelete 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isDeleted = await _taskService.DeleteAsync(id, userId);
            if (!isDeleted)
            {
                return BadRequest("Delete failed");
            }
            return Ok("Delete is successful");
        }
        #endregion

    }
}