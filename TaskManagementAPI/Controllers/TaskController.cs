using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using System.Net.Sockets;
using System.Threading.Tasks;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Authorize] //JWT access control
    [RequiredScope("access_as_user")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        //Add DB context
        private readonly TaskDbContext _context;

        public TaskController(TaskDbContext context)
        {
            _context = context;
        }

        //GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            //return await _context.Tasks.ToListAsync();

            var query = await (
                from task in _context.Tasks
                where task.Last_Completed == null
                select new Models.Task
                {
                    TaskId = task.TaskId,
                    Description = task.Description,
                    Details = task.Details,
                    Due_Date = task.Due_Date,
                    Frequency_Type = task.Frequency_Type,
                    Frequency_Number = task.Frequency_Number,
                    Sensative = task.Sensative,
                    Last_Completed = task.Last_Completed
                }).ToListAsync();

            return Ok(query);
        }

        //GET: api/Task/NonSensative
        [HttpGet("NonSensative")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetNonSensitiveTasks()
        {
            var query = await (
                from task in _context.Tasks
                where !task.Sensative
                select new Models.Task
                {
                    TaskId = task.TaskId,
                    Description = task.Description,
                    Details = task.Details,
                    Due_Date = task.Due_Date,
                    Frequency_Type = task.Frequency_Type,
                    Frequency_Number = task.Frequency_Number,
                    Sensative = task.Sensative,
                    Last_Completed = task.Last_Completed
                }).ToListAsync();

            return Ok(query);
        }

        // PUT api/UpdateTask
        [HttpPatch("UpdateTask")]
        public async Task<ActionResult> UpdateTicket(Models.Task taskUpdate)
        {
            Models.Task? existingTask = await _context.Tasks.FindAsync(taskUpdate.TaskId);

            if (existingTask == null)
            {
                return NotFound();
            }

            //existingTask.Last_Completed = taskUpdate.Last_Completed;
            existingTask.Last_Completed = DateTime.Now;

            using (_context)
            {
                _context.Entry(existingTask).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        //GET: api/Task/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

    }
}
