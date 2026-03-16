using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagmentSystem.API.DTOs.Tasks;
using TaskManagmentSystem.API.Interfaces.Repositories;

namespace TaskManagmentSystem.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TasksController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            this._mapper = mapper;
        }

        #region POST EndPoint
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto taskDto)
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
            await _taskRepository.AddAsync(task);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        #endregion

        #region GET -Endpoint 
        [Authorize(Roles = "User , Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tasks = await _taskRepository.GetAllAsync(userId);
            var responseTask = _mapper.Map<IEnumerable<ResponseTaskDto>>(tasks);

            return Ok(responseTask);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var task = await _taskRepository.GetById(id, userId);
            if (task == null)
                return NotFound();
            var resposneTask = _mapper.Map<ResponseTaskDto>(task);
            return Ok(resposneTask);
        }
        #endregion

        #region HttpPut 
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = _mapper.Map<Entities.TodoItem>(taskDto);

            var isUpdated = await _taskRepository.UpdateAsync(id, task);
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var existingTask = await _taskRepository.GetById(id, userId);
            if (existingTask == null)
                return NotFound();

            var isDeleted = await _taskRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Delete failed");
            }
            return Ok("Delete is successful");
        }
        #endregion

    }
}